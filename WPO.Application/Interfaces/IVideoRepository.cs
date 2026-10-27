using System;
using System.Collections.Generic;
using System.Text;
using WPO.Domain;

namespace WPO.Application.Interfaces
{
    public interface IVideoRepository : IRepository<Video>
    {
        Task<IEnumerable<Video>> GetApprovedVideosAsync();
        Task<IEnumerable<Video>> GetAllWithDetailsAsync();
        Task<Video?> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Video>> GetVideoListByUserIdAsync(int id);
        Task<IEnumerable<Video>> GetPendingVideosAsync();
        Task ApproveVideoAsync(int videoId);
    }
}
