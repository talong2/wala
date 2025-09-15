using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Soil.Shared.Models.InventoryModel;

public class InspectionClass
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("ins_id")]
    public ObjectId InspectionId { get; set; }

    [BsonElement("ins_detail_id")]
    public ObjectId InspectionDetailId { get; set; }
    
    [BsonElement("tracking")]
    public string TrackingNo { get; set; } = string.Empty;

    [BsonElement("InspectionNumber")]
    public string InspectionNumber { get; set; } = string.Empty;

    [BsonElement("InspectionDate")]
    public string InspectionDate { get; set; } = string.Empty;
    // ❗ If this should be a real date, consider switching to DateTime.

    [BsonElement("unit")]
    public string Unit { get; set; } = string.Empty;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("quantity")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Quantity { get; set; }

    [BsonElement("unitcost")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal UnitCost { get; set; }

    [BsonElement("totalcost")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal TotalCost { get; set; }
}