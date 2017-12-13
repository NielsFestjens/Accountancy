using Accountancy.Controllers.Documents;
using Microsoft.Extensions.DependencyInjection;

namespace Accountancy.Startup.Installers
{
    public class DocumentIntaller
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IInvoicePdfCreator, InvoicePdfCreator>();
        }
    }
}