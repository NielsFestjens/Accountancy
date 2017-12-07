using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Accountancy.Startup.Installers
{
    public class LoggingInstaller
    {
        public static void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IConfigurationSection loggingConfig)
        {
            loggerFactory.AddConsole(loggingConfig);
            loggerFactory.AddDebug();
        }
    }
}