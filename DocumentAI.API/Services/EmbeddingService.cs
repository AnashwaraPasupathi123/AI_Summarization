using Microsoft.ML;
using Microsoft.ML.Transforms.Text;
using Microsoft.ML.Data;


namespace DocumentAI.API.Services;

public class EmbeddingService
{
    private readonly MLContext _ml;

    public EmbeddingService()
    {
        _ml = new MLContext();
    }

    public Task<float[]> GetEmbeddingAsync(string text)
    {
        var data = new[] { new InputData { Text = text } };
        var dataView = _ml.Data.LoadFromEnumerable(data);

        var pipeline = _ml.Transforms.Text.FeaturizeText("Features", nameof(InputData.Text));

        var model = pipeline.Fit(dataView);
        var transformed = model.Transform(dataView);

        // Extract features column
        var featuresColumn = transformed.GetColumn<float[]>("Features");

        float[] embedding = featuresColumn.First();

        return Task.FromResult(embedding);
    }

    private class InputData
    {
        public string Text { get; set; }
    }
}
