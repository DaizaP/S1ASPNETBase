using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using S1ASPNETBase.Abstraction;
using S1ASPNETBase.Dto;
using S1ASPNETBase.Models;

namespace S1ASPNETBase.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("getProducts")]
        public IActionResult GetProducts([FromQuery]bool csv, bool url)
        {
            if (csv)
            {
                var products = _productRepository.GetProductsCsv();
                if (url)
                {
                    string filename = "products" + DateTime.Now.ToBinary().ToString() + ".csv";
                    System.IO.File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles", filename), products);
                    return Ok("https://" + Request.Host.ToString() + "/static/" + filename);
                }
                else
                {
                    return File(new System.Text.UTF8Encoding().GetBytes(products), "text/csv", "products.csv");
                }
            }
            else
            {
                var products = _productRepository.GetProducts();
                return Ok(products);
            }
        }

        [HttpPost("postProducts")]
        public IActionResult PostProducts([FromBody] ProductDto productDto)
        {
            var result = _productRepository.AddProduct(productDto);
            return Ok(result);
        }

        [HttpDelete("deleteProducts")]
        public IActionResult DeleteProduct([FromQuery] string name)
        {
            try
            {
                using (var context = new MarketModelsDtContext())
                {
                    if (context.Products.Any(x => x.Name.ToLower().Equals(name.ToLower())))
                    {
                        context.Products.Where(x => x.Name.ToLower().Equals(name.ToLower())).ExecuteDelete();
                        return Ok();
                    }
                    else
                    {
                        return StatusCode(409);
                    }
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPatch("updateProducts")]
        public IActionResult UpdateProducts(
            [FromQuery] string name,
            [FromBody] UpdateProductsDto dtoUpdateProducts)
        {
            try
            {
                using (var context = new MarketModelsDtContext())
                {
                    if (context.Products.Any(x => x.Name.ToLower().Equals(name.ToLower())))
                    {
                        context.Products.Where(x => x.Name.ToLower().Equals(name.ToLower()))
                        .ExecuteUpdate(setters => setters
                        .SetProperty(x => x.Description, dtoUpdateProducts.Description)
                        .SetProperty(x => x.Cost, dtoUpdateProducts.Cost)
                        .SetProperty(x => x.CategoryId, dtoUpdateProducts.CategoryId));
                        return Ok();
                    }
                    else
                    {
                        return StatusCode(409);
                    }
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
