using System.Net;
using MongoDB.Driver;
using MongoDB.Bson;
using PPDIS.Shared;
using PPDIS.Server.Services;
using PPDIS.Shared.Models;


namespace PPDIS.Server.Services;

public class MongoDBService : IMongoDBService
{
    

    private readonly IMongoCollection<StockCardData> _task;
    private IMongoDatabase db;

    public MongoDBService(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetConnectionString("MongoConnection"));
        db = client.GetDatabase("DataBase");
        
  
    }
    




    public async Task<string> InsertStock(StockCardData stock)
    {
        await db.GetCollection<StockCardData>("Stock").InsertOneAsync(stock);
        return stock.Id;
    }
    
    public async Task<bool> AddMultiplePoSuppliesAsync(string stockId, List<ListSuppliesGroupPo> newPoGroups)
    {
        var filter = Builders<StockCardData>.Filter.Eq(s => s.Id, stockId);
        var update = Builders<StockCardData>.Update.PushEach(s => s.list_supplies_po, newPoGroups);

        var result = await _task.UpdateOneAsync(filter, update);
        return result.ModifiedCount > 0;
    }
    
    public async Task<List<StockCardData>> GetStock()
    {
        return await db.GetCollection<StockCardData>("Stock").Find(q => true).ToListAsync();
    }
    
    public async Task<bool> EditParent(StockCardData stock)
    {
        var collection = db.GetCollection<StockCardData>("Stock");
        var filter = Builders<StockCardData>.Filter.Eq(t => t.Id, stock.Id);

        var result = await collection.ReplaceOneAsync(filter, stock, new ReplaceOptions { IsUpsert = false });

        return result.MatchedCount > 0 && result.ModifiedCount > 0;
    }


    
    public async Task<string> DeleteParent(string parentId, string descriptionToDelete)
    {
        var collection = db.GetCollection<StockCardData>("Stock");

        var parent = await collection.Find(s => s.Id == parentId).FirstOrDefaultAsync();

        if (parent == null)
            return "not found";

        // Filter list_supplies_pr
        parent.list_supplies_pr = parent.list_supplies_pr
            .Where(s => s.description != descriptionToDelete)
            .ToList();

        // Filter list_supplies_qu
        parent.list_supplies_qu = parent.list_supplies_qu
            .Where(s => s.description != descriptionToDelete)
            .ToList();

        // Filter list_supplies_po (nested Supplies)
        foreach (var group in parent.list_supplies_po)
        {
            group.Supplies = group.Supplies
                .Where(s => s.description != descriptionToDelete)
                .ToList();
        }

        // Filter list_supplies_ins (nested Ins)
        foreach (var group in parent.list_supplies_ins)
        {
            group.Ins = group.Ins
                .Where(s => s.description != descriptionToDelete)
                .ToList();
        }

        // Save the updated document
        var result = await collection.ReplaceOneAsync(
            s => s.Id == parentId,
            parent
        );

        return result.ModifiedCount > 0 ? "ok" : "not found";
    }


    
    
    public async Task<List<T>> getDataListFiltered<T>(string table, PipelineDefinition<T, T> Aggregate)
    {
        var collection = db.GetCollection<T>(table);
        var data = await collection.Aggregate(Aggregate).ToListAsync();
        return data;
    }

  
    
    
    public BsonValue InsertRecordAndReturnID(string collectionName, BsonDocument document)
    {
        var collection = db.GetCollection<BsonDocument>(collectionName);
        collection.InsertOne(document);
        return document["_id"];
    }

    public BsonValue UpsertRecordAndReturnID(string collectionName, BsonDocument filter, BsonDocument document)
    {
        var collection = db.GetCollection<BsonDocument>(collectionName);
        var updateOptions = new ReplaceOptions { IsUpsert = true };
        collection.ReplaceOne(filter, document, updateOptions);
        return document["_id"];
    }

}
