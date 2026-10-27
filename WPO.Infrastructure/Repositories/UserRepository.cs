using Microsoft.EntityFrameworkCore;
using WPO.Application.Interfaces;
using WPO.Domain;
using WPO.Domain.Enum;
using WPO.Infrastructure.EF;

namespace WPO.Infrastructure.Repositories
{
    public class UserRepository : EFRepository<User>, IUserRepository
    {
        public UserRepository(PolyOmniaContext context) : base(context) { }

        public async Task CreateUserAsync(User model)
        {
            await _context.Users.AddAsync(model);
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<User?> GetByLoginAsync(string login)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Login == login);
        }
        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _context.Users.AsNoTracking().AnyAsync(u => u.Email == email);
        }
        public async Task<bool> IsLoginUniqueAsync(string login)
        {
            return !await _context.Users.AsNoTracking().AnyAsync(u => u.Login == login);
        }

        public async Task<User?> GetByLoginOrEmailAsync(string emailOrLogin)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == emailOrLogin || u.Email == emailOrLogin);
        }
        public async Task UpdateRoleAsync(int userId, UserRole newRole)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.Role = newRole;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> IsUserUnconfirmedAsync(int id)
        {
            User? user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            if (user?.Role == UserRole.Unconfirmed)
            {
                return true;
            }
            return false;
        }
        public async Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role)
        {
            return await _context.Users.AsNoTracking().Where(u => u.Role == role).ToListAsync();
        }
        public Task<User?> GetUserWithVideosAsync(int userId)
        {
            return _context.Users.AsNoTracking().Include(u => u.Videos).FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
