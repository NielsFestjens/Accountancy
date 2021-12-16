namespace Accountancy.Domain.Invoices;

public record Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string FullName { get; set; }
    public string VAT { get; set; }
    public string BankAccount { get; set; }
    public Person ContactPerson { get; set; }
    public string Website { get; set; }
    public string Recipients { get; set; }

    public virtual List<CompanyAddress> Addresses { get; set; } = new List<CompanyAddress>();

    public CompanyAddress GetActiveAddress(DateTime onDate) => Addresses.Single(x => x.Start <= onDate && x.End > onDate);
}

public record CompanyAddress
{
    public int Id { get; set; }
    public Company Company { get; set; }
    public string AddressLine { get; set; }
    public string CityLine { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}