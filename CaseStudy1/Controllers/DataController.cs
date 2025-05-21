using CaseStudy1.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "BasicAuthentication")]
public class DataController : ControllerBase
{
    private readonly IDataIngestionService _dataIngestionService;

    public DataController(IDataIngestionService dataIngestionService)
    {
        _dataIngestionService = dataIngestionService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadData(IFormFile file, [FromQuery] string tenantId, [FromQuery] string dataType)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        using var stream = file.OpenReadStream();
        var fileId = await _dataIngestionService.IngestDataAsync(stream, tenantId, dataType);
        return Ok(new { FileId = fileId });
    }

    [HttpGet("download/{tenantId}/{id}")]
    public async Task<IActionResult> DownloadData(string tenantId, string id)
    {
        try
        {
            var data = await _dataIngestionService.RetrieveDataAsync(id, tenantId);
            return File(data, "application/octet-stream");
        }
        catch (FileNotFoundException)
        {
            return NotFound();
        }
    }
}