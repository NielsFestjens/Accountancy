using Accountancy.Domain.Invoices;
using Accountancy.Infrastructure.Database;
using Accountancy.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Accountancy.Controllers.Invoices;

[Route("api/Invoices/[controller]")]
public class GetInvoiceController : Controller
{
    private readonly IRepository _repository;
    private readonly ISecurityService _securityService;

    public GetInvoiceController(IRepository repository, ISecurityService securityService)
    {
        _repository = repository;
        _securityService = securityService;
    }

    [HttpGet]
    public object Get(int id)
    {
            
        var invoice = _repository
            .Query<Invoice>()
            .Include(x => x.InvoiceLines)
            .Single(x => x.Id == id);

        return new
        {
            invoice.Id,
            invoice.Year,
            invoice.Month,
            invoice.Date,
            invoice.ExpiryPeriodDays,
            invoice.Status,
            InvoiceLines = invoice.InvoiceLines.Select(x => new
            {
                x.Id,
                x.Description,
                x.Amount,
                x.Price,
                x.VatType
            })
        };
    }
}