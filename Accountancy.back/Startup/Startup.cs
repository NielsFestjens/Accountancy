using Accountancy.Startup.Installers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Accountancy.Startup
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            Configuration = ConfigurationInstaller.BuildConfiguration(env);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            MvcInstaller.ConfigureServices(services);
            DatabaseInstaller.ConfigureServices(services);
            SecurityInstaller.ConfigureServices(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            LoggingInstaller.Configure(app, env, loggerFactory, Configuration);
            ExceptionInstaller.Configure(app, env, loggerFactory);
            SecurityInstaller.Configure(app, env, loggerFactory);
            MvcInstaller.Configure(app, env, loggerFactory);
        }
    }
}
