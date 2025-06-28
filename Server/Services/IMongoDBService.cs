using MongoDB.Bson;
using MongoDB.Driver;
using PPDIS.Shared;
using PPDIS.Shared.Models;


namespace PPDIS.Server.Services;

public interface IMongoDBService
{
    Task<string> InsertClient(Taskclass task);    
    Task<string> InsertStock(StockCardData stock);  
    Task<List<Taskclass>>GetClients();
    Task<List<MunicipalityClass>> GetMunicipality();
    Task<List<ChartClass>> GetChart();
    Task<string> DeleteTask(string id);
    Task<string> UpdateClient(Taskclass task);
    
    
    Task<List<PovertyClass>>GetProverty();
    
    Task<List<Examinee>>GetExam();
    Task<List<StockCardData>>GetStock();
    
    Task<List<T>> getDataListFiltered<T>(string table, PipelineDefinition<T, T> Aggregate);



}