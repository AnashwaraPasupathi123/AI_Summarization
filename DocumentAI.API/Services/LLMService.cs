using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace DocumentAI.API.Services;

public class LLMService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;
    private readonly string _model;

    public LLMService(IConfiguration config)
    {
        _http = new HttpClient();
        _apiKey = config["Groq:ApiKey"];
        _model = config["Groq:Model"] ?? "groq/compound-mini";
    }

    public async Task<string> AnswerAsync(string question, List<string> chunks)
    {
        var prompt = $"Context:\n{string.Join("\n", chunks)}\n\nQuestion: {question}";

        var requestBody = new
        {
            model = _model,
            messages = new[]
            {
                new {
                    role = "system",
                    content = "You are a helpful assistant. Use the provided context to answer the user's question or summarize the document when asked."
                },
                new {
                    role = "user",
                    content = prompt
                }
            }
        };

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            "https://api.groq.com/openai/v1/chat/completions"
        );

        request.Headers.Add("Authorization", $"Bearer {_apiKey}");
        request.Content = JsonContent.Create(requestBody);

        var response = await _http.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);

        // ⭐ If Groq returned an error, show it
        if (doc.RootElement.TryGetProperty("error", out var error))
        {
            var msg = error.GetProperty("message").GetString();
            return $"Groq API Error: {msg}";
        }

        // ⭐ Safe parsing of choices
        if (!doc.RootElement.TryGetProperty("choices", out var choices))
        {
            return "Groq API returned unexpected JSON: " + json;
        }

        var content = choices[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return content;
    }
}
