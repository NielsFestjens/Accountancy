using System;
using System.Collections.Generic;
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
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            using (var context = DatabaseContextFacory.Create())
            {
                var niels = new Person
                {
                    FirstName = "Niels",
                    LastName = "Festjens",
                    Email = "festjens_niels@hotmail.com",
                    Phone = "+32 477 / 60 39 05"
                };
                context.Add(niels);

                var danny = new Person
                {
                    FirstName = "Danny",
                    LastName = "Gladines",
                    Email = "danny.gladines@qframe.be"
                };
                context.Add(danny);

                var nfSoftware = new Company
                {
                    ContactPerson = niels,
                    Name = "N.F. Software",
                    FullName = "N.F. Software Comm.V",
                    AddressLine = "Hollebeekstraat 5 bus 3",
                    CityLine = "2840 Rumst",
                    VAT = "BE 0681.952.956",
                    BankAccount = "BE75 7360 4179 6051"
                };
                context.Add(nfSoftware);

                var qframe = new Company
                {
                    ContactPerson = danny,
                    Name = "Qframe",
                    FullName = "Qframe NV",
                    AddressLine = "Veldkant 33A",
                    CityLine = "2550 Kontich",
                    VAT = "BE 0887.377.180",
                    Recipients = "karina.vereecken@qframe.be"
                };
                context.Add(qframe);

                var cronos = new Company
                {
                    Name = "Cronos",
                    FullName = "Cronos NV",
                    AddressLine = "Veldkant 35D",
                    CityLine = "2550 Kontich",
                    VAT = "BE 0443.807.959",
                    Recipients = "daria.wycislo@cronos.be; heidi.lens@cronos.be; karina.vereecken@qframe.be"
                };
                context.Add(cronos);
                context.SaveChanges();

                int factuurNr;

                void CreateInvoice(Company receiver, int year, int month, InvoiceStatus status, decimal? amount, string theirReference)
                {
                    var date = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                    context.Add(new Invoice
                    {
                        Number = ++factuurNr,
                        IssuingCompany = nfSoftware,
                        ReceivingCompany = receiver,
                        Year = year,
                        Month = month,
                        Date = date,
                        ExpiryPeriodDays = 30,
                        Status = status,
                        TheirReference = theirReference,
                        InvoiceLines = amount == null
                            ? new List<InvoiceLine>()
                            : new List<InvoiceLine>
                            {
                                new InvoiceLine
                                {
                                    Description = "Gepresteerde dagen",
                                    Amount = amount.Value,
                                    Price = 520.00m,
                                    VatType = VatType.Vat21
                                }
                            }
                    });
                    context.SaveChanges();
                }

                void CreateInvoices(int year, int month, decimal? amountQframe, decimal amountCronos, string referenceCronos = null)
                {
                    if (amountQframe.HasValue)
                        CreateInvoice(qframe, year, month, InvoiceStatus.Sent, amountQframe, null);

                    CreateInvoice(cronos, year, month, InvoiceStatus.Sent, amountCronos, referenceCronos);
                }

                factuurNr = 0;
                CreateInvoices(2017, 10, 2.78m, 17.94m);
                CreateInvoices(2017, 11, 1.53m, 20.28m);
                CreateInvoices(2017, 12, 1.13m, 22.44m);

                factuurNr = 0;
                CreateInvoices(2018, 01, 1.51m, 25.20m);
                CreateInvoices(2018, 02, 0.25m, 24.38m, "CRO18/0257/0001");
                CreateInvoices(2018, 03, 0.38m, 28.13m, "CRO18/0257/0001");
                CreateInvoices(2018, 04,  null, 26.06m, "CRO18/0257/0001");
                CreateInvoices(2018, 05, 0.94m, 19.44m, "CRO18/0257/0001");
                CreateInvoices(2018, 06, 1.50m, 19.50m, "CRO18/0257/0001");
                CreateInvoices(2018, 07, 1.13m, 17.69m, "CRO18/0257/0001");
                CreateInvoices(2018, 08, 0.75m, 11.94m, "CRO18/0257/0001");
                CreateInvoices(2018, 09, 2.38m, 18.56m, "CRO18/0257/0001");
                CreateInvoices(2018, 10, 2.58m, 23.09m, "CRO18/0257/0001");
                CreateInvoices(2018, 11, 1.50m, 19.75m, "CRO18/0257/0001");
                CreateInvoices(2018, 12, 0.56m, 19.44m, "CRO18/0257/0001");

                factuurNr = 0;

                context.SaveChanges();
            }
        }
    }
}