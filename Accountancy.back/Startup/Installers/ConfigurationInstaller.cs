using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Accountancy.Startup.Installers
{
    public class ConfigurationInstaller
    {
        public static IConfigurationRoot BuildConfiguration(IHostingEnvironment env)
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