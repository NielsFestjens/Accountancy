using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accountancy.Domain.Invoices;
using Accountancy.Infrastructure.Database;
using Accountancy.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;

namespace Accountancy.Controllers.Dashboard
{
    [Route("api/Dashboard/[controller]")]
    public class GetInvoicesController : Controller
    {
        private readonly IRepository _repository;
        private readonly ISecurityService _securityService;

        public GetInvoicesController(IRepository repository, ISecurityService securityService)
        {
            _repository = repository;
            _securityService = securityService;
        }

        [HttpGet]
        public async Task<IEnumerable<InvoiceDto>> Get()
        {
            return _repository.Query<Invoice>().Select(x => new InvoiceDto
            {
                Id = x.Id,
                Name = $"{x.Year}/{x.Month} - {x.ReceivingCompany.Name}",
                Status = x.Status
            });
        }
    }

    public class InvoiceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public InvoiceStatus Status { get; set; }
    }

    public enum InvoiceStatus
    {
        Draft,
        Sent,
        Paid,
    }
}