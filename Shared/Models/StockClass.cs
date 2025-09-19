
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;


namespace PPDIS.Shared.Models;
    [BsonIgnoreExtraElements]
public class StockCardData
{

    
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string tracking { get; set; } = string.Empty;

    
    public DateTime date { get; set; } = DateTime.Now;

    public string item { get; set; } = string.Empty;
    public string descript { get; set; } = string.Empty;
    public string unit { get; set; } = string.Empty;
    public string stock { get; set; } = string.Empty;
    public string reorder { get; set; } = string.Empty;

    public string pr_number { get; set; } = string.Empty;
    
   
    public string pr_date { get; set; } = string.Empty;
    public string canvass_no { get; set; } = string.Empty;



    public List<StockClass> entries { get; set; } = new();

    public List<PrSuppliesClass> list_supplies_pr { get; set; } = new(); // ✅ grouped with _id and items

    public List<ListSuppliesGroupPo> list_supplies_po { get; set; } = new();
    public List<QuSuppliesClass> list_supplies_qu { get; set; } = new();
    public List<ListSuppliesGroupIns> list_supplies_ins { get; set; } = new();
}

// ✅ Represents each grouped supply entry in list_supplies
public class ListSuppliesGroupPo
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    
    public string Supplier { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string TIN { get; set; } = string.Empty;
    public string PONumber { get; set; } = string.Empty;
    public string PODate { get; set; } = string.Empty;
    public string ProcurementMode { get; set; } = string.Empty;
    
    

    public List<SuppliesClass> Supplies { get; set; } = new();
}

public class ListSuppliesGroupIns
{
    [BsonElement("_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    
    public string InspectionNumber { get; set; } = string.Empty;
    public string InspectionDate { get; set; } = string.Empty;

    public List<InsSuppliesClass> Ins { get; set; } = new();
}

public class SuppliesClass
{
    public string unit { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
   public decimal? quantity { get; set; }
    public decimal? unitcost { get; set; }
    public decimal? totalcost { get; set; }
}

public class PrSuppliesClass
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } = ObjectId.GenerateNewId().ToString();
    public string? unit { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
   public decimal? quantity { get; set; }
    public decimal? unitcost { get; set; }
    public decimal? totalcost { get; set; }
}

public class QuSuppliesClass
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string unit { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
   public decimal? quantity { get; set; }
    public decimal? unitcost { get; set; }
    public decimal? totalcost { get; set; }
}

public class InsSuppliesClass
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string unit { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
   public decimal? quantity { get; set; }
    public decimal? unitcost { get; set; }
    public decimal? totalcost { get; set; }
}

public class StockClass
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string date { get; set; } = string.Empty;
    public string reference { get; set; } = string.Empty;
    public string receiptQty { get; set; } = string.Empty;
    public string issueQty { get; set; } = string.Empty;
    public string office { get; set; } = string.Empty;
    public string balanceQty { get; set; } = string.Empty;
    public string daysToConsume { get; set; } = string.Empty;
    
    

}




