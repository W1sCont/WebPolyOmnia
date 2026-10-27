using AutoMapper;
using WPO.Application.DTO;
using WPO.Application.Interfaces;
using WPO.Application.Exceptions;
using WPO.Domain;
using WPO.Domain.Enum;

namespace WPO.Infrastructure.Services
{
    public class UserService(IUnitOfWork db, IMapper mapper, IPasswordHasher passwordHasher) : IUserService
    {
        public async Task CreateUser(UserDTO model)
        {
            var user = mapper.Map<UserDTO, User>(model);
            var (passwordHash, passwordSalt) = passwordHasher.HashPassword(model.PasswordHash);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Role = UserRole.Unconfirmed;

            await db.User.AddAsync(user);
            await db.Save();
        }

        public async Task UpdateUser(UserDTO model)
        {
            var user = mapper.Map<UserDTO, User>(model);
            await db.User.UpdateAsync(user);
            await db.Save();
        }

        public async Task DeleteUser(int userId)
        {
            var user = await db.User.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ValidationException("User with id not found", nameof(userId));
            }
            await db.User.DeleteAsync(user);
            await db.Save();
        }

        public async Task<UserDTO?> GetUserById(int userId)
        {
            var user = await db.User.GetByIdAsync(userId);
            if (user == null)
                throw new ValidationException("User with id not found", nameof(userId));

            return mapper.Map<User, UserDTO>(user);
        }

        public async Task<UserDTO?> GetUserByEmail(string email)
        {
            var user = await db.User.GetByEmailAsync(email);
            if (user == null)
                throw new ValidationException("User with email not found", nameof(email));

            return mapper.Map<User, UserDTO>(user);
        }

        public async Task<UserDTO?> GetUserByLogin(string login)
        {
            var user = await db.User.GetByLoginAsync(login);
            if (user == null)
                throw new ValidationException($"User with login not found", nameof(login));

            return mapper.Map<User, UserDTO>(user);
        }

        public async Task<UserDTO?> GetUserByLoginOrEmail(string emailOrLogin)
        {
            var user = await db.User.GetByLoginOrEmailAsync(emailOrLogin);
            if (user == null)
                return null;

            return mapper.Map<User, UserDTO>(user);
        }

        public async Task<bool> IsEmailUnique(string email)
        {
            var user = await db.User.GetByEmailAsync(email);
            return user == null;
        }

        public async Task<bool> IsLoginUnique(string login)
        {
            var user = await db.User.GetByLoginAsync(login);
            return user == null;
        }

        public async Task UpdateUserRole(int userId, string newRole)
        {
            var user = await db.User.GetByIdAsync(userId);
            if (user == null)
                throw new ValidationException($"User with id not found", nameof(userId));

            if (!Enum.TryParse<UserRole>(newRole, true, out var parsedRole))
                throw new ValidationException("Invalid role:", newRole);

            user.Role = parsedRole;
            await db.User.UpdateAsync(user);
            await db.Save();
        }

        public async Task<bool> IsUserUnconfirmed(int id)
        {
            return await db.User.IsUserUnconfirmedAsync(id);
        }

        public async Task<IEnumerable<UserDTO>> GetUsersByRole(string role)
        {
            if (!Enum.TryParse<UserRole>(role, true, out var parsedRole))
                throw new ValidationException("Invalid role:", role);

            var users = await db.User.GetUsersByRoleAsync(parsedRole);
            return mapper.Map<IEnumerable<User>, IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO?> GetUserWithVideos(int userId)
        {
            var user = await db.User.GetUserWithVideosAsync(userId);
            if (user == null)
                throw new ValidationException($"User with id not found", nameof(userId));

            return mapper.Map<User, UserDTO>(user);
        }

        public async Task<UserDTO?> Authenticate(string EmailOrLogin, string Password)
        {
            UserDTO? user = await GetUserByLoginOrEmail(EmailOrLogin);
            if (user == null)
                return null;

            if (passwordHasher.VerifyPassword(Password, user.PasswordHash, user.PasswordSalt))
                return user;
            else return null;
        }

        public async Task<IEnumerable<VideoDTO>> GetVideoList(int userId)
        {
            IEnumerable<Video?> videoList = await db.Video.GetVideoListByUserIdAsync(userId);
            if (!videoList.Any())
                return null;
            return mapper.Map<IEnumerable<Video>, IEnumerable<VideoDTO>>(videoList);
        }
    }
}
