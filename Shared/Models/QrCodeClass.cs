using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Soil.Shared.Models;

public class QrCodeClass
{
    public string? NameOf;

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string? Id { get; set; }

    [BsonElement("client_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ClientId { get; set; }

    [BsonElement("farm_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? FarmId { get; set; }
    
    [BsonElement("created_by_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? created_by_id { get; set; }

    [BsonElement("num_hole")]
    public int num_hole { get; set; }

    [BsonElement("unit_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? UnitId { get; set; }

    [BsonElement("unit")]
    public string? Unit { get; set; }

    [BsonElement("lot_size")]
    public decimal LotSize { get; set; }

    [BsonElement("date")]
    public DateTime date { get; set; }

    [BsonElement("time")]
    public DateTime time { get; set; }
    
    [BsonElement("soil_source")]
    public string? soil_source { get; set; } = string.Empty;
    
    [BsonElement("project_type")]
    public string? project_type { get; set; } = string.Empty;
    
    [BsonElement("crops")]
    public string? crops { get; set; } = string.Empty;

    [BsonElement("coordinates")]
    public List<double[]> Coordinates { get; set; } = new();

    [BsonElement("status")]
    public string? Status { get; set; }

    [BsonElement("req_visit_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ReqVisitId { get; set; }

    [BsonElement("current_plant")]
    public string? CurrentPlant { get; set; }

    [BsonElement("prev_plant")]
    public string? PrevPlant { get; set; }

    [BsonElement("next_plant")]
    public string? NextPlant { get; set; }

    [BsonElement("current_plant_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? CurrentPlantId { get; set; }

    [BsonElement("prev_plant_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? PrevPlantId { get; set; }

    [BsonElement("next_plant_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? NextPlantId { get; set; }

    [BsonElement("firstname")]
    public string? Firstname { get; set; }

    [BsonElement("middlename")]
    public string? Middlename { get; set; }

    [BsonElement("lastname")]
    public string? Lastname { get; set; }

    [BsonElement("email")]
    public string? Email { get; set; }

    [BsonElement("mobile_number")]
    public string? MobileNumber { get; set; }

    [BsonElement("region")]
    public string? Region { get; set; }

    [BsonElement("province")]
    public string? Province { get; set; }

    [BsonElement("city_municipality")]
    public string? CityMunicipality { get; set; }

    [BsonElement("barangay")]
    public string? Barangay { get; set; }
}



public class QrCodeDto
{
    [JsonPropertyName("requestid")]
    public string RequestId { get; set; }

    [JsonPropertyName("farm_id")]
    public string FarmId { get; set; }

    [JsonPropertyName("client_id")]
    public string ClientId { get; set; }

    [JsonPropertyName("nameof")]
    public string NameOf { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; }

    [JsonPropertyName("mobile_number")]
    public string MobileNumber { get; set; }

    [JsonPropertyName("num_hole")]
    public int NumHole { get; set; }
}




public class QrTableClass
{
    public string? NameOf;

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("_id")]
    public string? Id { get; set; }

    [BsonElement("client_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ClientId { get; set; }

    [BsonElement("farm_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? FarmId { get; set; }
    
    [BsonElement("created_by_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? created_by_id { get; set; }

    [BsonElement("num_hole")]
    public int num_hole { get; set; }

    [BsonElement("unit_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? UnitId { get; set; }

    [BsonElement("unit")]
    public string? Unit { get; set; }

    [BsonElement("lot_size")]
    public decimal LotSize { get; set; }

    [BsonElement("date")]
    public DateTime date { get; set; }

    [BsonElement("time")]
    public DateTime time { get; set; }
    
    [BsonElement("soil_source")]
    public string? soil_source { get; set; } = string.Empty;
    
    [BsonElement("project_type")]
    public string? project_type { get; set; } = string.Empty;
    
    [BsonElement("crops")]
    public string? crops { get; set; } = string.Empty;

    [BsonElement("coordinates")]
    public List<double[]> Coordinates { get; set; } = new();

    [BsonElement("status")]
    public string? Status { get; set; }

    [BsonElement("req_visit_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? ReqVisitId { get; set; }

    [BsonElement("current_plant")]
    public string? CurrentPlant { get; set; }

    [BsonElement("prev_plant")]
    public string? PrevPlant { get; set; }

    [BsonElement("next_plant")]
    public string? NextPlant { get; set; }

    [BsonElement("current_plant_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? CurrentPlantId { get; set; }

    [BsonElement("prev_plant_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? PrevPlantId { get; set; }

    [BsonElement("next_plant_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? NextPlantId { get; set; }

    [BsonElement("firstname")]
    public string? Firstname { get; set; }

    [BsonElement("middlename")]
    public string? Middlename { get; set; }

    [BsonElement("lastname")]
    public string? Lastname { get; set; }

    [BsonElement("email")]
    public string? Email { get; set; }

    [BsonElement("mobile_number")]
    public string? MobileNumber { get; set; }

    [BsonElement("region")]
    public string? Region { get; set; }

    [BsonElement("province")]
    public string? Province { get; set; }

    [BsonElement("city_municipality")]
    public string? CityMunicipality { get; set; }

    [BsonElement("barangay")]
    public string? Barangay { get; set; }
    
    [BsonElement("tracking")]
    public string? Tracking { get; set; }
}
