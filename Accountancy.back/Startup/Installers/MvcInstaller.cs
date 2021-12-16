namespace Accountancy.Startup.Installers;

public class MvcInstaller
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddCors();
        services.AddControllers();
    }

    public static void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseCors(builder => builder.WithOrigins("http://127.0.0.1:8080", "http://localhost:3000").AllowAnyHeader().SetIsOriginAllowedToAllowWildcardSubdomains().AllowCredentials());
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}