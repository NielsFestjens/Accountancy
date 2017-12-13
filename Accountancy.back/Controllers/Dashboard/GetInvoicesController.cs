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
        public async Task<IEnumerable<object>> Get()
        {
            return _repository.Query<Invoice>().Select(x => new
            {
                x.Id,
                Name = $"{x.Year}/{x.Month} - {x.ReceivingCompany.Name}",
                x.Status
            });
        }
    }
}