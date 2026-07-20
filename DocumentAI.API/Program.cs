using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite; 
using DocumentAI.API.Data;
using DocumentAI.API.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=documentai.db"));
builder.Services.AddScoped<TextExtractionService>();
builder.Services.AddScoped<ChunkingService>();
builder.Services.AddSingleton<EmbeddingService>();
builder.Services.AddSingleton<LLMService>();
builder.Services.AddScoped<RAGSearchService>();
builder.Services.AddControllers();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();


app.Run();
