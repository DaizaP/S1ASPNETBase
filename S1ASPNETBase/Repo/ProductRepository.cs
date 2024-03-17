using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using S1ASPNETBase.Abstraction;
using S1ASPNETBase.Dto;
using S1ASPNETBase.Models;
using S1ASPNETBase.Services.FileExtractor;

namespace S1ASPNETBase.Repo
{
    public class ProductRepository : IProductRepository
    {
        readonly private IMapper _mapper;
        readonly private IMemoryCache _memoryCache;
        readonly private MarketModelsDbContext _marketModelsDbContext;
        public ProductRepository(IMapper mapper, IMemoryCache memoryCache, MarketModelsDbContext marketModelsDbContext)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _marketModelsDbContext = marketModelsDbContext;
        }

        public int AddProduct(ProductDto productDto)
        {
            var entityProduct = _marketModelsDbContext.Products
                .FirstOrDefault(
                x => x.Name.ToLower() == productDto.Name.ToLower()
                );
            if (entityProduct == null)
            {
                entityProduct = _mapper.Map<Product>(productDto);
                _marketModelsDbContext.Products.Add(entityProduct);
                _marketModelsDbContext.SaveChanges();
                _memoryCache.Remove("products");
            }
            return entityProduct.Id;

        }

        public IEnumerable<ProductDto> GetProducts()
        {
            if (_memoryCache.TryGetValue("products", out List<ProductDto> products))
            {
                return products;
            }
            var productList = _marketModelsDbContext.Products.Select(x => _mapper.Map<ProductDto>(x)).ToList();
            _memoryCache.Set("products", productList, TimeSpan.FromMinutes(30));
            return productList;
        }

        public string GetProductsCsv()
        {
            var content = "";
            if (_memoryCache.TryGetValue("products", out List<ProductDto> products))
            {
                content = GetCsv.GetProducts(products);
            }
            else
            {
                var productList = _marketModelsDbContext.Products.Select(x => _mapper.Map<ProductDto>(x)).ToList();
                _memoryCache.Set("products", productList, TimeSpan.FromMinutes(30));
                content = GetCsv.GetProducts(productList);
            }
            return content;
        }
        public bool DelProduct(string name)
        {
            if (!_marketModelsDbContext.Products.Any(x => x.Name.ToLower().Equals(name.ToLower())))
            {
                return false;
            }
            _marketModelsDbContext.Products.Where(x => x.Name.ToLower().Equals(name.ToLower())).ExecuteDelete();
            _memoryCache.Remove("products");
            return true;
        }

        public bool UpdProduct(string name, UpdateProductsDto updateProductsDto)
        {
            if (_marketModelsDbContext.Products.Any(x => x.Name.ToLower().Equals(name.ToLower())))
            {
                _marketModelsDbContext.Products.Where(x => x.Name.ToLower().Equals(name.ToLower()))
                .ExecuteUpdate(setters => setters
                .SetProperty(x => x.Description, updateProductsDto.Description)
                .SetProperty(x => x.Cost, updateProductsDto.Cost)
                .SetProperty(x => x.CategoryId, updateProductsDto.CategoryId));
                _memoryCache.Remove("products");
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
