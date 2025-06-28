using MongoDB.Bson;
using MongoDB.Driver;
using PPDIS.Shared;
using PPDIS.Shared.Models;


namespace PPDIS.Server.Services;

public interface IMongoDBService
{
   
    Task<string> InsertStock(StockCardData stock);  
 
    Task<List<StockCardData>>GetStock();
    
    Task<List<T>> getDataListFiltered<T>(string table, PipelineDefinition<T, T> Aggregate);



}