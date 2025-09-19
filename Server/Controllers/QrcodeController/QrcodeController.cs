using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Soil.Shared.Models;

namespace Soil.Server.Controllers.QrcodeController;

[ApiController]
[Route("api/qrcode")]
public class QrcodeController : ControllerBase
{
    private readonly IMongoCollection<QrCodeClass> _items;
    private readonly IMongoDatabase _database;

    public QrcodeController(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetConnectionString("MongoConnection"));
        _database = client.GetDatabase("DataBase"); // Database name
        _items = _database.GetCollection<QrCodeClass>("vw_req"); // Collection name
    }

    [HttpGet("view_qr")]
    public async Task<ActionResult<List<QrCodeClass>>> GetAll()
    {
        var items = await _items.Find(_ => true).ToListAsync();
        return Ok(items);
    }

    [HttpPost("scan")]
    public async Task<ActionResult<string>> Create([FromBody] QrCodeClass item)
    {
        await _items.InsertOneAsync(item);
        return Ok(item.Id);
    }

    [HttpGet("view_qr/{id}")]
    public async Task<ActionResult<QrCodeClass>> GetById(string id)
    {
        var filter = Builders<QrCodeClass>.Filter.Eq(x => x.ReqVisitId, id);
        var item = await _items.Find(filter).FirstOrDefaultAsync();

        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateQr([FromBody] QrCodeClass item)
    {
        var filter = Builders<QrCodeClass>.Filter.Eq(x => x.ReqVisitId, item.ReqVisitId);
        var update = Builders<QrCodeClass>.Update
            .Set(x => x.Firstname, item.Firstname)
            .Set(x => x.Middlename, item.Middlename)
            .Set(x => x.Lastname, item.Lastname)
            .Set(x => x.Email, item.Email)
            .Set(x => x.MobileNumber, item.MobileNumber)
            .Set(x => x.project_type, item.project_type)
            .Set(x => x.crops, item.crops);
        // Add other fields as needed

        var result = await _items.UpdateOneAsync(filter, update);

        if (result.ModifiedCount == 0)
            return NotFound("QR record not found or no changes made.");

        return Ok("Updated successfully");
    }

    [HttpPost("verify_qr")]
    public async Task<ActionResult<bool>> VerifyQr([FromBody] QrCodeDto dto)
    {
        if (dto == null || string.IsNullOrEmpty(dto.RequestId) ||
            string.IsNullOrEmpty(dto.FarmId) || string.IsNullOrEmpty(dto.ClientId))
        {
            return BadRequest("Invalid QR data.");
        }

        var filter = Builders<QrCodeClass>.Filter.Eq(x => x.ReqVisitId, dto.RequestId) &
                     Builders<QrCodeClass>.Filter.Eq(x => x.FarmId, dto.FarmId) &
                     Builders<QrCodeClass>.Filter.Eq(x => x.ClientId, dto.ClientId);

        var record = await _items.Find(filter).FirstOrDefaultAsync();
        if (record == null)
            return NotFound("No matching record found.");

        string Normalize(string s) => s?.Trim().Replace("  ", " ").ToUpper() ?? string.Empty;

        string dbName = Normalize($"{record.Firstname} {record.Middlename} {record.Lastname}");
        string qrName = Normalize(dto.NameOf);

        string dbAddress = Normalize($"{record.Barangay}, {record.CityMunicipality}, {record.Province}, {record.Region}");
        string qrAddress = Normalize(dto.Address);

        bool isMatch = dbName == qrName &&
                       dbAddress == qrAddress &&
                       record.MobileNumber == dto.MobileNumber &&
                       record.num_hole == dto.NumHole;

        return Ok(isMatch);
    }

    [HttpPost("transfer_to_req_table")]
    public async Task<IActionResult> TransferToReqTable([FromBody] QrCodeDto dto)
    {
        if (dto == null || string.IsNullOrEmpty(dto.RequestId))
            return BadRequest("Invalid QR data.");

        var filter = Builders<QrCodeClass>.Filter.Eq(x => x.ReqVisitId, dto.RequestId);
        var record = await _items.Find(filter).FirstOrDefaultAsync();

        if (record == null)
            return NotFound("Original QR record not found.");

        // Map QrCodeClass -> QrTableClass
        var newRecord = new QrTableClass
        {
            NameOf = $"{record.Firstname?.Trim()} {record.Middlename?.Trim()} {record.Lastname?.Trim()}",
            ClientId = record.ClientId,
            FarmId = record.FarmId,
            created_by_id = record.created_by_id,
            num_hole = record.num_hole,
            UnitId = record.UnitId,
            Unit = record.Unit,
            LotSize = record.LotSize,
            date = record.date,
            time = record.time,
            soil_source = record.soil_source,
            project_type = record.project_type,
            crops = record.crops,
            Coordinates = record.Coordinates,
            Status = record.Status,
            ReqVisitId = record.ReqVisitId,
            CurrentPlant = record.CurrentPlant,
            PrevPlant = record.PrevPlant,
            NextPlant = record.NextPlant,
            CurrentPlantId = record.CurrentPlantId,
            PrevPlantId = record.PrevPlantId,
            NextPlantId = record.NextPlantId,
            Firstname = record.Firstname,
            Middlename = record.Middlename,
            Lastname = record.Lastname,
            Email = record.Email,
            MobileNumber = record.MobileNumber,
            Region = record.Region,
            Province = record.Province,
            CityMunicipality = record.CityMunicipality,
            Barangay = record.Barangay,
            Tracking = ""
        };

        var targetCollection = _database.GetCollection<QrTableClass>("req_table_view");
        await targetCollection.InsertOneAsync(newRecord);

        return Ok("Record transferred successfully.");
    }
}
