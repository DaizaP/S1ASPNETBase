using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using S1ASPNETBase.Abstraction;
using S1ASPNETBase.Dto;
using S1ASPNETBase.Models;
using S1ASPNETBase.Repo;

namespace S1ASPNETBase.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("getCategories")]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepository.GetCategories();
            return Ok(categories);
        }
        [HttpPost("postCategories")]
        public IActionResult PostCategories([FromBody] CategoryDto categoryDto)
        {
            var result = _categoryRepository.AddCategory(categoryDto);
            return Ok(result);
        }

        [HttpDelete("deleteCategories")]
        public IActionResult DeleteCategories([FromQuery] string name)
        {
            try
            {
                using (var context = new MarketModelsDtContext())
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
