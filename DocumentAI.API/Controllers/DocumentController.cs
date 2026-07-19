using Microsoft.AspNetCore.Mvc;
using DocumentAI.API.Data;
using DocumentAI.API.Models;
using DocumentAI.API.Services;

namespace DocumentAI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentController : ControllerBase
{
  private readonly AppDbContext _context;
  private readonly TextExtractionService _extractor;
  public DocumentController(AppDbContext context, TextExtractionService extractor)
  {
    _context = context;
    _extractor = extractor;
  }
  [HttpPost("upload")]
  public async Task<IActionResult> Upload(IFormFile file)
  {
    if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");
    var text = await _extractor.ExtractTextAsync(file);
    var doc = new Document {
        FileName = file.FileName,
        Text = text,
        UploadedAt = DateTime.UtcNow
    };
    _context.Documents.Add(doc);
    await _context.SaveChangesAsync();
    return Ok(new { documentId = doc.Id });
  }
}