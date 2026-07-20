using System.Net.Http.Json;
using System.Collections.Generic;

namespace DocumentAI.API.Services;

public class EmbeddingService
{
    private readonly HttpClient _http;
    public EmbeddingService (HttpClient http)
    {
        _http = http;
    }
    public async Task<float[]> GetEmbeddingAsync(string text)
    {
        var payload = new
        {
            inputs = text
        };
        var response = await _http.PostAsJsonAsync(
            "https://api-inference.huggingface.co/models/sentence-transformers/all-MiniLM-L6-v2",
            payload);
        var result = await response.Content.ReadFromJsonAsync<List<List<float>>>();

        return result![0].ToArray();
    }
}