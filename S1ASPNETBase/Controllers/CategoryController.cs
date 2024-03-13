using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using S1ASPNETBase.Models;

namespace S1ASPNETBase.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        [HttpGet("getCategories")]
        public IActionResult GetCategories()
        {
            try
            {
                var context = new ProductContext();
                IQueryable<Category> categories = context.Categories.Select(x => new Category
                {
                    Id = x.Id,
                    Name = x.Name,
                });
                return Ok(categories);

            }
            catch
            {
                return StatusCode(500);
            }
        }
        [HttpPost("postCategories")]
        public IActionResult PostCategories(
            [FromQuery] string name)
        {
            try
            {
                var context = new ProductContext();
                if (!context.Categories.Any(x => x.Name.ToLower().Equals(name.ToLower())))
                {
                    context.Add(new Category()
                    {
                        Name = name
                    });
                    context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return StatusCode(409);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpDelete("deleteCategories")]
        public IActionResult DeleteCategories([FromQuery] string name)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    if (context.Categories.Any(x => x.Name.ToLower().Equals(name.ToLower())))
                    {
                        context.Categories.Where(x => x.Name.ToLower()
                        .Equals(name.ToLower()))
                            .ExecuteDelete();
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
