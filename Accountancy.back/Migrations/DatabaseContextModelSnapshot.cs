﻿// <auto-generated />
using Accountancy.Controllers.Dashboard;
using Accountancy.Domain.Invoices;
using Accountancy.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Accountancy.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Accountancy.Domain.Auth.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Password");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Accountancy.Domain.Invoices.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressLine");

                    b.Property<string>("BankAccount");

                    b.Property<string>("CityLine");

                    b.Property<int?>("ContactPersonId");

                    b.Property<string>("FullName");

                    b.Property<string>("Name");

                    b.Property<string>("Recipients");

                    b.Property<string>("VAT");

                    b.Property<string>("Website");

                    b.HasKey("Id");

                    b.HasIndex("ContactPersonId");

                    b.ToTable("Company");
                });

            modelBuilder.Entity("Accountancy.Domain.Invoices.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<int>("ExpiryPeriodDays");

                    b.Property<int?>("IssuingCompanyId");

                    b.Property<int>("Month");

                    b.Property<int?>("ReceivingCompanyId");

                    b.Property<int>("Status");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.HasIndex("IssuingCompanyId");

                    b.HasIndex("ReceivingCompanyId");

                    b.ToTable("Invoice");
                });

            modelBuilder.Entity("Accountancy.Domain.Invoices.InvoiceLine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<string>("Description");

                    b.Property<int?>("InvoiceId");

                    b.Property<decimal>("Price");

                    b.Property<int>("VatType");

                    b.HasKey("Id");

                    b.HasIndex("InvoiceId");

                    b.ToTable("InvoiceLine");
                });

            modelBuilder.Entity("Accountancy.Domain.Invoices.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Phone");

                    b.HasKey("Id");

                    b.ToTable("Person");
                });

            modelBuilder.Entity("Accountancy.Domain.Invoices.Company", b =>
                {
                    b.HasOne("Accountancy.Domain.Invoices.Person", "ContactPerson")
                        .WithMany()
                        .HasForeignKey("ContactPersonId");
                });

            modelBuilder.Entity("Accountancy.Domain.Invoices.Invoice", b =>
                {
                    b.HasOne("Accountancy.Domain.Invoices.Company", "IssuingCompany")
                        .WithMany()
                        .HasForeignKey("IssuingCompanyId");

                    b.HasOne("Accountancy.Domain.Invoices.Company", "ReceivingCompany")
                        .WithMany()
                        .HasForeignKey("ReceivingCompanyId");
                });

            modelBuilder.Entity("Accountancy.Domain.Invoices.InvoiceLine", b =>
                {
                    b.HasOne("Accountancy.Domain.Invoices.Invoice")
                        .WithMany("InvoiceLines")
                        .HasForeignKey("InvoiceId");
                });
#pragma warning restore 612, 618
        }
    }
}
