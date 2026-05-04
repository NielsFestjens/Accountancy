using Accountancy.Domain.Documents;
using Accountancy.Domain.Invoices;
using Accountancy.Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;

namespace Accountancy.Controllers.Invoices;

[Route("api/Invoices/[controller]")]
[AllowAnonymous]
public class ExportPeppolController : Controller
{
    private readonly IRepository _repository;
    private readonly IInvoicePdfCreator _invoicePdfCreator;

    public ExportPeppolController(IRepository repository, IInvoicePdfCreator invoicePdfCreator)
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
            
            // Convert combined PDF to base64
            var combinedPdfBytes = combinedPdfStream.ToArray();
            var base64Pdf = Convert.ToBase64String(combinedPdfBytes);

            // Generate XML
            var xml = GenerateUblXml(invoice, base64Pdf);
            
            // Generate filename
            var fileName = $"{invoice.Year}-{invoice.Month:00} - Factuur Peppol.xml";
            
            Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{fileName}\"");
            return new ContentResult
            {
                Content = xml,
                ContentType = "application/xml",
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error generating Peppol XML: {ex.Message}");
        }
    }

    private string GenerateUblXml(Invoice invoice, string base64Pdf)
    {
        var invoiceNr = $"{invoice.Year:0000}{invoice.Id:00000}";
        var issueDate = invoice.Date.ToString("yyyy-MM-dd");
        var dueDate = invoice.Date.AddDays(30).ToString("yyyy-MM-dd");
        var invoiceLine = invoice.InvoiceLines.Single(); // we currently only support single lines
        if (invoice.ReceivingCompany.FullName != "Qframe NV")
            throw new Exception("We hardcoded Qframe here");
        
        return $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<Invoice
    xmlns=""urn:oasis:names:specification:ubl:schema:xsd:Invoice-2""
	xmlns:cac=""urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2""
	xmlns:cbc=""urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"">
    <cbc:CustomizationID>urn:cen.eu:en16931:2017#compliant#urn:fdc:peppol.eu:2017:poacc:billing:3.0</cbc:CustomizationID>
    <cbc:ProfileID>urn:fdc:peppol.eu:2017:poacc:billing:01:1.0</cbc:ProfileID>
	
    <cbc:ID>{invoiceNr}</cbc:ID>
    <cbc:IssueDate>{issueDate}</cbc:IssueDate>
    <cbc:DueDate>{dueDate}</cbc:DueDate>
    <cbc:InvoiceTypeCode>380</cbc:InvoiceTypeCode>
    <cbc:DocumentCurrencyCode>EUR</cbc:DocumentCurrencyCode>
	<cbc:BuyerReference>Contract N.F. Software</cbc:BuyerReference>
	
	<cac:AdditionalDocumentReference>
		<cbc:ID>InvoicePDF</cbc:ID>
		<cac:Attachment>
			<cbc:EmbeddedDocumentBinaryObject
				mimeCode=""application/pdf""
				filename=""invoice-{invoiceNr}.pdf"">
				{base64Pdf}
			</cbc:EmbeddedDocumentBinaryObject>
		</cac:Attachment>
	</cac:AdditionalDocumentReference>
	
    <cac:AccountingSupplierParty>
        <cac:Party>
            <cbc:EndpointID schemeID=""0208"">0681952956</cbc:EndpointID>
            <cac:PartyIdentification>
                <cbc:ID schemeID=""0208"">0681952956</cbc:ID>
            </cac:PartyIdentification>
            <cac:PartyName>
                <cbc:Name>N.F. Software</cbc:Name>
            </cac:PartyName>
            <cac:PostalAddress>
                <cbc:StreetName>Goorlei 26</cbc:StreetName>
                <cbc:CityName>Heist-op-den-Berg</cbc:CityName>
                <cbc:PostalZone>2220</cbc:PostalZone>
                <cac:Country>
                    <cbc:IdentificationCode>BE</cbc:IdentificationCode>
                </cac:Country>
            </cac:PostalAddress>
            <cac:PartyTaxScheme>
                <cbc:CompanyID>BE0681952956</cbc:CompanyID>
                <cac:TaxScheme>
                    <cbc:ID>VAT</cbc:ID>
                </cac:TaxScheme>
            </cac:PartyTaxScheme>
            <cac:PartyLegalEntity>
                <cbc:RegistrationName>N.F. Software BV</cbc:RegistrationName>
                <cbc:CompanyID schemeID=""0208"">0681952956</cbc:CompanyID>
            </cac:PartyLegalEntity>
        </cac:Party>
    </cac:AccountingSupplierParty>
	
    <cac:AccountingCustomerParty>
        <cac:Party>
            <cbc:EndpointID schemeID=""0208"">0887377180</cbc:EndpointID>
            <cac:PartyIdentification>
                <cbc:ID schemeID=""0208"">0887377180</cbc:ID>
            </cac:PartyIdentification>
            <cac:PartyName>
                <cbc:Name>Qframe NV</cbc:Name>
            </cac:PartyName>
            <cac:PostalAddress>
                <cbc:StreetName>Veldkant 33A</cbc:StreetName>
                <cbc:CityName>Kontich</cbc:CityName>
                <cbc:PostalZone>2550</cbc:PostalZone>
                <cac:Country>
                    <cbc:IdentificationCode>BE</cbc:IdentificationCode>
                </cac:Country>
            </cac:PostalAddress>
            <cac:PartyTaxScheme>
                <cbc:CompanyID>BE0887377180</cbc:CompanyID>
                <cac:TaxScheme>
                    <cbc:ID>VAT</cbc:ID>
                </cac:TaxScheme>
            </cac:PartyTaxScheme>
            <cac:PartyLegalEntity>
                <cbc:RegistrationName>Qframe NV</cbc:RegistrationName>
                <cbc:CompanyID schemeID=""0208"">0887377180</cbc:CompanyID>
            </cac:PartyLegalEntity>
            <cac:Contact>
                <cbc:Name>Karina Vereecken</cbc:Name>
                <cbc:Telephone>034508030</cbc:Telephone>
                <cbc:ElectronicMail>Karina.Vereecken@qframe.be</cbc:ElectronicMail>
            </cac:Contact>
        </cac:Party>
    </cac:AccountingCustomerParty>
	
    <cac:PaymentMeans>
        <cbc:PaymentMeansCode name=""Credit transfer"">30</cbc:PaymentMeansCode>
        <cac:PayeeFinancialAccount>
            <cbc:ID>BE75736041796051</cbc:ID>
            <cac:FinancialInstitutionBranch>
                <cbc:ID>KREDBEBB</cbc:ID>
            </cac:FinancialInstitutionBranch>
        </cac:PayeeFinancialAccount>
    </cac:PaymentMeans>
	
    <cac:TaxTotal>
        <cbc:TaxAmount currencyID=""EUR"">{invoice.Vat21.ToString("F2", CultureInfo.InvariantCulture)}</cbc:TaxAmount>
        <cac:TaxSubtotal>
            <cbc:TaxableAmount currencyID=""EUR"">{invoice.TotalExclVat.ToString("F2", CultureInfo.InvariantCulture)}</cbc:TaxableAmount>
            <cbc:TaxAmount currencyID=""EUR"">{invoice.Vat21.ToString("F2", CultureInfo.InvariantCulture)}</cbc:TaxAmount>
            <cac:TaxCategory>
                <cbc:ID>S</cbc:ID>
                <cbc:Percent>21.0</cbc:Percent>
                <cac:TaxScheme>
                    <cbc:ID>VAT</cbc:ID>
                </cac:TaxScheme>
            </cac:TaxCategory>
        </cac:TaxSubtotal>
    </cac:TaxTotal>
	
    <cac:LegalMonetaryTotal>
        <cbc:LineExtensionAmount currencyID=""EUR"">{invoice.TotalExclVat.ToString("F2", CultureInfo.InvariantCulture)}</cbc:LineExtensionAmount>
        <cbc:TaxExclusiveAmount currencyID=""EUR"">{invoice.TotalExclVat.ToString("F2", CultureInfo.InvariantCulture)}</cbc:TaxExclusiveAmount>
        <cbc:TaxInclusiveAmount currencyID=""EUR"">{invoice.Total.ToString("F2", CultureInfo.InvariantCulture)}</cbc:TaxInclusiveAmount>
        <cbc:PayableAmount currencyID=""EUR"">{invoice.Total.ToString("F2", CultureInfo.InvariantCulture)}</cbc:PayableAmount>
    </cac:LegalMonetaryTotal>
    
    <cac:InvoiceLine>
        <cbc:ID>1</cbc:ID>
        <cbc:InvoicedQuantity unitCode=""DAY"">{invoiceLine.Amount.ToString("F2", CultureInfo.InvariantCulture)}</cbc:InvoicedQuantity>
        <cbc:LineExtensionAmount currencyID= ""EUR"">{invoiceLine.TotalExclVat.ToString("F2", CultureInfo.InvariantCulture)}</cbc:LineExtensionAmount>
        <cac:Item>
                <cbc:Name>Gepresteerde dagen</cbc:Name>
                <cac:ClassifiedTaxCategory>
                    <cbc:ID>S</cbc:ID>
                    <cbc:Percent>21.0</cbc:Percent>
                    <cac:TaxScheme>
                        <cbc:ID>VAT</cbc:ID>
                    </cac:TaxScheme>
                </cac:ClassifiedTaxCategory>
            </cac:Item>
        <cac:Price>
            <cbc:PriceAmount currencyID=""EUR"">{invoiceLine.Price.ToString("F2", CultureInfo.InvariantCulture)}</cbc:PriceAmount>
        </cac:Price>
    </cac:InvoiceLine>
</Invoice>";
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