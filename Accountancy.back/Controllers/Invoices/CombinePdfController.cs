using Accountancy.Domain.Documents;
using Accountancy.Domain.Invoices;
using Accountancy.Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Accountancy.Controllers.Invoices;

[Route("api/Invoices/[controller]")]
[AllowAnonymous]
public class CombinePdfController : Controller
{
    private readonly IRepository _repository;
    private readonly IInvoicePdfCreator _invoicePdfCreator;

    public CombinePdfController(IRepository repository, IInvoicePdfCreator invoicePdfCreator)
    {
        _repository = repository;
        _invoicePdfCreator = invoicePdfCreator;
    }

    [HttpPost("{id}")]
    public async Task<ActionResult> Post(int id, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded");
        }

        if (file.ContentType != "application/pdf" && !file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest("Only PDF files are allowed");
        }

        try
        {
            // Get the invoice from database
            var invoice = _repository
                .Query<Invoice>()
                .Include(x => x.IssuingCompany)
                .Include(x => x.IssuingCompany.ContactPerson)
                .Include(x => x.IssuingCompany.Addresses)
                .Include(x => x.ReceivingCompany)
                .Include(x => x.ReceivingCompany.ContactPerson)
                .Include(x => x.ReceivingCompany.Addresses)
                .Include(x => x.InvoiceLines)
                .Single(x => x.Id == id);

            // Generate the invoice PDF
            var invoicePdfStream = _invoicePdfCreator.Generate(invoice);
            
            // Convert to byte array since the stream is closed by the generator
            var invoicePdfBytes = invoicePdfStream.ToArray();
            var invoicePdfMemoryStream = new MemoryStream(invoicePdfBytes);
            
            // Read the uploaded file
            var uploadedPdfStream = new MemoryStream();
            await file.CopyToAsync(uploadedPdfStream);
            uploadedPdfStream.Position = 0;

            // Combine the PDFs
            var combinedPdfStream = CombinePdfs(invoicePdfMemoryStream, uploadedPdfStream);
            
            // Generate filename
            var fileName = $"{invoice.Year}-{invoice.Month:00} - Factuur van {invoice.IssuingCompany.FullName} voor {invoice.ReceivingCompany.FullName} inclusief timesheets.pdf";
            
            Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{fileName}\"");
            return new FileContentResult(combinedPdfStream.ToArray(), "application/pdf");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error combining PDFs: {ex.Message}");
        }
    }

    private MemoryStream CombinePdfs(MemoryStream invoicePdf, MemoryStream uploadedPdf)
    {
        var outputStream = new MemoryStream();
        var document = new Document();
        var writer = PdfWriter.GetInstance(document, outputStream);
        
        document.Open();
        var contentByte = writer.DirectContent;

        // Add invoice PDF pages first
        invoicePdf.Position = 0;
        var invoiceReader = new PdfReader(invoicePdf);
        for (int i = 1; i <= invoiceReader.NumberOfPages; i++)
        {
            document.NewPage();
            var importedPage = writer.GetImportedPage(invoiceReader, i);
            contentByte.AddTemplate(importedPage, 0, 0);
        }

        // Add uploaded PDF pages
        uploadedPdf.Position = 0;
        var uploadedReader = new PdfReader(uploadedPdf);
        for (int i = 1; i <= uploadedReader.NumberOfPages; i++)
        {
            document.NewPage();
            var importedPage = writer.GetImportedPage(uploadedReader, i);
            contentByte.AddTemplate(importedPage, 0, 0);
        }

        document.Close();
        invoiceReader.Close();
        uploadedReader.Close();
        
        outputStream.Position = 0;
        return outputStream;
    }
}