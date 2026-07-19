using Microsoft.EntityFrameworkCore;
using DocumentAI.API.Models;

namespace DocumentAI.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options){}
    public DbSet<Document> Documents{get;set;}
    public DbSet<Chunk> Chunks{get;set;}
}