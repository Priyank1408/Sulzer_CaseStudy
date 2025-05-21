using CaseStudy1.Interface;
using CaseStudy3.Interface;
using System.Text.Json;

public class DataIngestionService : IDataIngestionService
{
    private readonly string _storageDirectory;
    private readonly ILogger<DataIngestionService> _logger;

    public DataIngestionService(ILogger<DataIngestionService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _storageDirectory = configuration["LocalStorage:Path"] ?? Path.Combine(Directory.GetCurrentDirectory(), "AppData");
        Directory.CreateDirectory(_storageDirectory);
    }

    public async Task<string> IngestDataAsync(Stream data, string tenantId, string dataType)
    {
        var fileId = Guid.NewGuid().ToString();
        var tenantDirectory = Path.Combine(_storageDirectory, tenantId);
        Directory.CreateDirectory(tenantDirectory);

        var filePath = Path.Combine(tenantDirectory, fileId);
        using var fileStream = File.Create(filePath);
        await data.CopyToAsync(fileStream);

        // Store metadata
        var metadata = new { FileId = fileId, TenantId = tenantId, DataType = dataType };
        await File.WriteAllTextAsync(
            filePath + ".meta.json",
            JsonSerializer.Serialize(metadata)
        );

        _logger.LogInformation($"Data ingested: {fileId} for tenant {tenantId}");
        return fileId;
    }

    public async Task<byte[]> RetrieveDataAsync(string id, string tenantId)
    {
        var filePath = Path.Combine(_storageDirectory, tenantId, id);
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Data not found for id: {id}");
        }

        return await File.ReadAllBytesAsync(filePath);
    }
}