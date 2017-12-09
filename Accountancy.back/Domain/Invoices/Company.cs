namespace Accountancy.Domain.Invoices
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string AddressLine { get; set; }
        public string CityLine { get; set; }
        public string VAT { get; set; }
        public string BankAccount { get; set; }
        public Person ContactPerson { get; set; }
        public string Website { get; set; }
        public string Recipients { get; set; }
    }
}