using Microsoft.AspNetCore.Mvc;
using WPO.Application.DTO;
using WPO.Application.Interfaces;
using WPO.Infrastructure.Services;

namespace WebPolyOmnia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryService categoryService) : Controller
    {
        // Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategory()
        {
            var category = await categoryService.GetAllCategories();
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            var category = await categoryService.GetCategoryById(id);
            if (category == null) return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory([FromBody] CategoryDTO model)
        {
            await categoryService.CreateCategory(model);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutCategory(int id, [FromBody] CategoryDTO model)
        {
            if (id != model.Id) return BadRequest(new { message = "ID у маршруті не співпадає з ID у тілі запиту." });

            var category = await categoryService.GetCategoryById(id);
            if (category == null) return NotFound();

            category.Name = model.Name.Trim();
            await categoryService.UpdateCategory(category);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var category = await categoryService.GetCategoryById(id);
            if (category != null)
            {
                await categoryService.DeleteCategory(category);
            }

            return Ok();
        }
    }
}
