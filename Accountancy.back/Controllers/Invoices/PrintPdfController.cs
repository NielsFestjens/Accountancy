using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accountancy.Controllers.Invoices
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class PrintPdfController : Controller
    {
        [HttpGet]
        public ActionResult Get(int id)
        {
            if (id == 0)
                return RedirectToAction("Get", new {id = 1});

            var stream = new MemoryStream();
            var document = new Document(PageSize.A4, 50, 50, 25, 25); // total size: 595 x 842, inner size: 495 x 792
            var writer = PdfWriter.GetInstance(document, stream);
            
            document.Open();
            var directContent = writer.DirectContent;

            var model = GetModel(id);

            GenerateContent(document, directContent, model);

            document.Close();
            writer.Close();
            stream.Close();
            
            Response.Headers.Add("Content-Disposition", $"inline; filename={model.Year}-{model.Month} - Factuur van {model.IssuingCompany.FullName} voor {model.ReceivingCompany.FullName}.pdf");
            return new FileContentResult(stream.ToArray(), "application/pdf");
        }

        private static void GenerateContent(IElementListener document, PdfContentByte directContent, InvoiceModel model)
        {
            AddIssuingCompany(directContent, model);
            AddInvoiceHeadingPart(directContent, model);

            document.Add(GetHeaderPart(model));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            
            document.Add(GetCompaniesPart(model));
            document.Add(new Paragraph("\n"));
            document.Add(GetInvoiceLinesPart(model));
            document.Add(new Paragraph("\n"));
            document.Add(GetFooterPart(model));
        }

        private static void AddIssuingCompany(PdfContentByte directContent, InvoiceModel model)
        {
            var table = new PdfPTable(3) { HorizontalAlignment = Element.ALIGN_LEFT, WidthPercentage = 100 };
            table.SetWidths(new float[] { 18, 5, 78 });
            table.AddCell(PdfHelper.GetCell("", PdfHelper.SmallBoldFont).Pad(0).AlignRight().PadTop().BorderTop());
            table.AddCell(PdfHelper.GetCell("", PdfHelper.SmallBoldFont).Pad(0));
            table.AddCell(PdfHelper.GetCell(model.IssuingCompany.FullName, PdfHelper.SmallBoldFont).Pad(0).PadTop(2).BorderTop());
            table.AddCell(PdfHelper.GetCell("Adres", PdfHelper.SmallFont).Pad(0).AlignRight());
            table.AddCell(PdfHelper.GetCell("", PdfHelper.SmallBoldFont).Pad(0));
            table.AddCell(PdfHelper.GetCell(model.IssuingCompany.AddressLine, PdfHelper.SmallFont).Pad(0));
            table.AddCell(PdfHelper.GetCell("", PdfHelper.SmallFont).Pad(0).AlignRight());
            table.AddCell(PdfHelper.GetCell("", PdfHelper.SmallBoldFont).Pad(0));
            table.AddCell(PdfHelper.GetCell(model.IssuingCompany.CityLine, PdfHelper.SmallFont).Pad(0).PadBottom());
            table.AddCell(PdfHelper.GetCell("BTW", PdfHelper.SmallFont).Pad(0).AlignRight());
            table.AddCell(PdfHelper.GetCell("", PdfHelper.SmallBoldFont).Pad(0));
            table.AddCell(PdfHelper.GetCell(model.IssuingCompany.VAT, PdfHelper.SmallFont).Pad(0));
            table.AddCell(PdfHelper.GetCell("IBAN", PdfHelper.SmallFont).Pad(0).AlignRight());
            table.AddCell(PdfHelper.GetCell("", PdfHelper.SmallBoldFont).Pad(0));
            table.AddCell(PdfHelper.GetCell(model.IssuingCompany.BankAccount, PdfHelper.SmallFont).Pad(0).PadBottom());
            table.AddCell(PdfHelper.GetCell("Gsm", PdfHelper.SmallFont).Pad(0).AlignRight());
            table.AddCell(PdfHelper.GetCell("", PdfHelper.SmallBoldFont).Pad(0));
            table.AddCell(PdfHelper.GetCell(model.IssuingCompany.ContactPerson.Phone, PdfHelper.SmallFont).Pad(0));
            table.AddCell(PdfHelper.GetCell("E-mail", PdfHelper.SmallFont).Pad(0).PadBottom().AlignRight().BorderBottom());
            table.AddCell(PdfHelper.GetCell("", PdfHelper.SmallBoldFont).Pad(0));
            table.AddCell(PdfHelper.GetCell(model.IssuingCompany.ContactPerson.Email, PdfHelper.SmallFont).Pad(0).PadBottom().BorderBottom());

            var columnText = new ColumnText(directContent);
            var size = directContent.PdfDocument.PageSize;
            columnText.SetSimpleColumn(size.Right - 250, size.Top - 25, size.Right - 50, size.Top - 250);
            columnText.AddElement(table);
            columnText.Go();
        }

        private static void AddInvoiceHeadingPart(PdfContentByte directContent, InvoiceModel model)
        {
            var table = new PdfPTable(2) { HorizontalAlignment = Element.ALIGN_LEFT, WidthPercentage = 100 };
            table.SetWidths(new float[] {55, 45});

            table.AddCell(PdfHelper.GetCell("Factuurdatum", PdfHelper.BoldFont).AlignRight());
            table.AddCell(PdfHelper.GetCell(model.Date.ToShortDateString()));

            table.AddCell(PdfHelper.GetCell("Vervaldatum", PdfHelper.BoldFont).AlignRight());
            table.AddCell(PdfHelper.GetCell(model.ExpiryDate.ToShortDateString()));

            table.AddCell(PdfHelper.GetCell("Factuurnummer", PdfHelper.BoldFont).AlignRight());
            table.AddCell(PdfHelper.GetCell(model.FullNumber).BorderRight());

            PdfHelper.SurroundBorders(table);

            var columnText = new ColumnText(directContent);
            var size = directContent.PdfDocument.PageSize;
            columnText.SetSimpleColumn(size.Right - 250, size.Top - 135, size.Right - 50, size.Top - 500);
            columnText.AddElement(table);
            columnText.Go();
        }

        private static IElement GetHeaderPart(InvoiceModel model)
        {
            return new Paragraph(new Phrase(model.IssuingCompany.Name, PdfHelper.TitleFont));
        }

        private static IElement GetCompaniesPart(InvoiceModel model)
        {
            var table = new PdfPTable(1) { HorizontalAlignment = Element.ALIGN_LEFT, WidthPercentage = 50 };

            table.AddCell(PdfHelper.GetCell(model.ReceivingCompany.FullName, PdfHelper.LargeBoldFont));
            table.AddCell(PdfHelper.GetCell(model.ReceivingCompany.AddressLine, PdfHelper.LargeFont));
            table.AddCell(PdfHelper.GetCell(model.ReceivingCompany.CityLine, PdfHelper.LargeFont));
            table.AddCell(PdfHelper.GetCell(model.ReceivingCompany.VAT, PdfHelper.LargeFont));

            return table;
        }

        private static IElement GetInvoiceLinesPart(InvoiceModel model)
        {
            var table = new PdfPTable(7) {HorizontalAlignment = Element.ALIGN_LEFT, WidthPercentage = 100};
            table.SetWidths(new float[] {52, 1, 15, 1, 15, 1, 15});

            table.AddCell(PdfHelper.GetTitleCell("Omschrijving"));
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetTitleCell("Aantal")).AlignRight();
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetTitleCell("Prijs")).AlignRight();
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetTitleCell("Totaal")).AlignRight();

            foreach (var invoiceLine in model.InvoiceLines)
            {
                table.AddCell(PdfHelper.GetCell(invoiceLine.Description));
                table.AddCell(PdfHelper.GetSpacerCell());
                table.AddCell(PdfHelper.GetCell(invoiceLine.Amount.ToString("F2")).AlignRight());
                table.AddCell(PdfHelper.GetSpacerCell());
                table.AddCell(PdfHelper.GetCell(invoiceLine.Price.ToString("C2")).AlignRight());
                table.AddCell(PdfHelper.GetSpacerCell());
                table.AddCell(PdfHelper.GetCell(invoiceLine.TotalExclVat.ToString("C2")).AlignRight());
            }
            PdfHelper.BorderBottomRow(table.Rows.Last());

            table.AddCell(PdfHelper.GetCell("Totaal Excl. BTW", PdfHelper.BoldFont));
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetCell("")).AlignRight();
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetCell("")).AlignRight();
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetCell(model.TotalExclVat.ToString("C2"), PdfHelper.BoldFont)).AlignRight();

            table.AddCell(PdfHelper.GetCell("BTW 21%"));
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetCell("")).AlignRight();
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetCell("")).AlignRight();
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetCell(model.Vat21.ToString("C2"))).AlignRight();
            PdfHelper.BorderBottomRow(table.Rows.Last());

            table.AddCell(PdfHelper.GetCell("Totaal", PdfHelper.BoldFont));
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetCell("")).AlignRight();
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetCell("")).AlignRight();
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetCell(model.Total.ToString("C2"), PdfHelper.BoldFont)).AlignRight();

            return table;
        }

        private static IElement GetFooterPart(InvoiceModel model)
        {
            return new Paragraph($"Gelieve het bedrag van {model.Total:C2} over te maken op {model.IssuingCompany.BankAccount} binnen de {model.ExpiryPeriod.TotalDays} dagen.", PdfHelper.SmallItalicFont);
        }

        private static InvoiceModel GetModel(int id)
        {
            var nfSoftware = new Company
            {
                Name = "N.F. Software",
                FullName = "N.F. Software Comm.V",
                AddressLine = "Hollebeekstraat 5 bus 3",
                CityLine = "2840 Rumst",
                VAT = "BE 0681.952.956",
                BankAccount = "BE75 7360 4179 6051",
                ContactPerson = new Person
                {
                    FirstName = "Niels",
                    LastName = "Festjens",
                    Email = "festjens_niels@hotmail.com",
                    Phone = "+32 477 / 60 39 05"
                },
                Website = null
            };

            var qframe = new Company
            {
                Name = "Qframe",
                FullName = "Qframe NV",
                AddressLine = "Veldkant 33A",
                CityLine = "2550 Kontich",
                VAT = "BE 0887.377.180",
                ContactPerson = new Person
                {
                    FirstName = "Danny",
                    LastName = "Gladines"
                },
                Recipients = "karina.vereecken@qframe.be"
            };

            var cronos = new Company
            {
                Name = "Cronos",
                FullName = "Cronos NV",
                AddressLine = "Veldkant 35D",
                CityLine = "2550 Kontich",
                VAT = "BE 0443.807.959",
                Recipients = "daria.wycislo@cronos.be; heidi.lens@cronos.be; karina.vereecken@qframe.be"
            };

            var invoicedatas = new Dictionary<int, InvoiceData>
            {
                { 1, new InvoiceData { Year = 2017, Month = 10, Company = qframe, DaysWorked =  2.78m } },
                { 2, new InvoiceData { Year = 2017, Month = 10, Company = cronos, DaysWorked = 17.94m } },

                { 3, new InvoiceData { Year = 2017, Month = 11, Company = qframe, DaysWorked =  1.53m } },
                { 4, new InvoiceData { Year = 2017, Month = 11, Company = cronos, DaysWorked = 20.28m } },

                { 5, new InvoiceData { Year = 2017, Month = 12, Company = qframe, DaysWorked =   0.0m } },
                { 6, new InvoiceData { Year = 2017, Month = 12, Company = cronos, DaysWorked =   0.0m } },
            };

            var invoiceData = invoicedatas[id];
            
            var model = new InvoiceModel
            {
                Number = id,
                Year = invoiceData.Year,
                Month = invoiceData.Month,
                Date = new DateTime(invoiceData.Year, invoiceData.Month, DateTime.DaysInMonth(invoiceData.Year, invoiceData.Month)),
                ExpiryPeriod = TimeSpan.FromDays(30),
                IssuingCompany = nfSoftware,
                ReceivingCompany = invoiceData.Company,
                Recipients = invoiceData.Company.Recipients,
                InvoiceLines = new List<InvoiceLine>
                {
                    new InvoiceLine
                    {
                        Description = "Gepresteerde dagen",
                        Amount = invoiceData.DaysWorked,
                        Price = 520.00m,
                        VatType = VatType.Vat21
                    }
                },
            };
            return model;
        }
    }

    public static class PdfHelper
    {
        public static Font BaseFont = new Font(Font.FontFamily.UNDEFINED, 12);
        public static Font TitleFont = new Font(BaseFont.BaseFont, BaseFont.Size + 8, Font.BOLD);
        public static Font BoldFont = new Font(BaseFont.BaseFont, BaseFont.Size, Font.BOLD);

        public static Font LargeFont = new Font(BaseFont.BaseFont, BaseFont.Size + 2);
        public static Font LargeBoldFont = new Font(BaseFont.BaseFont, BaseFont.Size + 2, Font.BOLD);

        public static Font SmallFont = new Font(BaseFont.BaseFont, BaseFont.Size - 2);
        public static Font SmallBoldFont = new Font(BaseFont.BaseFont, BaseFont.Size - 2, Font.BOLD);
        public static Font SmallItalicFont = new Font(BaseFont.BaseFont, BaseFont.Size - 2, Font.ITALIC);

        public static void SurroundBorders(PdfPTable table)
        {
            var rows = table.Rows;
            if (!rows.Any())
                return;
            
            BorderTopRow(rows[0]);

            foreach (var row in rows)
            {
                var cells = row.GetCells();
                if (!cells.Any())
                    continue;

                cells[0].BorderLeft();
                cells[cells.Length - 1].BorderRight();
            }

            BorderBottomRow(rows[rows.Count - 1]);
        }

        public static void BorderTopRow(PdfPRow row)
        {
            foreach (var cell in row.GetCells())
            {
                cell.BorderTop();
            }
        }

        public static void BorderBottomRow(PdfPRow row)
        {
            foreach (var cell in row.GetCells())
            {
                cell.BorderBottom();
            }
        }

        public static PdfPCell GetCell(string text, Font font = null)
        {
            return new PdfPCell(new Phrase(text, font ?? BaseFont)).Pad().NoBorder();
        }

        public static PdfPCell GetTitleCell(string text)
        {
            return GetCell(text, BoldFont).BorderBottom();
        }

        public static PdfPCell GetSpacerCell()
        {
            return GetCell("");
        }
    }

    public static class PdfExtensions
    {
        public static T NoBorder<T>(this T rectangle) where T : Rectangle => rectangle.SetBorder(Rectangle.NO_BORDER);
        public static T BorderTop<T>(this T rectangle) where T : Rectangle => rectangle.AddBorder(Rectangle.TOP_BORDER);
        public static T BorderRight<T>(this T rectangle) where T : Rectangle => rectangle.AddBorder(Rectangle.RIGHT_BORDER);
        public static T BorderBottom<T>(this T rectangle) where T : Rectangle => rectangle.AddBorder(Rectangle.BOTTOM_BORDER);

        public static T BorderLeft<T>(this T rectangle) where T : Rectangle => rectangle.AddBorder(Rectangle.LEFT_BORDER);
        public static T BorderHorizontal<T>(this T rectangle) where T : Rectangle => rectangle.AddBorder(Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER);
        public static T BorderVertical<T>(this T rectangle) where T : Rectangle => rectangle.AddBorder(Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER);

        public static T SetBorder<T>(this T rectangle, int border) where T : Rectangle
        {
            rectangle.Border = border;
            return rectangle;
        }

        public static T AddBorder<T>(this T rectangle, int border) where T : Rectangle
        {
            rectangle.Border = rectangle.Border | border;
            return rectangle;
        }

        public static T BorderColorBottom<T>(this T rectangle, BaseColor color) where T : Rectangle
        {
            rectangle.BorderColorBottom = color;
            return rectangle;
        }

        public static PdfPCell AlignRight(this PdfPCell cell)
        {
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            return cell;
        }

        public static PdfPCell Pad(this PdfPCell cell, int padding = 5)
        {
            cell.Padding = padding;
            return cell;
        }

        public static PdfPCell PadTop(this PdfPCell cell, int padding = 5) => cell.Do(() => cell.PaddingTop = padding);
        public static PdfPCell PadRight(this PdfPCell cell, int padding = 5) => cell.Do(() => cell.PaddingRight = padding);
        public static PdfPCell PadBottom(this PdfPCell cell, int padding = 5) => cell.Do(() => cell.PaddingBottom = padding);
        public static PdfPCell PadLeft(this PdfPCell cell, int padding = 5) => cell.Do(() => cell.PaddingLeft = padding);

        public static T Do<T>(this T item, Action action)
        {
            action();
            return item;
        }
    }

    public class InvoiceModel
    {
        public int Number { get; set; }
        public string FullNumber => $"{Date.Year}{Number:00000}";
        public DateTime Date { get; set; }
        public TimeSpan ExpiryPeriod { get; set; }
        public DateTime ExpiryDate => Date.Add(ExpiryPeriod);

        public Company IssuingCompany { get; set; }
        public Company ReceivingCompany { get; set; }

        public List<InvoiceLine> InvoiceLines { get; set; }
        public decimal TotalExclVat => InvoiceLines.Sum(x => x.TotalExclVat);
        public decimal TotalExclVatForVat21 => InvoiceLines.Where(x => x.VatType == VatType.Vat21).Sum(x => x.TotalExclVat);
        public decimal Vat21 => InvoiceLines.Where(x => x.VatType == VatType.Vat21).Sum(x => x.TotalExclVat * 21 / 100);
        public decimal Total => TotalExclVat + Vat21; 
        public string Recipients { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }

    public class Company
    {
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

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class InvoiceLine
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public decimal TotalExclVat => Amount * Price;
        public VatType VatType { get; set; }
    }

    public enum VatType
    {
        Vat21 = 21
    }

    public class InvoiceData
    {
        public decimal DaysWorked { get; set; }
        public Company Company { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}