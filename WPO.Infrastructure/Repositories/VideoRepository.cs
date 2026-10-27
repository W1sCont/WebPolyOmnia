using Microsoft.EntityFrameworkCore;
using WPO.Application.Interfaces;
using WPO.Domain;
using WPO.Infrastructure.EF;

namespace WPO.Infrastructure.Repositories
{
    public class VideoRepository : EFRepository<Video>, IVideoRepository
    {
        public VideoRepository(PolyOmniaContext context) : base(context) { }

        public async Task<IEnumerable<Video>> GetApprovedVideosAsync()
        {
            return await _context.Videos.AsNoTracking()
                .Include(v => v.Category)
                .Include(v => v.User)
                .Where(v => v.IsApproved && v.IsPublished)
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Video>> GetAllWithDetailsAsync()
        {
            return await _context.Videos.AsNoTracking()
                .Include(v => v.Category)
                .Include(v => v.User)
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync();
        }

        public async Task<Video?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Videos
                .Include(v => v.User)
                .Include(v => v.Category)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Video>> GetVideoListByUserIdAsync(int id)
        {
            return await _context.Videos.AsNoTracking()
                .Include(v => v.User)
                .Include(v => v.Category)
                .Where(v => v.UserId == id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Video>> GetPendingVideosAsync()
        {
            return await _context.Videos.AsNoTracking()
                .Include(v => v.Category)
                .Include(v => v.User)
                .Where(v => !v.IsApproved)
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync();
        }

        public async Task ApproveVideoAsync(int videoId)
        {
            var video = await _context.Videos.FindAsync(videoId);
            if (video != null)
            {
                video.IsApproved = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
