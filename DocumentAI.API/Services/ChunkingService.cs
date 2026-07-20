using System.Text;

namespace DocumentAI.API.Services;

public class ChunkingService
{
    public List<string> ChunkText(string text, int chunkSize = 500)
    {
        var chunks =  new List<string>();
        if (string.IsNullOrWhiteSpace(text))
            return chunks;
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var sb = new StringBuilder();
        foreach(var word in words)
        {
            sb.Append(word+" ");
            if(sb.Length >= chunkSize)
            {
                chunks.Add(sb.ToString().Trim());
                sb.Clear();
            }
        }
        if(sb.Length > 0)
        {
            chunks.Add(sb.ToString().Trim());
        }
        return chunks;
    }
}