using AutoMapper;
using WPO.Application.DTO;
using WPO.Application.Interfaces;
using WPO.Application.Exceptions;
using WPO.Domain;

namespace WPO.Infrastructure.Services
{
    public class VideoService(IUnitOfWork db, IMapper mapper) : IVideoService
    {
        public async Task CreateVideo(VideoDTO videoDTO)
        {
            var video = mapper.Map<VideoDTO, Video>(videoDTO);
            await db.Video.AddAsync(video);
            await db.Save();
        }

        public async Task UpdateVideo(VideoDTO videoDTO)
        {
            var existingVideo = await db.Video.GetByIdWithDetailsAsync(videoDTO.Id);
            if (existingVideo == null)
                throw new ValidationException("Video with id not found", nameof(videoDTO.Id));

            mapper.Map(videoDTO, existingVideo);
            await db.Save();
        }

        public async Task DeleteVideo(VideoDTO videoDTO)
        {
            var video = mapper.Map<VideoDTO, Video>(videoDTO);
            if (video == null)
            {
                throw new ValidationException("Video with id not found", nameof(videoDTO.Id));
            }
            await db.Video.DeleteAsync(video);
            await db.Save();
        }

        public async Task<IEnumerable<VideoDTO>> GetApprovedVideos()
        {
            var videos = await db.Video.GetApprovedVideosAsync();
            return mapper.Map<IEnumerable<Video>, IEnumerable<VideoDTO>>(videos);
        }

        public async Task<IEnumerable<VideoDTO>> GetAllWithDetails()
        {
            var videos = await db.Video.GetAllWithDetailsAsync();
            return mapper.Map<IEnumerable<Video>, IEnumerable<VideoDTO>>(videos);
        }

        public async Task<VideoDTO> GetByIdWithDetails(int id)
        {
            var video = await db.Video.GetByIdWithDetailsAsync(id);
            if (video == null)
            {
                throw new ValidationException("Video with id not found", nameof(video.Id));
            }
            return mapper.Map<Video, VideoDTO>(video);
        }

        public async Task<IEnumerable<VideoDTO>> GetApprovedVideosAsync()
        {
            var video = await db.Video.GetApprovedVideosAsync();
            return mapper.Map<IEnumerable<Video>, IEnumerable<VideoDTO>>(video);
        }

        public async Task<IEnumerable<VideoDTO>> GetPendingVideosAsync()
        {
            var video = await db.Video.GetPendingVideosAsync();
            return mapper.Map<IEnumerable<Video>, IEnumerable<VideoDTO>>(video);
        }

        public async Task ApproveVideoAsync(int videoId)
        {
            await db.Video.ApproveVideoAsync(videoId);
        }

        public async Task DeleteVideoById(int id)
        {
            var video = await db.Video.GetByIdAsync(id);
            if (video == null) throw new ValidationException("Video with id not found", nameof(video.Id));

            await db.Video.DeleteAsync(video);
            await db.Save();
        }
    }
}
