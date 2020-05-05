using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accountancy.Domain.Invoices;
using Accountancy.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Accountancy.Controllers.Dashboard
{
    [Route("api/Dashboard/[controller]")]
    public class GetInvoicesController : Controller
    {
        private readonly IRepository _repository;

        public GetInvoicesController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<object>> Get()
        {
            return await _repository
                .Query<Invoice>()
                .Include(x => x.ReceivingCompany)
                .Include(x => x.InvoiceLines)
                .Where(x => x.Date.Year == DateTime.Now.Year)
                .Select(x => new
                {
                    x.Id,
                    x.Year,
                    x.Month,
                    ReceivingCompany = x.ReceivingCompany.Name,
                    x.Status,
                    x.Total
                })
                .ToListAsync();
        }
    }
}