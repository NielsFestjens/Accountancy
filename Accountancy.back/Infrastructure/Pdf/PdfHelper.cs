using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Accountancy.Infrastructure.Pdf
{
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
}