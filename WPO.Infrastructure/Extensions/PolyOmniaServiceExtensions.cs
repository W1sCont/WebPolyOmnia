using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WPO.Application.Interfaces;
using WPO.Infrastructure.Services;

namespace WPO.Infrastructure.Extensions
{
    public static class PolyOmniaServiceExtensions
    {
        public static void AddPolyOmniaServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
        }
    }
}
