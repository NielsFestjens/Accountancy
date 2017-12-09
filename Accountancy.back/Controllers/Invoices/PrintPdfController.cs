using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Accountancy.Domain.Invoices;
using Accountancy.Infrastructure.Database;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Accountancy.Controllers.Invoices
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class PrintPdfController : Controller
    {
        private readonly IRepository _repository;

        public PrintPdfController(IRepository repository)
        {
            _repository = repository;
        }

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

            var model = _repository
                .Query<Invoice>()
                .Include(x => x.IssuingCompany)
                .Include(x => x.IssuingCompany.ContactPerson)
                .Include(x => x.ReceivingCompany)
                .Include(x => x.ReceivingCompany.ContactPerson)
                .Include(x => x.InvoiceLines)
                .Single(x => x.Id == id);

            GenerateContent(document, directContent, model);

            document.Close();
            writer.Close();
            stream.Close();
            
            Response.Headers.Add("Content-Disposition", $"inline; filename={model.Year}-{model.Month} - Factuur van {model.IssuingCompany.FullName} voor {model.ReceivingCompany.FullName}.pdf");
            return new FileContentResult(stream.ToArray(), "application/pdf");
        }

        private static void GenerateContent(IElementListener document, PdfContentByte directContent, Invoice model)
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

        private static void AddIssuingCompany(PdfContentByte directContent, Invoice model)
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

        private static void AddInvoiceHeadingPart(PdfContentByte directContent, Invoice model)
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

        private static IElement GetHeaderPart(Invoice model)
        {
            return new Paragraph(new Phrase(model.IssuingCompany.Name, PdfHelper.TitleFont));
        }

        private static IElement GetCompaniesPart(Invoice model)
        {
            var table = new PdfPTable(1) { HorizontalAlignment = Element.ALIGN_LEFT, WidthPercentage = 50 };

            table.AddCell(PdfHelper.GetCell(model.ReceivingCompany.FullName, PdfHelper.LargeBoldFont));
            table.AddCell(PdfHelper.GetCell(model.ReceivingCompany.AddressLine, PdfHelper.LargeFont));
            table.AddCell(PdfHelper.GetCell(model.ReceivingCompany.CityLine, PdfHelper.LargeFont));
            table.AddCell(PdfHelper.GetCell(model.ReceivingCompany.VAT, PdfHelper.LargeFont));

            return table;
        }

        private static IElement GetInvoiceLinesPart(Invoice model)
        {
            var table = new PdfPTable(7) {HorizontalAlignment = Element.ALIGN_LEFT, WidthPercentage = 100};
            table.SetWidths(new float[] {52, 1, 15, 1, 15, 1, 15});

            table.AddCell(PdfHelper.GetTitleCell("Omschrijving"));
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetTitleCell("Aantal").AlignRight());
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetTitleCell("Prijs").AlignRight());
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetTitleCell("Totaal").AlignRight());

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
            PdfHelper.BorderBottomRow(table.Rows.OfType<PdfPRow>().Last());

            table.AddCell(PdfHelper.GetCell("Totaal Excl. BTW", PdfHelper.BoldFont));
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetCell("").AlignRight());
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetCell("").AlignRight());
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetCell(model.TotalExclVat.ToString("C2"), PdfHelper.BoldFont).AlignRight());

            table.AddCell(PdfHelper.GetCell("BTW 21%"));
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetCell("").AlignRight());
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetCell("").AlignRight());
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetCell(model.Vat21.ToString("C2")).AlignRight());
            PdfHelper.BorderBottomRow(table.Rows.OfType<PdfPRow>().Last());

            table.AddCell(PdfHelper.GetCell("Totaal", PdfHelper.BoldFont));
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetCell("").AlignRight());
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetCell("").AlignRight());
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetCell(model.Total.ToString("C2"), PdfHelper.BoldFont).AlignRight());

            return table;
        }

        private static IElement GetFooterPart(Invoice model)
        {
            return new Paragraph($"Gelieve het bedrag van {model.Total:C2} over te maken op {model.IssuingCompany.BankAccount} binnen de {model.ExpiryPeriodDays} dagen.", PdfHelper.SmallItalicFont);
        }
    }

    public static class PdfHelper
    {
        public static Font BaseFont = new Font(Font.UNDEFINED, 12);
        public static Font TitleFont = new Font(BaseFont.BaseFont, BaseFont.Size + 8, Font.BOLD);
        public static Font BoldFont = new Font(BaseFont.BaseFont, BaseFont.Size, Font.BOLD);

        public static Font LargeFont = new Font(BaseFont.BaseFont, BaseFont.Size + 2);
        public static Font LargeBoldFont = new Font(BaseFont.BaseFont, BaseFont.Size + 2, Font.BOLD);

        public static Font SmallFont = new Font(BaseFont.BaseFont, BaseFont.Size - 2);
        public static Font SmallBoldFont = new Font(BaseFont.BaseFont, BaseFont.Size - 2, Font.BOLD);
        public static Font SmallItalicFont = new Font(BaseFont.BaseFont, BaseFont.Size - 2, Font.ITALIC);

        public static void SurroundBorders(PdfPTable table)
        {
            var rows = table.Rows.OfType<PdfPRow>().ToList();
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

    public class InvoiceData
    {
        public decimal DaysWorked { get; set; }
        public Company Company { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}