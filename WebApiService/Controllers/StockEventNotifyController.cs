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
    public class StockEventNotifyController : ApiController
    {
        private IStockStrategyService _StockStrategyService;
        public StockEventNotifyController(IStockStrategyService StockStrategyService)
        {
            this._StockStrategyService = StockStrategyService;
        }
        public bool Post(List<StockEventNotify> s)
        {
			return this._StockStrategyService.InsertStockEventNotify(s); 
        }
		public List<StockEventNotify> Get()
		{ 
			return this._StockStrategyService.GetStockEventNotifyList();
		}
		public bool Put(StockEventNotify s)
		{
			//this._StockStrategyService.UpdateStockEventNotify(s);
			return true;
		}
		public bool Delete(int id)
		{
			//this._StockStrategyService.DeleteStockEventNotify(id);
			return true;
		}
	}
}
