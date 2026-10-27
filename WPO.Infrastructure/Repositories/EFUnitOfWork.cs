using System;
using System.Collections.Generic;
using System.Text;
using WPO.Application.Interfaces;
using WPO.Infrastructure.EF;

namespace WPO.Infrastructure.Repositories
{
    public class EFUnitOfWork(PolyOmniaContext context) : IUnitOfWork
    {
        private UserRepository? _userRepository;
        private VideoRepository? _videoRepository;
        private CategoryRepository? _categoryRepository;
        public IUserRepository User => _userRepository ??= new UserRepository(context);
        public IVideoRepository Video => _videoRepository ??= new VideoRepository(context);
        public ICategoryRepository Category => _categoryRepository ??= new CategoryRepository(context);
        public async Task Save() => await context.SaveChangesAsync();
    }
}
