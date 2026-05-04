using Accountancy.Domain.Documents;

namespace Accountancy.Startup.Installers;

public class DocumentInstaller
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IInvoicePdfCreator, InvoicePdfCreator>();
    }
}