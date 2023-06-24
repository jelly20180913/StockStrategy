using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApiService.Filters.Authorization;
using WebApiService.Models;
using WebApiService.Services.Interface.Tables;
using System.Web.Http.Cors;
using WebApiService.Services.Interface;
using WebApiService.Services.Implement;

namespace WebApiService.Controllers
{
    [StockJwtAuthAction]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StockPickingController : ApiController
    {
        private IStockStrategyService _StockStrategyService;
        public StockPickingController(IStockStrategyService StockStrategyService)
        {
            this._StockStrategyService = StockStrategyService;
        }
        public bool Post(List<StockPicking> s)
        {
			return this._StockStrategyService.InsertStockPicking(s); 
        }
		public List<StockPicking> Get()
		{ 
			return this._StockStrategyService.GetStockPickingList();
		}
		public bool Put(StockPicking s)
		{
			this._StockStrategyService.UpdateStockPicking(s);
			return true;
		}
		public bool Delete(int id)
		{
			this._StockStrategyService.DeleteStockPicking(id);
			return true;
		}
	}
}
