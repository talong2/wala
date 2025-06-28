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
        
        
        /*var clientOnline = new MongoClient(configuration.GetConnectionString("MongoConnectionOnline"));
        db = clientOnline.GetDatabase("IGIS");*/
    }
    




    public async Task<string> InsertStock(StockCardData stock)
    {
         db.GetCollection<StockCardData>("Stock").InsertOne(stock);
        return stock.Id;
    }
    
    
    public async Task<List<StockCardData>> GetStock()
    {
        return await db.GetCollection<StockCardData>("Stock").Find(q => true).ToListAsync();
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
