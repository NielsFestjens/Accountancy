using Accountancy.Startup.Installers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Accountancy.Startup
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostEnvironment env)
        {
            Configuration = ConfigurationInstaller.BuildConfiguration(env);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            MvcInstaller.ConfigureServices(services);
            DatabaseInstaller.ConfigureServices(services, Configuration.GetSection("ConnectionStrings"));
            DocumentIntaller.ConfigureServices(services);
            SecurityInstaller.ConfigureServices(services);
        }

        public void Configure(IApplicationBuilder app)
        {
            ExceptionInstaller.Configure(app);
            SecurityInstaller.Configure(app);
            MvcInstaller.Configure(app);
        }
    }
}
