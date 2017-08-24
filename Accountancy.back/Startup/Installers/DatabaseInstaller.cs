using Accountancy.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Accountancy.Startup.Installers
{
    public class DatabaseInstaller
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkInMemoryDatabase().AddDbContext<DatabaseContext>(options => options.UseInMemoryDatabase("LeDatabase"));
            services.AddTransient<IRepository, Repository>();
        }
    }
}