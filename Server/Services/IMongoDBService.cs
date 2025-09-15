using MongoDB.Bson;
using MongoDB.Driver;
using PPDIS.Shared;
using PPDIS.Shared.Models;


namespace PPDIS.Server.Services;

public interface IMongoDBService
{
   
    Task<string> InsertStock(StockCardData stock);
    Task<bool> AddMultiplePoSuppliesAsync(string stockId, List<ListSuppliesGroupPo> newPoGroups);
 
    Task<List<StockCardData>>GetStock();
    
    Task<bool> EditParent(StockCardData stock);


    Task<string> DeleteParent(string parentId, string descriptionToDelete);
    
    Task<List<T>> getDataListFiltered<T>(string table, PipelineDefinition<T, T> Aggregate);



}