using Accountancy.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Accountancy.Startup.Installers
{
    public class DatabaseInstaller
    {
        public static void ConfigureServices(IServiceCollection services, IConfigurationSection connectionStrings)
        {
            services.AddEntityFrameworkSqlServer();
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connectionStrings["DefaultConnection"]));
            services.AddTransient<IRepository, Repository>();
        }
    }
}