using Accountancy.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Accountancy.Startup.Installers
{
    public class DatabaseInstaller
    {
        public static void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            var config = configuration.GetSection("Database");
            services.AddEntityFrameworkInMemoryDatabase().AddDbContext<DatabaseContext>(options => options.UseSqlServer(config["Connectionstring"]));
            services.AddTransient<IRepository, Repository>();
        }
    }
}