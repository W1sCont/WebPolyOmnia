using System;
using System.Collections.Generic;
using System.Text;
using WPO.Domain;
using WPO.Domain.Enum;

namespace WPO.Application.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task CreateUserAsync(User model);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByLoginAsync(string login);
        Task<bool> IsEmailUniqueAsync(string email);
        Task<bool> IsLoginUniqueAsync(string login);
        Task<User?> GetByLoginOrEmailAsync(string emailOrLogin);
        Task UpdateRoleAsync(int userId, UserRole newRole);
        Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role);
        Task<bool> IsUserUnconfirmedAsync(int id);
        Task<User?> GetUserWithVideosAsync(int userId);
    }
}
