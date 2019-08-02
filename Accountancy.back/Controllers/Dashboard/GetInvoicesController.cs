using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accountancy.Domain.Invoices;
using Accountancy.Infrastructure.Database;
using Accountancy.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var t = _repository.Query<Invoice>().Include(x => x.ReceivingCompany).Include(x => x.InvoiceLines).Where(x => x.Date.Year == DateTime.Now.Year).ToList();
            return t.Select(x => new
            {
                x.Id,
                x.Year,
                x.Month,
                ReceivingCompany = x.ReceivingCompany.Name,
                x.Status,
                x.Total
            });
        }
    }
}