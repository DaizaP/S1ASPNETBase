using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using S1ASPNETBase.Dto;
using S1ASPNETBase.Models;

namespace S1ASPNETBase.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpGet("getProducts")]
        public IActionResult GetProducts()
        {
            try
            {
                var context = new ProductContext();
                IQueryable<Product> products = context.Products.Select(x => new Product
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Category = x.Category,
                    Cost = x.Cost,
                    CategoryId = x.CategoryId,
                    Storages = x.Storages
                });
                return Ok(products);

            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost("postProducts")]
        public IActionResult PostProducts(
            [FromQuery] string name,
            string description,
            int categoryId,
            int cost)
        {
            try
            {
                var context = new ProductContext();
                if (!context.Products.Any(x => x.Name.ToLower().Equals(name.ToLower())))
                {
                    context.Add(new Product()
                    {
                        Name = name,
                        Description = description,
                        CategoryId = categoryId,
                        Cost = cost
                    });
                    context.SaveChanges();
                    context.Dispose();
                    return Ok();
                }
                else
                {
                    return StatusCode(409);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("deleteProducts")]
        public IActionResult DeleteProduct([FromQuery] string name)
        {
            try
            {
                using (var context = new ProductContext())
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
            [FromBody] DtoUpdateProducts dtoUpdateProducts)
        {
            try
            {
                using (var context = new ProductContext())
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
