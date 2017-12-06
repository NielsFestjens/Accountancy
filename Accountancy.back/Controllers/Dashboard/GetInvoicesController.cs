using System.Collections.Generic;
using System.Threading.Tasks;
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
            return new List<InvoiceDto>
            {
                new InvoiceDto
                {
                    Id = 1,
                    Name = "2017/10 - Qframe",
                    Status = InvoiceStatus.Paid
                },
                new InvoiceDto
                {
                    Id = 2,
                    Name = "2017/10 - Cronos",
                    Status = InvoiceStatus.Paid
                },
                new InvoiceDto
                {
                    Id = 3,
                    Name = "2017/11 - Qframe",
                    Status = InvoiceStatus.Sent
                },
                new InvoiceDto
                {
                    Id = 4,
                    Name = "2017/11 - Cronos",
                    Status = InvoiceStatus.Sent
                },
                new InvoiceDto
                {
                    Id = 5,
                    Name = "2017/11 - Qframe",
                    Status = InvoiceStatus.Draft
                },
                new InvoiceDto
                {
                    Id = 6,
                    Name = "2017/11 - Cronos",
                    Status = InvoiceStatus.Draft
                }
            };
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