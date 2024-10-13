namespace Pipeline;

using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

public class JsonFilePipelineContextRepository : IPipelineContextRepository
{
    private readonly string _storagePath;

    public JsonFilePipelineContextRepository(string storagePath)
    {
        _storagePath = storagePath;
        if (!Directory.Exists(_storagePath))
        {
            Directory.CreateDirectory(_storagePath);
        }
    }

    public async Task SaveContextAsync<T>(PipelineContext<T> context)
    {
        string fileName = Path.Combine(_storagePath, $"{context.CorrelationId}.json");
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        string json = JsonSerializer.Serialize(context, options);
        await File.WriteAllTextAsync(fileName, json);
    }
}
