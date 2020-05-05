using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Accountancy.Startup.Installers
{
    public class ConfigurationInstaller
    {
        public static IConfigurationRoot BuildConfiguration(IHostEnvironment env)
        {
            return new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}