using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Soil.Shared.Models.InventoryModel;

[BsonIgnoreExtraElements]
public class QuotationClass
{
    [BsonId]
    public ObjectId Id { get; set; }
    
    [BsonElement("tracking")]
    public string TrackingNo { get; set; } = string.Empty;

    [BsonElement("pr_number")]
    public string PRNumber { get; set; } = string.Empty;

    [BsonElement("pr_date")]
    public DateTime PRDate { get; set; }

    [BsonElement("canvass_no")]
    public string CanvassNo { get; set; } = string.Empty;

    [BsonElement("unit")]
    public string Unit { get; set; } = string.Empty;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("quantity")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Quantity { get; set; }

    [BsonElement("unitcost")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal? UnitCost { get; set; }

    [BsonElement("totalcost")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal? TotalCost { get; set; }
}
