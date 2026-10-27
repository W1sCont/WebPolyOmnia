using System;
using System.Collections.Generic;
using System.Text;
using WPO.Application.DTO;

namespace WPO.Application.Interfaces
{
    public interface IVideoService
    {
        Task CreateVideo(VideoDTO videoDTO);
        Task UpdateVideo(VideoDTO videoDTO);
        Task DeleteVideo(VideoDTO videoDTO);
        Task DeleteVideoById(int id);
        Task<IEnumerable<VideoDTO>> GetApprovedVideos();
        Task<IEnumerable<VideoDTO>> GetAllWithDetails();
        Task<VideoDTO> GetByIdWithDetails(int id);
        Task<IEnumerable<VideoDTO>> GetPendingVideosAsync();
        Task ApproveVideoAsync(int id);
    }
}
