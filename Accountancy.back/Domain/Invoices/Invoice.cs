using Accountancy.Infrastructure.Database;

namespace Accountancy.Domain.Invoices;

public record Invoice : IEntity<int>
{
    public int Id { get; set; }
    public int Number { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public DateTime Date { get; set; }
    public int ExpiryPeriodDays { get; set; }
    public InvoiceStatus Status { get; set; }
    public string TheirReference { get; set; }

    public Company IssuingCompany { get; set; }
    public Company ReceivingCompany { get; set; }

    public List<InvoiceLine> InvoiceLines { get; set; }

    public string FullNumber => $"{Date.Year}{Number:00000}";
    public DateTime ExpiryDate => Date.AddDays(ExpiryPeriodDays);
    public decimal TotalExclVat => InvoiceLines?.Sum(x => x.TotalExclVat) ?? 0;
    public decimal TotalExclVatForVat21 => InvoiceLines?.Where(x => x.VatType == VatType.Vat21).Sum(x => x.TotalExclVat) ?? 0;
    public decimal Vat21 => InvoiceLines?.Where(x => x.VatType == VatType.Vat21).Sum(x => x.TotalExclVat * 21 / 100) ?? 0;
    public decimal Total => TotalExclVat + Vat21;
}