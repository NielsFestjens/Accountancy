using Accountancy.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Builder;

namespace Accountancy.Startup.Installers
{
    public class ExceptionInstaller
    {
        public static void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
        }
    }
}