using Microsoft.Extensions.DependencyInjection;
using WPO.Application.Profiles;

namespace WPO.Infrastructure.Extensions
{
    public static class AutoMapperServiceExtensions
    {
        public static void AddPolyOmniaAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<PolyOmniaProfile>();
            });
        }
    }
}
