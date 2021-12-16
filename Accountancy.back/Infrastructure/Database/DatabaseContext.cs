using Accountancy.Domain.Auth;
using Accountancy.Domain.Invoices;
using Microsoft.EntityFrameworkCore;

namespace Accountancy.Infrastructure.Database;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<User>().HasKey(x => x.Id);
        builder.Entity<Invoice>().HasKey(x => x.Id);
        builder.Entity<InvoiceLine>().HasKey(x => x.Id);
        builder.Entity<Person>().HasKey(x => x.Id);
        builder.Entity<Company>().HasKey(x => x.Id);
    }
}