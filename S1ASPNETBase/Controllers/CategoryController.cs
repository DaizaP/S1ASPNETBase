using Microsoft.AspNetCore.Mvc;
using S1ASPNETBase.Abstraction;
using S1ASPNETBase.Dto;

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
                var result = _categoryRepository.DelCategory(name);
                if (result)
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(409, "Категории с таким именем не существует");
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
