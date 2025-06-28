using System.Net;
using MongoDB.Driver;
using MongoDB.Bson;
using PPDIS.Shared;
using PPDIS.Server.Services;
using PPDIS.Shared.Models;


namespace PPDIS.Server.Services;

public class MongoDBService : IMongoDBService
{
    

    private readonly IMongoCollection<Taskclass> _task;
    private IMongoDatabase db;

    public MongoDBService(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetConnectionString("MongoConnection"));
        db = client.GetDatabase("DataBase");
        
        
        /*var clientOnline = new MongoClient(configuration.GetConnectionString("MongoConnectionOnline"));
        db = clientOnline.GetDatabase("IGIS");*/
    }
    



    public async Task<string> InsertClient(Taskclass task)
    {
        await db.GetCollection<Taskclass>("PPDIS").InsertOneAsync(task);
        return "ok";
    }
    
    public async Task<string> InsertStock(StockCardData stock)
    {
         db.GetCollection<StockCardData>("Stock").InsertOne(stock);
        return stock.Id;
    }
    
    public async Task<List<Taskclass>> GetClients()
    {
        return await db.GetCollection<Taskclass>("PPDIS").Find(q => true).ToListAsync();
    }
    
    
    public async Task<List<PovertyClass>> GetProverty()
    {
        return await db.GetCollection<PovertyClass>("Local Poverty Intensity Index").Find(q => true).ToListAsync();
    }

    public async Task<List<Examinee>> GetExam()
    {
        return await db.GetCollection<Examinee>("Exam").Find(q => true).ToListAsync();
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

    public async Task<List<MunicipalityClass>> GetMunicipality()
    {
        return await db.GetCollection<MunicipalityClass>("Data Dictionary.LPII Municipality").Find(q => true).ToListAsync();
    }

    
    public async Task<List<ChartClass>> GetChart()
    {
        return await db.GetCollection<ChartClass>("LPII Chart").Find(q => true).ToListAsync();
    }


    public async Task<Taskclass> CreateAsync(Taskclass task)
    {
        await _task.InsertOneAsync(task);
        return task;
    }

   
    
    public async Task<string> DeleteTask(string id)
    {
    
        var filter = Builders<Taskclass>.Filter.Eq(c => c.id, id);
        var result = await db.GetCollection<Taskclass>("PPDIS").DeleteOneAsync(filter);
    
        return result.DeletedCount > 0 ? "ok" : "not found";
    }
    
    //
    //
    //
    // public async Task<object> UpdateTaskAsync(string id, Taskclass updatedTask)
    // {
    //     var filter = Builders<Taskclass>.Filter.Eq(c => c.id, id); // Assuming 'Id' is the property name
    //     var update = Builders<Taskclass>.Update
    //         .Set(c => c.SomeProperty, updatedTask.SomeProperty) // Replace with actual property updates
    //         .Set(c => c.AnotherProperty, updatedTask.AnotherProperty); // Replace with actual property updates
    //
    //     var collection = db.GetCollection<Taskclass>("Task");
    //     var result = await collection.UpdateOneAsync(filter, update);
    //
    //     return updatedTask;
    // }
    public async Task<string> UpdateClient(Taskclass task)
    {
        try
        {
            var collection = db.GetCollection<Taskclass>("PPDIS");

            // Assuming `task.Id` is the unique identifier for the task
            var filter = Builders<Taskclass>.Filter.Eq(t => t.id, task.id);

            // Replace the document if it exists, otherwise insert a new one
            var result = await collection.ReplaceOneAsync(filter, task, new ReplaceOptions { IsUpsert = true });

            return "ok";
        }
        catch (Exception ex)
        {
            // Log or handle the exception as needed
            return $"Error: {ex.Message}";
        }
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
