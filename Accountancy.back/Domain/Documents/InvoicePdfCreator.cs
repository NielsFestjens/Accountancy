using System;
using System.IO;
using System.Linq;
using Accountancy.Domain.Invoices;
using Accountancy.Infrastructure.Pdf;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Accountancy.Controllers.Documents
{
    public interface IInvoicePdfCreator
    {
        MemoryStream Generate(Invoice invoice);
    }

    public class InvoicePdfCreator : IInvoicePdfCreator
    {
        public MemoryStream Generate(Invoice invoice)
        {
            var stream = new MemoryStream();
            var document = new Document(PageSize.A4, 50, 50, 25, 25); // total size: 595 x 842, inner size: 495 x 792
            var writer = PdfWriter.GetInstance(document, stream);

            document.Open();
            var directContent = writer.DirectContent;

            GenerateContent(document, directContent, invoice);

            document.Close();
            writer.Close();
            stream.Close();

            return stream;
        }

        private static void GenerateContent(IElementListener document, PdfContentByte directContent, Invoice invoice)
        {
            AddIssuingCompany(directContent, invoice);
            AddInvoiceHeadingPart(directContent, invoice);

            document.Add(GetHeaderPart(invoice));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));
            document.Add(new Paragraph("\n"));

            document.Add(GetCompaniesPart(invoice));
            document.Add(new Paragraph("\n"));
            document.Add(GetInvoiceLinesPart(invoice));
            document.Add(new Paragraph("\n"));
            document.Add(GetFooterPart(invoice));
        }

        private static void AddIssuingCompany(PdfContentByte directContent, Invoice invoice)
        {
            var table = new PdfPTable(3) { HorizontalAlignment = Element.ALIGN_LEFT, WidthPercentage = 100 };
            table.SetWidths(new float[] { 18, 5, 78 });
            table.AddCell(PdfHelper.GetCell("", PdfHelper.SmallBoldFont).Pad(0).AlignRight().PadTop().BorderTop());
            table.AddCell(PdfHelper.GetCell("", PdfHelper.SmallBoldFont).Pad(0));
            table.AddCell(PdfHelper.GetCell(invoice.IssuingCompany.FullName, PdfHelper.SmallBoldFont).Pad(0).PadTop(2).BorderTop());
            table.AddCell(PdfHelper.GetCell("Adres", PdfHelper.SmallFont).Pad(0).AlignRight());
            table.AddCell(PdfHelper.GetCell("", PdfHelper.SmallBoldFont).Pad(0));
            table.AddCell(PdfHelper.GetCell(invoice.IssuingCompany.AddressLine, PdfHelper.SmallFont).Pad(0));
            table.AddCell(PdfHelper.GetCell("", PdfHelper.SmallFont).Pad(0).AlignRight());
            table.AddCell(PdfHelper.GetCell("", PdfHelper.SmallBoldFont).Pad(0));
            table.AddCell(PdfHelper.GetCell(invoice.IssuingCompany.CityLine, PdfHelper.SmallFont).Pad(0).PadBottom());
            table.AddCell(PdfHelper.GetCell("BTW", PdfHelper.SmallFont).Pad(0).AlignRight());
            table.AddCell(PdfHelper.GetCell("", PdfHelper.SmallBoldFont).Pad(0));
            table.AddCell(PdfHelper.GetCell(invoice.IssuingCompany.VAT, PdfHelper.SmallFont).Pad(0));
            table.AddCell(PdfHelper.GetCell("IBAN", PdfHelper.SmallFont).Pad(0).AlignRight());
            table.AddCell(PdfHelper.GetCell("", PdfHelper.SmallBoldFont).Pad(0));
            table.AddCell(PdfHelper.GetCell(invoice.IssuingCompany.BankAccount, PdfHelper.SmallFont).Pad(0).PadBottom());
            table.AddCell(PdfHelper.GetCell("Gsm", PdfHelper.SmallFont).Pad(0).AlignRight());
            table.AddCell(PdfHelper.GetCell("", PdfHelper.SmallBoldFont).Pad(0));
            table.AddCell(PdfHelper.GetCell(invoice.IssuingCompany.ContactPerson.Phone, PdfHelper.SmallFont).Pad(0));
            table.AddCell(PdfHelper.GetCell("E-mail", PdfHelper.SmallFont).Pad(0).PadBottom().AlignRight().BorderBottom());
            table.AddCell(PdfHelper.GetCell("", PdfHelper.SmallBoldFont).Pad(0));
            table.AddCell(PdfHelper.GetCell(invoice.IssuingCompany.ContactPerson.Email, PdfHelper.SmallFont).Pad(0).PadBottom().BorderBottom());

            var columnText = new ColumnText(directContent);
            var size = directContent.PdfDocument.PageSize;
            columnText.SetSimpleColumn(size.Right - 250, size.Top - 25, size.Right - 50, size.Top - 250);
            columnText.AddElement(table);
            columnText.Go();
        }

        private static void AddInvoiceHeadingPart(PdfContentByte directContent, Invoice invoice)
        {
            var table = new PdfPTable(2) { HorizontalAlignment = Element.ALIGN_LEFT, WidthPercentage = 100 };
            table.SetWidths(new float[] { 55, 55 });

            table.AddCell(PdfHelper.GetCell("Factuurdatum", PdfHelper.BoldFont).AlignRight());
            table.AddCell(PdfHelper.GetCell(invoice.Date.ToShortDateString()));

            table.AddCell(PdfHelper.GetCell("Vervaldatum", PdfHelper.BoldFont).AlignRight());
            table.AddCell(PdfHelper.GetCell(invoice.ExpiryDate.ToShortDateString()));

            table.AddCell(PdfHelper.GetCell("Factuurnummer", PdfHelper.BoldFont).AlignRight());
            table.AddCell(PdfHelper.GetCell(invoice.FullNumber).BorderRight());

            if (!string.IsNullOrEmpty(invoice.TheirReference))
            {
                table.AddCell(PdfHelper.GetCell("Uw referentie", PdfHelper.BoldFont).AlignRight());
                table.AddCell(PdfHelper.GetCell(invoice.TheirReference).BorderRight());
            }

            PdfHelper.SurroundBorders(table);

            var columnText = new ColumnText(directContent);
            var size = directContent.PdfDocument.PageSize;
            columnText.SetSimpleColumn(size.Right - 300, size.Top - 135, size.Right - 50, size.Top - 500);
            columnText.AddElement(table);
            columnText.Go();
        }

        private static IElement GetHeaderPart(Invoice invoice)
        {
            return new Paragraph(new Phrase("Factuur van " + invoice.IssuingCompany.Name, PdfHelper.TitleFont));
        }

        private static IElement GetCompaniesPart(Invoice invoice)
        {
            var table = new PdfPTable(1) { HorizontalAlignment = Element.ALIGN_LEFT, WidthPercentage = 50 };

            table.AddCell(PdfHelper.GetCell(invoice.ReceivingCompany.FullName, PdfHelper.LargeBoldFont));
            table.AddCell(PdfHelper.GetCell(invoice.ReceivingCompany.AddressLine, PdfHelper.LargeFont));
            table.AddCell(PdfHelper.GetCell(invoice.ReceivingCompany.CityLine, PdfHelper.LargeFont));
            table.AddCell(PdfHelper.GetCell(invoice.ReceivingCompany.VAT, PdfHelper.LargeFont));

            return table;
        }

        private static IElement GetInvoiceLinesPart(Invoice invoice)
        {
            var table = new PdfPTable(7) { HorizontalAlignment = Element.ALIGN_LEFT, WidthPercentage = 100 };
            table.SetWidths(new float[] { 52, 1, 15, 1, 15, 1, 15 });

            table.AddCell(PdfHelper.GetTitleCell("Omschrijving"));
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetTitleCell("Aantal").AlignRight());
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetTitleCell("Prijs").AlignRight());
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetTitleCell("Totaal").AlignRight());

            foreach (var invoiceLine in invoice.InvoiceLines)
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
            table.AddCell(PdfHelper.GetCell(invoice.TotalExclVat.ToString("C2"), PdfHelper.BoldFont).AlignRight());

            table.AddCell(PdfHelper.GetCell("BTW 21%"));
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetCell("").AlignRight());
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetCell("").AlignRight());
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetCell(invoice.Vat21.ToString("C2")).AlignRight());
            PdfHelper.BorderBottomRow(table.Rows.OfType<PdfPRow>().Last());

            table.AddCell(PdfHelper.GetCell("Totaal", PdfHelper.BoldFont));
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetCell("").AlignRight());
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetCell("").AlignRight());
            table.AddCell(PdfHelper.GetSpacerCell());
            table.AddCell(PdfHelper.GetCell(invoice.Total.ToString("C2"), PdfHelper.BoldFont).AlignRight());

            return table;
        }

        private static IElement GetFooterPart(Invoice invoice)
        {
            return new Paragraph($"Gelieve het bedrag van {invoice.Total:C2} over te maken op {invoice.IssuingCompany.BankAccount} binnen de {invoice.ExpiryPeriodDays} dagen.", PdfHelper.SmallItalicFont);
        }
    }
}