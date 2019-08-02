namespace Accountancy.Domain.Invoices
{
    public class InvoiceLine
    {
        public InvoiceLine() {}

        public InvoiceLine(string description, decimal amount, decimal price, VatType vatType = VatType.Vat21)
        {
            Description = description;
            Amount = amount;
            Price = price;
            VatType = vatType;
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public decimal TotalExclVat => Amount * Price;
        public VatType VatType { get; set; }
    }
}