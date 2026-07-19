namespace DocumentAI.API.Models;

public class Chunk
{
    public int Id { get; set; }
    public int DocumentId { get; set; }
    public string Content { get; set; }
    public float[] Embedding { get; set; }
}
