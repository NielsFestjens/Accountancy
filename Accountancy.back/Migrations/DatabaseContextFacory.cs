using Accountancy.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Accountancy.Migrations;

public class DatabaseContextFacory
{
    public static DatabaseContext Create(string connectionStringName = "DefaultConnection")
    {
        var environmentName = Environment.GetEnvironmentVariable("Hosting:Environment");
        var basePath = AppContext.BaseDirectory;

        var builder = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environmentName}.json", true)
            .AddEnvironmentVariables();

        var config = builder.Build();
        return new DatabaseContext(new DbContextOptionsBuilder<DatabaseContext>().UseSqlServer(config.GetConnectionString(connectionStringName)).Options);
    }
}