using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

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
