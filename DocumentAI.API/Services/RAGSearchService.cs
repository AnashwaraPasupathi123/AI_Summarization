using DocumentAI.API.Data;
using DocumentAI.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentAI.API.Services;

public class RAGSearchService
{
    private readonly AppDbContext _context;
    public RAGSearchService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<Chunk>> SearchAsync(float[] queryEmbedding,int documentId, int topK = 3)
    {
        var chunks = await _context.Chunks
                     .Where(c => c.DocumentId == documentId)
                     .ToListAsync();
        if (chunks.Count == 0)
            return new List<Chunk>();
        var scoredchunks = chunks
              .Select(c=> new
              {
                  Chunk = c,
                  Score = CosineSimilarity(queryEmbedding, c.Embedding)
              })
              .OrderByDescending(x => x.Score)
              .Take(topK)
              .Select(x => x.Chunk)
              .ToList();
        return scoredchunks;
    }
    private float CosineSimilarity(float[] a, float[] b)
    {
        float dot = 0, magA = 0, magB = 0;

        for (int i = 0; i < a.Length; i++)
        {
            dot += a[i] * b[i];
            magA += a[i] * a[i];
            magB += b[i] * b[i];
        }

        return dot / (float)(Math.Sqrt(magA) * Math.Sqrt(magB));
    }
}