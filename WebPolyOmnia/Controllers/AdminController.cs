using Microsoft.AspNetCore.Mvc;
using WPO.Application.DTO;
using WPO.Application.Interfaces;
using WPO.Domain;

namespace WebPolyOmnia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(IUserService userService, IVideoService videoService, ICategoryService categoryService) : Controller
    {
        // Category
        [HttpGet]
        public async Task<IActionResult> Categories()
        {
            var category = await categoryService.GetAllCategories();
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] CategoryDTO model)
        {
            await categoryService.CreateCategory(model);
            return CreatedAtAction(nameof(GetCategpry), new { id = model.Id }, model);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategpry(int id)
        {
            var category = await categoryService.GetCategoryById(id);
            if (category == null) return NotFound();

            return Ok(category);
        }

        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PutCategory(int id, [FromBody] CategoryDTO model)
        {
            if(id != model.Id) return BadRequest(new { message = "ID у маршруті не співпадає з ID у тілі запиту." });

            var category = await categoryService.GetCategoryById(id);
            if (category == null) return NotFound();

            category.Name = model.Name.Trim();
            await categoryService.UpdateCategory(category);

            return NoContent();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await categoryService.GetCategoryById(id);
            if (category != null)
            {
                await categoryService.DeleteCategory(category);
            }

            return CreatedAtAction(nameof(GetCategpry), null);
        }

        // User
        [HttpGet]
        public async Task<IActionResult> ApproveUsers()
        {
            var users = await userService.GetUsersByRole("Unconfirmed");
            return Ok(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveUser(int id, [FromBody] UserDTO userDTO)
        {
            if(id != userDTO.Id) return BadRequest(new { message = "ID у маршруті не співпадає з ID у тілі запиту." });

            await userService.UpdateUserRole(id, "User");
            return CreatedAtAction(nameof(ApproveUsers), null);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RejectUser(int id)
        {
            await userService.DeleteUser(id);
            return CreatedAtAction(nameof(ApproveUsers), null);
        }

        // Video
        [HttpGet]
        public async Task<IActionResult> ApproveVideos()
        {
            var videos = await videoService.GetPendingVideosAsync();
            return Ok(videos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveVideo(int id)
        {
            await videoService.ApproveVideoAsync(id);
            return CreatedAtAction(nameof(ApproveVideos), null);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RejectVideo(int id)
        {
            await videoService.DeleteVideoById(id);
            return CreatedAtAction(nameof(ApproveVideos), null);
        }
    }
}
