using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using S1ASPNETBase.Abstraction;
using S1ASPNETBase.Dto;
using S1ASPNETBase.Models;
using S1ASPNETBase.Services.FileExtractor;
using System.Text;

namespace S1ASPNETBase.Repo
{
    public class ProductRepository : IProductRepository
    {
        readonly private IMapper _mapper;
        readonly private IMemoryCache _memoryCache;
        public ProductRepository(IMapper mapper, IMemoryCache memoryCache)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        public int AddProduct(ProductDto productDto)
        {
            using (var context = new MarketModelsDtContext())
            {
                var entityProduct = context.Products
                    .FirstOrDefault(
                    x => x.Name.ToLower() == productDto.Name.ToLower()
                    );
                if (entityProduct == null)
                {
                    entityProduct = _mapper.Map<Product>(productDto);
                    context.Products.Add(entityProduct);
                    context.SaveChanges();
                    _memoryCache.Remove("products");
                }
                return entityProduct.Id;
            }

        }

        public IEnumerable<ProductDto> GetProducts()
        {
            if (_memoryCache.TryGetValue("products", out List<ProductDto> products))
            {
                return products;
            }
            using (var context = new MarketModelsDtContext())
            {
                var productList = context.Products.Select(x => _mapper.Map<ProductDto>(x)).ToList();
                _memoryCache.Set("products", productList, TimeSpan.FromMinutes(30));
                return productList;
            }
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
                using (var context = new MarketModelsDtContext())
                {
                    var productList = context.Products.Select(x => _mapper.Map<ProductDto>(x)).ToList();
                    _memoryCache.Set("products", productList, TimeSpan.FromMinutes(30));
                    content = GetCsv.GetProducts(productList);
                }
            }
            return content;
        }
    }
}
