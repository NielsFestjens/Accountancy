using Accountancy.Domain.Documents;
using Accountancy.Domain.Invoices;
using Accountancy.Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Accountancy.Controllers.Invoices;

[Route("api/Invoices/[controller]")]
[AllowAnonymous]
public class PrintPdfController : Controller
{
    private readonly IRepository _repository;
    private readonly IInvoicePdfCreator _invoicePdfCreator;

    public PrintPdfController(IRepository repository, IInvoicePdfCreator invoicePdfCreator)
    {
        _repository = repository;
        _invoicePdfCreator = invoicePdfCreator;
    }

    [HttpGet]
    public ActionResult Get(int id)
    {
        var invoice = _repository
            .Query<Invoice>()
            .Include(x => x.IssuingCompany)
            .Include(x => x.IssuingCompany.ContactPerson)
            .Include(x => x.IssuingCompany.Addresses)
            .Include(x => x.ReceivingCompany)
            .Include(x => x.ReceivingCompany.ContactPerson)
            .Include(x => x.ReceivingCompany.Addresses)
            .Include(x => x.InvoiceLines)
            .Single(x => x.Id == id);

        var stream = _invoicePdfCreator.Generate(invoice);
            
        Response.Headers.Add("Content-Disposition", $"inline; filename={invoice.Year}-{invoice.Month:00} - Factuur van {invoice.IssuingCompany.FullName} voor {invoice.ReceivingCompany.FullName}.pdf");
        return new FileContentResult(stream.ToArray(), "application/pdf");
    }
}