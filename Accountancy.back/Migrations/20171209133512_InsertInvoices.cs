using System;
using System.Collections.Generic;
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
                context.SaveChanges();

                Action<int, Company, int, int, InvoiceStatus, decimal?, string> createInvoice = (number, receiver, year, month, status, amount, theirReference) =>
                {
                    var date = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                    context.Add(new Invoice
                    {
                        Number = number,
                        IssuingCompany = nfSoftware,
                        ReceivingCompany = receiver,
                        Year = year,
                        Month = month,
                        Date = date,
                        ExpiryPeriodDays = 30,
                        Status = status,
                        TheirReference = theirReference,
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
                    context.SaveChanges();
                };
                
                createInvoice(1, qframe, 2017, 10, InvoiceStatus.Paid,  2.78m, null);
                createInvoice(2, cronos, 2017, 10, InvoiceStatus.Paid, 17.94m, null);
                createInvoice(3, qframe, 2017, 11, InvoiceStatus.Paid,  1.53m, null);
                createInvoice(4, cronos, 2017, 11, InvoiceStatus.Paid, 20.28m, null);
                createInvoice(5, qframe, 2017, 12, InvoiceStatus.Paid,  1.13m, null);
                createInvoice(6, cronos, 2017, 12, InvoiceStatus.Paid, 22.44m, null);

                createInvoice(1, qframe, 2018, 01, InvoiceStatus.Paid,  1.51m, null);
                createInvoice(2, cronos, 2018, 01, InvoiceStatus.Paid, 25.20m, null);
                createInvoice(3, qframe, 2018, 02, InvoiceStatus.Sent,  0.25m, null);
                createInvoice(4, cronos, 2018, 02, InvoiceStatus.Sent, 24.38m, "CRO18/0257/0001");

                context.SaveChanges();
            }
        }
    }
}