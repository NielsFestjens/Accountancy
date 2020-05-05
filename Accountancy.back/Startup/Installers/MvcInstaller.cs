using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Accountancy.Startup.Installers
{
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
            app.UseCors(builder => builder.WithOrigins("http://localhost:8080").AllowAnyHeader().SetIsOriginAllowedToAllowWildcardSubdomains().AllowCredentials());
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}