using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApiService.Filters.Authorization;
using WebApiService.Models;
using WebApiService.Services.Interface.Tables;
using System.Web.Http.Cors;
namespace WebApiService.Controllers
{
    [StockJwtAuthAction]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StockInventoryController : ApiController
    {
        private IStockInventoryService _StockInventoryService;
        public StockInventoryController(IStockInventoryService StockInventoryService)
        {
            this._StockInventoryService = StockInventoryService;
        }
        public List<StockInventory> Get()
        {
            IEnumerable<StockInventory> _StockInventoryList;
            _StockInventoryList = this._StockInventoryService.GetAll();
            return _StockInventoryList.ToList();
        }
        public bool Post( StockInventory  s)
        {
            this._StockInventoryService.Create(s);
            return true;
        }
        public bool Put(StockInventory s)
        {
            this._StockInventoryService.Update(s);
            return true;
        }
    }
}
