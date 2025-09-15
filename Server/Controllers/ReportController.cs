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
        [FromQuery] string id, string propertyNo, string propertyId)
    {
        var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "Purchase Request.trdp");

        if (!System.IO.File.Exists(reportPath))
            return NotFound("Return report file not found.");

        var reportSource = new UriReportSource { Uri = reportPath };
    
        reportSource.Parameters.Add("id", id);
        reportSource.Parameters.Add("PropertyNo", propertyNo);
        reportSource.Parameters.Add("PropertyId", propertyId);


        var deviceInfo = new System.Collections.Hashtable
        {
            { "FontEmbedding", "Full" }
        };

        var reportProcessor = new ReportProcessor();
        var result = reportProcessor.RenderReport("PDF", reportSource, deviceInfo);

        return File(result.DocumentBytes, "application/pdf");
    }
    
    [HttpGet("StockReportAll")]
    public IActionResult GetReturnPrintViewAll([FromQuery] List<string> id, [FromQuery] List<string> propertyNo)
    {
        var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "Purchase Request All.trdp");

        if (!System.IO.File.Exists(reportPath))
            return NotFound("Return report file not found.");

        var reportSource = new UriReportSource { Uri = reportPath };

        // Example: join ids into a comma-separated string for your report
        reportSource.Parameters.Add("id", string.Join(",", id));
        reportSource.Parameters.Add("PropertyNo", string.Join(",", propertyNo));

        var deviceInfo = new System.Collections.Hashtable
        {
            { "FontEmbedding", "Full" }
        };

        var reportProcessor = new ReportProcessor();
        var result = reportProcessor.RenderReport("PDF", reportSource, deviceInfo);

        return File(result.DocumentBytes, "application/pdf");
    }
    
    
    [HttpGet("QuotationReport")]
    public IActionResult GetQuotationReport(
        [FromQuery] string id, string propertyNo, string propertyId)
    {
        var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "CALL FOR QUOTATION OF PRICES.trdp");

        if (!System.IO.File.Exists(reportPath))
            return NotFound("Quotation report file not found.");

        var reportSource = new UriReportSource { Uri = reportPath };

        reportSource.Parameters.Add("id", id);
        reportSource.Parameters.Add("PropertyNo", propertyNo);
        reportSource.Parameters.Add("PropertyId", propertyId);

        var deviceInfo = new System.Collections.Hashtable
        {
            { "FontEmbedding", "Full" }
        };

        var reportProcessor = new ReportProcessor();
        var result = reportProcessor.RenderReport("PDF", reportSource, deviceInfo);

        return File(result.DocumentBytes, "application/pdf");
    }

    
    
    [HttpGet("InspectionAcceptanceReport")]
    public IActionResult GetInspectionAcceptanceReport(
        [FromQuery] string id, string propertyNo, string propertyId)
    {
        var reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "INSPECTION AND ACCEPTANCE.trdp");

        if (!System.IO.File.Exists(reportPath))
            return NotFound("Inspection and Acceptance report file not found.");

        var reportSource = new UriReportSource { Uri = reportPath };

        reportSource.Parameters.Add("id", id);
        reportSource.Parameters.Add("PropertyNo", propertyNo);
        reportSource.Parameters.Add("PropertyId", propertyId);

        var deviceInfo = new System.Collections.Hashtable
        {
            { "FontEmbedding", "Full" }
        };

        var reportProcessor = new ReportProcessor();
        var result = reportProcessor.RenderReport("PDF", reportSource, deviceInfo);

        return File(result.DocumentBytes, "application/pdf");
    }
    
}