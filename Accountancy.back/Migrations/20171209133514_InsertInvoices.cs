using Accountancy.Domain.Invoices;
using Accountancy.Infrastructure.Database;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Accountancy.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20171209133514_InsertInvoices")]
    public class InsertInvoices : Migration
    {
        private class InvoiceInserter
        {
            private readonly DatabaseContext _context;
            private readonly Company _nfSoftware;
            private readonly Company _qframe;
            private readonly Company _cronos;

            private decimal _dagprijs;
            private int _jaar;
            private int _maand;
            private int _factuurNr;

            public string ReferenceCronos { get; set; }
            public bool SplitRoyalties { get; set; }

            public InvoiceInserter(DatabaseContext context, Company nfSoftware, Company qframe, Company cronos)
            {
                _context = context;
                _nfSoftware = nfSoftware;
                _qframe = qframe;
                _cronos = cronos;
            }

            public void StartJaar(decimal newDagprijs, int newJaar, int newMaand = 1)
            {
                _dagprijs = newDagprijs;
                _jaar = newJaar;
                _maand = newMaand;
                _factuurNr = 0;
            }

            public void SetDagprijs(decimal newDagprijs)
            {
                _dagprijs = newDagprijs;
            }

            private void CreateInvoice(Company receiver, InvoiceStatus status, decimal? amount, string theirReference)
            {
                var date = new DateTime(_jaar, _maand, DateTime.DaysInMonth(_jaar, _maand));
                _context.Add(new Invoice
                {
                    Number = ++_factuurNr,
                    IssuingCompany = _nfSoftware,
                    ReceivingCompany = receiver,
                    Year = _jaar,
                    Month = _maand,
                    Date = date,
                    ExpiryPeriodDays = 30,
                    Status = status,
                    TheirReference = theirReference,
                    InvoiceLines = GetInvoiceLines(amount).ToList()
                });
                _context.SaveChanges();
            }

            private IEnumerable<InvoiceLine> GetInvoiceLines(decimal? amount)
            {
                if (amount == null)
                    yield break;

                if (!SplitRoyalties)
                {
                    yield return new InvoiceLine("Gepresteerde dagen", amount.Value, _dagprijs);
                    yield break;
                }

                yield return new InvoiceLine("Gepresteerde dagen", amount.Value, _dagprijs * 3 / 4);
                yield return new InvoiceLine("Vergoeding overdracht auteursrechten", amount.Value, _dagprijs / 4);
            }

            public void CreateInvoices(decimal? amountQframe, decimal? amountCronos = null)
            {
                if (amountQframe.HasValue)
                    CreateInvoice(_qframe, InvoiceStatus.Sent, amountQframe, null);

                if (amountCronos.HasValue)
                    CreateInvoice(_cronos, InvoiceStatus.Sent, amountCronos, ReferenceCronos);

                _maand++;
            }
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            using (var context = DatabaseContextFacory.Create())
            {
                var nfSoftware = InsertNfSoftware(context);
                var qframe = InsertQframe(context);
                var cronos = InsertCronos(context);

                context.SaveChanges();

                InsertAllInvoices(context, nfSoftware, qframe, cronos);

                context.SaveChanges();
            }
        }

        private static Company InsertCronos(DatabaseContext context)
        {
            var cronos = new Company
            {
                Name = "Cronos",
                FullName = "Cronos NV",
                Addresses = new List<CompanyAddress>
                {
                    new CompanyAddress
                    {
                        AddressLine = "Veldkant 35D",
                        CityLine = "2550 Kontich",
                        Start = new DateTime(2007, 02, 14),
                        End = new DateTime(2100, 12, 31)
                    }
                },
                VAT = "BE 0443.807.959",
                Recipients = "daria.wycislo@cronos.be; heidi.lens@cronos.be; karina.vereecken@qframe.be"
            };
            context.Add(cronos);
            return cronos;
        }

        private static Company InsertQframe(DatabaseContext context)
        {
            var contactPerson = new Person
            {
                FirstName = "Danny",
                LastName = "Gladines",
                Email = "danny.gladines@qframe.be"
            };
            context.Add(contactPerson);

            var qframe = new Company
            {
                ContactPerson = contactPerson,
                Name = "Qframe",
                FullName = "Qframe NV",
                Addresses = new List<CompanyAddress>
                {
                    new CompanyAddress
                    {
                        AddressLine = "Veldkant 33A",
                        CityLine = "2550 Kontich",
                        Start = new DateTime(1995, 01, 1),
                        End = new DateTime(2100, 12, 31)
                    }
                },
                VAT = "BE 0887.377.180",
                Recipients = "karina.vereecken@qframe.be"
            };
            context.Add(qframe);

            return qframe;
        }

        private static Company InsertNfSoftware(DatabaseContext context)
        {
            var contactPerson = new Person
            {
                FirstName = "Niels",
                LastName = "Festjens",
                Email = "festjens_niels@hotmail.com",
                Phone = "+32 477 / 60 39 05"
            };
            context.Add(contactPerson);

            var nfSoftware = new Company
            {
                ContactPerson = contactPerson,
                Name = "N.F. Software",
                FullName = "N.F. Software Comm.V",
                Addresses = new List<CompanyAddress>
                {
                    new CompanyAddress
                    {
                        AddressLine = "Hollebeekstraat 5 bus 3",
                        CityLine = "2840 Rumst",
                        Start = new DateTime(2017, 10, 01),
                        End = new DateTime(2020, 05, 01)
                    },
                    new CompanyAddress
                    {
                        AddressLine = "Heist-Goorstraat 53A bus 5",
                        CityLine = "2220 Heist-op-den-Berg",
                        Start = new DateTime(2020, 05, 01),
                        End = new DateTime(2021, 06, 01)
                    },
                    new CompanyAddress
                    {
                        AddressLine = "Goorlei 26",
                        CityLine = "2220 Heist-op-den-Berg",
                        Start = new DateTime(2021, 06, 01),
                        End = new DateTime(2100, 12, 31)
                    }
                },
                VAT = "BE 0681.952.956",
                BankAccount = "BE75 7360 4179 6051"
            };
            context.Add(nfSoftware);

            return nfSoftware;
        }

        private static void InsertAllInvoices(DatabaseContext context, Company nfSoftware, Company qframe, Company cronos)
        {
            var invoiceInserter = new InvoiceInserter(context, nfSoftware, qframe, cronos);

            invoiceInserter.StartJaar(520.00m, 2017, 10);
            invoiceInserter.CreateInvoices(2.78m, 17.94m); 
            invoiceInserter.CreateInvoices(1.53m, 20.28m);
            invoiceInserter.CreateInvoices(1.13m, 22.44m);

            invoiceInserter.StartJaar(520.00m, 2018);
            invoiceInserter.CreateInvoices(1.51m, 25.20m);
            invoiceInserter.ReferenceCronos = "CRO18/0257/0001";
            invoiceInserter.CreateInvoices(0.25m, 24.38m);
            invoiceInserter.CreateInvoices(0.38m, 28.13m);
            invoiceInserter.CreateInvoices(null , 26.06m);
            invoiceInserter.CreateInvoices(0.94m, 19.44m);
            invoiceInserter.CreateInvoices(1.50m, 19.50m);
            invoiceInserter.CreateInvoices(1.13m, 17.69m);
            invoiceInserter.CreateInvoices(0.75m, 11.94m);
            invoiceInserter.CreateInvoices(2.38m, 18.56m);
            invoiceInserter.CreateInvoices(2.58m, 23.09m);
            invoiceInserter.CreateInvoices(1.50m, 19.75m);
            invoiceInserter.CreateInvoices(0.56m, 19.44m);

            invoiceInserter.StartJaar(540.00m, 2019);
            invoiceInserter.CreateInvoices(22.38m);
            invoiceInserter.CreateInvoices(18.00m);
            invoiceInserter.CreateInvoices(21.03m);
            invoiceInserter.CreateInvoices(15.06m);
            invoiceInserter.CreateInvoices(16.46m);
            invoiceInserter.CreateInvoices(17.15m);
            invoiceInserter.SplitRoyalties = true;
            invoiceInserter.CreateInvoices(17.81m);
            invoiceInserter.CreateInvoices(16.08m);
            invoiceInserter.CreateInvoices(21.03m);
            invoiceInserter.CreateInvoices(21.00m);
            invoiceInserter.CreateInvoices(19.00m);
            invoiceInserter.CreateInvoices(19.44m);

            invoiceInserter.StartJaar(540.00m, 2020);
            invoiceInserter.CreateInvoices(20.56m);
            invoiceInserter.CreateInvoices( 9.31m);
            invoiceInserter.CreateInvoices(22.46m);
            invoiceInserter.CreateInvoices(22.03m);
            invoiceInserter.CreateInvoices(19.78m);
            invoiceInserter.CreateInvoices(20.63m);
            invoiceInserter.CreateInvoices(12.38m);
            invoiceInserter.SetDagprijs(545m);
            invoiceInserter.CreateInvoices(10.81m);
            invoiceInserter.CreateInvoices(22.84m);
            invoiceInserter.CreateInvoices(23.06m);
            invoiceInserter.CreateInvoices(20.19m);
            invoiceInserter.CreateInvoices(22.13m);
            
            invoiceInserter.StartJaar(540.00m, 2021);
            invoiceInserter.CreateInvoices(20.00m);
            invoiceInserter.CreateInvoices(19.75m);
            invoiceInserter.CreateInvoices(23.25m);
            invoiceInserter.CreateInvoices(21.06m);
            invoiceInserter.CreateInvoices(19.13m);
            invoiceInserter.CreateInvoices(21.53m);
            invoiceInserter.CreateInvoices( 9.41m);
            invoiceInserter.CreateInvoices(10.10m);
            invoiceInserter.SetDagprijs(560m);
            invoiceInserter.CreateInvoices(19.66m);
            invoiceInserter.CreateInvoices(20.41m);
            invoiceInserter.CreateInvoices(18.44m);
        }
    }
}