using System.ComponentModel.DataAnnotations;
using WPO.Domain.Enum;

namespace WPO.Application.DTO
{
    public class CreateVideoDTO
    {
        [Required(ErrorMessage = "Введіть назву відео")]
        [StringLength(100, ErrorMessage = "Назва не повинна перевищувати 100 символів")]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000, ErrorMessage = "Опис не повинен перевищувати 2000 символів")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Оберіть жанр")]
        public int GenreId { get; set; }

        public VideoSourceType SourceType { get; set; } = VideoSourceType.LocalFile;

        // Поле для файлу (якщо обрано LocalFile)
        public string VideoFilePath { get; set; } = string.Empty;

        // Поле для зовнішнього URL (якщо обрано ExternalUrl)
        [Url(ErrorMessage = "Введіть коректну URL-адресу")]
        public string? ExternalUrl { get; set; }

        public string ThumbnailFile { get; set; } = string.Empty;

        public bool AllowDownload { get; set; } = false;
    }
}
