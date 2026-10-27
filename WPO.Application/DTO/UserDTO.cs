using System;
using System.Collections.Generic;
using System.Text;
using WPO.Domain.Enum;

namespace WPO.Application.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public UserRole Role { get; set; } = UserRole.User;
        public bool IsPublished { get; set; } = false;
        public bool IsApproved { get; set; } = false;
    }
}
