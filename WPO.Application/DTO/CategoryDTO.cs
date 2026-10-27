using System;
using System.Collections.Generic;
using System.Text;

namespace WPO.Application.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<VideoDTO> Videos { get; set; } = new List<VideoDTO>();
    }
}
