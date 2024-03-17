using AutoMapper;
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
        public CategoryRepository(IMapper mapper, IMemoryCache memoryCache)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
        }
        public int AddCategory(CategoryDto categoryDto)
        {
            using (var context = new MarketModelsDtContext())
            {
                var entityCategory = context.Categories
                    .FirstOrDefault(
                    x => x.Name.ToLower() == categoryDto.Name.ToLower()
                    );
                if (entityCategory == null)
                {
                    entityCategory = _mapper.Map<Category>(categoryDto);
                    context.Categories.Add(entityCategory);
                    context.SaveChanges();
                    _memoryCache.Remove("categories");
                }
                return entityCategory.Id;
            }

        }
        public IEnumerable<CategoryDto> GetCategories()
        {
            if (_memoryCache.TryGetValue("categories", out List<CategoryDto> categories))
            {
                return categories;
            }

            using (var context = new MarketModelsDtContext())
            {
                var categoryList = context.Categories.Select(x => _mapper.Map<CategoryDto>(x)).ToList();
                _memoryCache.Set("categories", categoryList, TimeSpan.FromMinutes(30));
                return categoryList;
            }
        }
    }
}
