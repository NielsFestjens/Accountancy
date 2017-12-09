namespace Accountancy.Domain.Invoices
{
    public class InvoiceLine
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public decimal TotalExclVat => Amount * Price;
        public VatType VatType { get; set; }
    }
}