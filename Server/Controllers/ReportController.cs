using Microsoft.AspNetCore.Mvc;
using PPDIS.Server.Services;
using Telerik.Reporting;
using Telerik.Reporting.Processing;

namespace Soil.Server.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly IMongoDBService _reportService;

    private readonly IConfiguration _config;

    public ReportController(IMongoDBService taskService, IConfiguration config)
    {
        _reportService = taskService;
        _config = config;
    }

    
    
    [HttpGet("StockReport")]
    public IActionResult GetReturnPrintView(
        [FromQuery] string id)
    {
        var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "Stock.trdp");

        if (!System.IO.File.Exists(reportPath))
            return NotFound("Return report file not found.");

        var reportSource = new UriReportSource { Uri = reportPath };

        reportSource.Parameters.Add("id", id);

        var deviceInfo = new System.Collections.Hashtable
        {
            { "FontEmbedding", "Full" }
        };

        var reportProcessor = new ReportProcessor();
        var result = reportProcessor.RenderReport("PDF", reportSource, deviceInfo);

        return File(result.DocumentBytes, "application/pdf");
    }
    
}