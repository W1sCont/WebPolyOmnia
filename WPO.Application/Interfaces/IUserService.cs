using System;
using System.Collections.Generic;
using System.Text;
using WPO.Application.DTO;

namespace WPO.Application.Interfaces
{
    public interface IUserService
    {
        Task CreateUser(UserDTO model);
        Task UpdateUser(UserDTO model);
        Task DeleteUser(int userId);
        Task<UserDTO?> GetUserById(int userId);
        Task<UserDTO?> GetUserByEmail(string email);
        Task<UserDTO?> GetUserByLogin(string login);
        Task<UserDTO?> GetUserByLoginOrEmail(string emailOrLogin);
        Task<bool> IsEmailUnique(string email);
        Task<bool> IsLoginUnique(string login);
        Task UpdateUserRole(int userId, string newRole);
        Task<bool> IsUserUnconfirmed(int id);
        Task<IEnumerable<UserDTO>> GetUsersByRole(string role);
        Task<UserDTO?> GetUserWithVideos(int userId);
        Task<UserDTO?> Authenticate(string EmailOrLogin, string Password);
        Task<IEnumerable<VideoDTO>> GetVideoList(int userId);
    }
}
