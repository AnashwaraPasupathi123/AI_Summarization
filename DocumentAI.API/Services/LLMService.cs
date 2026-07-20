using System.Net.Http.Json;
using DocumentAI.API.Models;

namespace DocumentAI.API.Services;

public class LLMService
{
    private readonly HttpClient _http;

    public LLMService(HttpClient http)
    {
        _http = http;
    }

    public async Task<string> AnswerAsync(string question, List<Chunk> chunks)
    {
        var context = string.Join("\n\n", chunks.Select(c => c.Content));

        var prompt = $"Context:\n{context}\n\nQuestion: {question}\nAnswer:";

        var payload = new
        {
            inputs = prompt
        };

        var response = await _http.PostAsJsonAsync(
            "https://api-inference.huggingface.co/models/google/gemma-2-2b-it",
            payload);

        var result = await response.Content.ReadFromJsonAsync<List<string>>();

        return result?[0] ?? "No answer generated.";
    }
}