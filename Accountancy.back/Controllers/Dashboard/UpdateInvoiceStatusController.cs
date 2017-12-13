using Accountancy.Domain.Invoices;
using Accountancy.Infrastructure.Database;
using Accountancy.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;

namespace Accountancy.Controllers.Dashboard
{
    public class UpdateInvoiceStatusCommand
    {
        public int Id { get; set; }
        public InvoiceStatus Status { get; set; }
    }

    [Route("api/Dashboard/[controller]")]
    public class UpdateInvoiceStatusController : Controller
    {
        private readonly IRepository _repository;
        private readonly ISecurityService _securityService;

        public UpdateInvoiceStatusController(IRepository repository, ISecurityService securityService)
        {
            _repository = repository;
            _securityService = securityService;
        }

        [HttpPost]
        public void Post([FromBody] UpdateInvoiceStatusCommand command)
        {
            var invoice = _repository.Get<Invoice>(command.Id);
            invoice.Status = command.Status;
            _repository.Save();
        }
    }
}