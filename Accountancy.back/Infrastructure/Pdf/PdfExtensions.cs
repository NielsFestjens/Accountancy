using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Accountancy.Infrastructure.Pdf;

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