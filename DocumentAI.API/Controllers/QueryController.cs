using Microsoft.AspNetCore.Mvc;
using DocumentAI.API.Services;
using DocumentAI.API.Models;
using DocumentAI.API.Data;

namespace DocumentAI.API.Controllers;

[ApiController]
[Route("api/query")]
public class QueryController : ControllerBase
{
    private readonly EmbeddingService _embedder;
    private readonly RAGSearchService _rag;
    private readonly LLMService _llm;

    public QueryController(EmbeddingService embedder, RAGSearchService rag, LLMService llm)
    {
        _embedder = embedder;
        _rag = rag;
        _llm = llm;
    }

    [HttpPost]
    public async Task<IActionResult> Ask([FromBody] QueryRequest request)
    {
            if (string.IsNullOrWhiteSpace(request.Question))
                 return BadRequest("Question is required");

            if (request.DocumentId <= 0)
                  return BadRequest("DocumentId is required");

        var queryEmbedding = await _embedder.GetEmbeddingAsync(request.Question);

        var relevantChunks = await _rag.SearchAsync(queryEmbedding, request.DocumentId);

        var chunkTexts = relevantChunks.Select(c => c.Content).ToList();

        var answer = await _llm.AnswerAsync(request.Question, chunkTexts);

        return Ok(new { answer, sources = relevantChunks.Select(c => c.Id).ToList()});
    }
}
public class QueryRequest
{
    public string Question { get; set; }
    public int DocumentId { get; set; }
}