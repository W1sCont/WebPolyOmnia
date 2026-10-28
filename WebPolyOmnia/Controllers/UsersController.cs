using Microsoft.AspNetCore.Mvc;
using WPO.Application.DTO;
using WPO.Application.Interfaces;
using WPO.Infrastructure.Services;

namespace WebPolyOmnia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : Controller
    {
        // User
        [HttpGet]
        public async Task<ActionResult> ApproveUsers()
        {
            var users = await userService.GetUsersByRole("Unconfirmed");
            return Ok(users);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ApproveUser(int id)
        {
            if (id == 0) return BadRequest(new { message = "Error ID" });

            await userService.UpdateUserRole(id, "User");
            return CreatedAtAction(nameof(ApproveUsers), null);
        }

        [HttpDelete("users/{id}")]
        public async Task<ActionResult> RejectUser(int id)
        {
            await userService.DeleteUser(id);
            return CreatedAtAction(nameof(ApproveUsers), null);
        }
    }
}
