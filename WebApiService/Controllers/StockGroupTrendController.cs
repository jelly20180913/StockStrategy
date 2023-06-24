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
    public class StockGroupTrendController : ApiController
    {
        private IStockStrategyService _StockStrategyService;
        public StockGroupTrendController(IStockStrategyService StockStrategyService)
        {
            this._StockStrategyService = StockStrategyService;
        }
        public bool Post(List<StockGroupTrend> s)
        {
			return this._StockStrategyService.InsertStockGroupTrend(s); 
        }
		public List<StockGroupTrend> Get()
		{ 
			return this._StockStrategyService.GetStockGroupTrendList();
		}
		public bool Put(StockGroupTrend s)
		{
			//this._StockStrategyService.UpdateStockGroupTrend(s);
			return true;
		}
		public bool Delete(int id)
		{
			//this._StockStrategyService.DeleteStockGroupTrend(id);
			return true;
		}
	}
}
