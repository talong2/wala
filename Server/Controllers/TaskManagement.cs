using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using PPDIS.Server.Services;
using PPDIS.Shared;
using PPDIS.Shared.Models;

namespace Myproject.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly IMongoDBService _taskService;

    private readonly IConfiguration _config;

    public TaskController(IMongoDBService taskService, IConfiguration config)
    {
        _taskService = taskService;
        _config = config;
    }

    
    
    [HttpPost("InsertStock")]
    public async Task<IActionResult> CreateStock([FromBody] StockCardData stock)
    {
        if (stock == null) return BadRequest("No data received.");
        await _taskService.InsertStock(stock);
        return Ok("ok");
    }
  
    
    [HttpGet("ViewStock")]
    public async Task<ActionResult<List<StockCardData>>> ViewStock()
    {
        var result = await _taskService.GetStock();
        return Ok(result);
    }
    
    
    
 [HttpPost("MDBQueryBuilder")]
    public async Task<ActionResult<string>> MDBQueryBuilder(string db, string Collection, string Aggregates)
    {
        try
        {

            ModelState.Clear();
            var dynamicmongodataaccess = new MongoDBService(_config);
            PipelineDefinition<BsonDocument, BsonDocument> pipeline;
            if (string.IsNullOrEmpty(Aggregates))
            {
                // Handle null or empty Aggregates by using an empty pipeline
                pipeline = PipelineDefinition<BsonDocument, BsonDocument>.Create(new BsonDocument[] { });
            }
            else
            {
                var bsonArray = BsonSerializer.Deserialize<BsonArray>(Aggregates);

                var aggregateStages = bsonArray.Select(x => x.AsBsonDocument).ToArray();

                // You want to use a PipelineDefinition, not a single BsonDocument
                pipeline = PipelineDefinition<BsonDocument, BsonDocument>.Create(aggregateStages);
            }

            var bsondocument = await dynamicmongodataaccess.getDataListFiltered<BsonDocument>(Collection, pipeline);


            //var bsondata = JsonConvert.SerializeObject(bsondocument);
            var jsonData = bsondocument.ToJson();
            // Return the JSON string as content
            return Content(jsonData, "application/json");

        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
    
    

    [HttpPost("PostUserData_public_v2")]
    public IActionResult PostUserData_public_v2(
        [FromBody] object userData,
        [FromQuery] string formname,
        [FromQuery] string RecordID,
        [FromQuery] string db)
    {
        if (userData == null || string.IsNullOrEmpty(formname) || string.IsNullOrEmpty(db))
            return BadRequest("Missing required parameters.");

        try
        {
            var dynamicMongoDataAccess = new MongoDBService(_config);

            var bsonData = BsonDocument.Parse(userData.ToString());
            bsonData["Date Time Entered"] = DateTime.UtcNow.AddHours(8);

            if (!string.IsNullOrEmpty(RecordID))
            {
                if (!ObjectId.TryParse(RecordID, out var objectId))
                    return BadRequest("Invalid RecordID format.");

                var filter = new BsonDocument("_id", objectId);
                
                var result = dynamicMongoDataAccess.UpsertRecordAndReturnID(formname, filter, bsonData);
                return Ok(result);
            }
            else
            {
                bsonData["_id"] = ObjectId.GenerateNewId();
                var result = dynamicMongoDataAccess.InsertRecordAndReturnID(formname, bsonData);
                return Ok(result);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal error: {ex.Message}");
        }
    }
}
