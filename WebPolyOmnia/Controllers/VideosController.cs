using Microsoft.AspNetCore.Mvc;
using WPO.Application.Interfaces;
using WPO.Infrastructure.Services;

namespace WebPolyOmnia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController(IVideoService videoService) : Controller
    {
        // Video
        [HttpGet]
        public async Task<ActionResult> ApproveVideos()
        {
            var videos = await videoService.GetPendingVideosAsync();
            return Ok(videos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetVideo(int id)
        {
            var video = await videoService.GetByIdWithDetails(id);
            return Ok(video);
        }

        [HttpPost]
        public async Task<ActionResult> ApproveVideo(int id)
        {
            await videoService.ApproveVideoAsync(id);
            return CreatedAtAction(nameof(ApproveVideos), null);
        }

        [HttpDelete("videos/{id}")]
        public async Task<ActionResult> RejectVideo(int id)
        {
            await videoService.DeleteVideoById(id);
            return CreatedAtAction(nameof(ApproveVideos), null);
        }
    }
}
