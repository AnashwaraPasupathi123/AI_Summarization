using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using DocumentFormat.OpenXml.Packaging;
using System.Text;

namespace DocumentAI.API.Services;

public class TextExtractionService
{
    public async Task<string> ExtractTextAsync(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).ToLower();
        using var stream = file.OpenReadStream();
        return extension switch{
          ".pdf" => ExtractPdf(stream),
          ".docx" => ExtractDocx(stream),
          ".txt" => await ExtractTxt(stream),
           _ => "Unsupported file format"
        };
    }
    private string ExtractPdf(Stream stream)
    {
        var sb = new StringBuilder();

        using var pdf = PdfDocument.Open(stream);

        foreach (var page in pdf.GetPages())
        {
            sb.AppendLine(page.Text);
        }

        return sb.ToString();
    }
    private string ExtractDocx(Stream stream)
    {
        var tempPath = Path.GetTempFileName();

        using (var fileStream = File.Create(tempPath))
        {
            stream.CopyTo(fileStream);
        }

        using var doc = WordprocessingDocument.Open(tempPath, false);

        var sb = new StringBuilder();

        var body = doc.MainDocumentPart.Document.Body;

        sb.Append(body.InnerText);

        return sb.ToString();
    }

    private async Task<string> ExtractTxt(Stream stream)
    {
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}