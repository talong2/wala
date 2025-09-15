using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Soil.Shared.Models.InventoryModel;

public class PurchaseOrderClass
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("po_id")]
    public ObjectId POId { get; set; }
    
    [BsonElement("tracking")]
    public string TrackingNo { get; set; } = string.Empty;

    [BsonElement("Supplier")]
    public string Supplier { get; set; } = string.Empty;

    [BsonElement("Address")]
    public string Address { get; set; } = string.Empty;

    [BsonElement("TIN")]
    public string TIN { get; set; } = string.Empty;

    [BsonElement("PONumber")]
    public string PONumber { get; set; } = string.Empty;

    [BsonElement("PODate")]
    public string PODate { get; set; } = string.Empty; 
    // ❗ If this is a real date, consider changing to DateTime.

    [BsonElement("ProcurementMode")]
    public string ProcurementMode { get; set; } = string.Empty;

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