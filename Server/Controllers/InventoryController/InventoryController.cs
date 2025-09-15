using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using PPDIS.Shared.Models; // PurchaseRequest model
using Soil.Shared.Models.InventoryModel; // PurchaseOrder and Inspection models
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Soil.Server.Controllers.InventoryController
{
    [ApiController]
    [Route("api/inventory")]
    public class InventoryController : ControllerBase
    {
        private readonly IMongoDatabase _db;

        public InventoryController(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoConnection"));
            _db = client.GetDatabase("DataBase");
        }

        // -------------------- Purchase Requests --------------------
        // GET: api/inventory/get_pr
        [HttpGet("get_pr")]
        public async Task<ActionResult<List<PurchaseRequest>>> GetAllPurchaseRequests()
        {
            var collection = _db.GetCollection<PurchaseRequest>("vw_pr");
            return await collection.Find(pr => true).ToListAsync();
        }
        
        
        [HttpGet("get_stock/{id}")]
        public async Task<ActionResult<StockCardData>> GetStockById(string id)
        {
            var collection = _db.GetCollection<StockCardData>("Stock");
            var stock = await collection.Find(s => s.Id == id).FirstOrDefaultAsync();
            if (stock == null) return NotFound();
            return stock;
        }
        
        // -------------------- Quotation --------------------
        // GET: api/inventory/get_po
        [HttpGet("get_qu")]
        public async Task<ActionResult<List<QuotationClass>>> GetAllQuotation()
        {
            var collection = _db.GetCollection<QuotationClass>("vw_qu");
            return await collection.Find(po => true).ToListAsync();
        }

        // -------------------- Purchase Orders --------------------
        // GET: api/inventory/get_po
        [HttpGet("get_po")]
        public async Task<ActionResult<List<PurchaseOrderClass>>> GetAllPurchaseOrders()
        {
            var collection = _db.GetCollection<PurchaseOrderClass>("vw_po");
            return await collection.Find(po => true).ToListAsync();
        }

        // -------------------- Inspections --------------------
        // GET: api/inventory/get_ins
        [HttpGet("get_ins")]
        public async Task<ActionResult<List<InspectionClass>>> GetAllInspections()
        {
            var collection = _db.GetCollection<InspectionClass>("vw_ins");
            return await collection.Find(ins => true).ToListAsync();
        }
    }
}