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
    public class StockHighLowController : ApiController
    {
        private IStockStrategyService _StockStrategyService;
        public StockHighLowController(IStockStrategyService StockStrategyService)
        {
            this._StockStrategyService = StockStrategyService;
        }
        public bool Post(List<StockHighLow> s)
        {
			return this._StockStrategyService.InsertStockHighLow(s); 
        }
		public List<StockHighLow> Get()
		{ 
			return this._StockStrategyService.GetStockHighLowList();
		}
		public bool Put(StockHighLow s)
		{
			//this._StockStrategyService.UpdateStockHighLow(s);
			return true;
		}
		public bool Delete(int id)
		{
			//this._StockStrategyService.DeleteStockHighLow(id);
			return true;
		}
	}
}
