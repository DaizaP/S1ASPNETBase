using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using S1ASPNETBase.Abstraction;
using S1ASPNETBase.Dto;
using S1ASPNETBase.Models;

namespace S1ASPNETBase.Repo
{
    public class CategoryRepository : ICategoryRepository
    {
        readonly private IMapper _mapper;
        readonly private IMemoryCache _memoryCache;
        readonly private MarketModelsDbContext _marketModelsDbContext;
        public CategoryRepository(IMapper mapper, IMemoryCache memoryCache, MarketModelsDbContext marketModelsDbContext)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _marketModelsDbContext = marketModelsDbContext;
        }
        public int AddCategory(CategoryDto categoryDto)
        {

            var entityCategory = _marketModelsDbContext.Categories
                .FirstOrDefault(
                x => x.Name.ToLower() == categoryDto.Name.ToLower()
                );
            if (entityCategory == null)
            {
                entityCategory = _mapper.Map<Category>(categoryDto);
                _marketModelsDbContext.Categories.Add(entityCategory);
                _marketModelsDbContext.SaveChanges();
                _memoryCache.Remove("categories");
            }
            return entityCategory.Id;

        }

        public bool DelCategory(string name)
        {
            if (_marketModelsDbContext.Categories.Any(x => x.Name.ToLower().Equals(name.ToLower())))
            {
                _marketModelsDbContext.Categories.Where(x => x.Name.ToLower()
                .Equals(name.ToLower())).ExecuteDelete();
                _memoryCache.Remove("categories");
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<CategoryDto> GetCategories()
        {
            if (_memoryCache.TryGetValue("categories", out List<CategoryDto> categories))
            {
                return categories;
            }

            var categoryList = _marketModelsDbContext.Categories.Select(x => _mapper.Map<CategoryDto>(x)).ToList();
            _memoryCache.Set("categories", categoryList, TimeSpan.FromMinutes(30));
            return categoryList;
        }

    }
}
