using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PPDIS.Shared.Models;

public class StockClass
{

    
    public string date { get; set; } = String.Empty;
    public string reference { get; set; } = String.Empty;
    public string receiptQty { get; set; } = String.Empty;
    public string issueQty { get; set; } = String.Empty;
    public string office { get; set; } = String.Empty;
    public string balanceQty { get; set; } = String.Empty;
    public string daysToConsume { get; set; } = String.Empty;
}

public class StockCardData
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = String.Empty;
    
    public string month { get; set; } = String.Empty;
    
    public string year { get; set; } = String.Empty;
    
    public List<StockClass> entries { get; set; }
}