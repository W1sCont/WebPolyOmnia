using WPO.Domain.Enum;

namespace WPO.Domain
{
    public class Video
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string UrlOrPath { get; set; } = string.Empty;
        public VideoSourceType SourceType { get; set; }
        public string? ThumbnailUrlOrPath { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }
        public string Duration { get; set; } = string.Empty;
        public bool IsApproved { get; set; } = false;
        public bool IsPublished { get; set; } = false;
        public bool AllowDownload { get; set; } = false;
        public int ViewCount { get; set; } = 0;
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
