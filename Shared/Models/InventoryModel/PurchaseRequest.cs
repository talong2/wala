using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PPDIS.Shared.Models;

public class PurchaseRequest
{
    // MongoDB primary key
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    // secondary objectId field (business ID)
    [BsonElement("pr_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string PrId { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("tracking")]
    public string TrackingNo { get; set; } = string.Empty;

    [BsonElement("pr_number")]
    public string PRNumber { get; set; } = string.Empty;

    [BsonElement("pr_date")]
    public string PRDate { get; set; } = string.Empty;

    [BsonElement("unit")]
    public string Unit { get; set; } = string.Empty;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("quantity")]
    public decimal? Quantity { get; set; } 

    [BsonElement("unitcost")]
    public decimal? UnitCost { get; set; } 
    [BsonElement("totalcost")]
    public decimal? TotalCost { get; set; } 
}
