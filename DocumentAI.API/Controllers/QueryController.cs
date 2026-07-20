using Microsoft.AspNetCore.Mvc;
using DocumentAI.API.Services;

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
        var queryEmbedding = await _embedder.GetEmbeddingAsync(request.Question);

        var relevantChunks = await _rag.SearchAsync(queryEmbedding);

        var answer = await _llm.AnswerAsync(request.Question, relevantChunks);

        return Ok(new { answer });
    }
}
public class QueryRequest
{
    public string Question { get; set; }
}