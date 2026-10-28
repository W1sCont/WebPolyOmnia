using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WPO.Infrastructure.EF;

namespace WPO.Infrastructure.Extensions
{
    public static class PolyOmniaContextExtensions
    {
        public static void AddPolyOmniaContext(this IServiceCollection services, string connection) =>
        services.AddDbContext<PolyOmniaContext>(options => options.UseSqlServer(connection));
    }
}
