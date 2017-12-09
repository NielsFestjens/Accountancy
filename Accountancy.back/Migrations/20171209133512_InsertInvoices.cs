using System;
using System.Collections.Generic;
using Accountancy.Controllers.Dashboard;
using Accountancy.Domain.Invoices;
using Accountancy.Infrastructure.Database;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Accountancy.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20171209133512_InsertInvoices")]
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

                Action<Company, int, int, InvoiceStatus, decimal?> createInvoice = (receiver, year, month, status, amount) =>
                {
                    var date = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                    context.Add(new Invoice
                    {
                        IssuingCompany = nfSoftware,
                        ReceivingCompany = receiver,
                        Year = year,
                        Month = month,
                        Date = date,
                        ExpiryPeriodDays = 30,
                        Status = status,
                        InvoiceLines = amount == null ? new List<InvoiceLine>() : new List<InvoiceLine>
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
                };

                createInvoice(qframe, 2017, 10, InvoiceStatus.Paid, 2.78m);
                createInvoice(cronos, 2017, 10, InvoiceStatus.Paid, 17.94m);
                createInvoice(qframe, 2017, 11, InvoiceStatus.Sent, 1.53m);
                createInvoice(cronos, 2017, 11, InvoiceStatus.Sent, 20.28m);
                createInvoice(qframe, 2017, 12, InvoiceStatus.Draft, null);
                createInvoice(cronos, 2017, 12, InvoiceStatus.Draft, null);

                context.SaveChanges();
            }
        }
    }
}