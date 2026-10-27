using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using WPO.Domain;

namespace WPO.Infrastructure.EF
{
    public class PolyOmniaContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Video> Videos { get; set; }

        public PolyOmniaContext(DbContextOptions<PolyOmniaContext> options) : base(options)
        {
        }
    }

    // Клас необхідний виключно для міграцій
    public class SampleContextFactory : IDesignTimeDbContextFactory<PolyOmniaContext>
    {
        public PolyOmniaContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PolyOmniaContext>();

            // Отримуємо конфігурацію із файлу appsettings.json
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            IConfigurationRoot config = builder.Build();

            // Отримуємо рядок підключення з файлу appsettings.json
            string? connectionString = config.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
            return new PolyOmniaContext(optionsBuilder.Options);
        }
    }
}
