using Microsoft.EntityFrameworkCore;
using WPO.Application.Interfaces;
using WPO.Infrastructure.EF;
using EFCore = Microsoft.EntityFrameworkCore.EF;

namespace WPO.Infrastructure.Repositories
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        protected readonly PolyOmniaContext _context;

        public EFRepository(PolyOmniaContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync() =>
            await _context.Set<T>().AsNoTracking().ToListAsync();

        public async Task<T?> GetByIdAsync(int id) =>
            await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => EFCore.Property<int>(e, "Id") == id);
        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}
