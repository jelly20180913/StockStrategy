using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApiService.Filters.Authorization;
using WebApiService.Models;
using WebApiService.Services.Interface;

namespace WebApiService.Controllers
{
	[StockJwtAuthAction]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StockRevenueController : ApiController
    {
        private IStockStrategyService _StockStrategyService;
        public StockRevenueController(IStockStrategyService StockStrategyService)
        {
            this._StockStrategyService = StockStrategyService;
        }
        public bool Post(List<StockRevenue> s)
        {
			return this._StockStrategyService.InsertStockRevenue(s); 
        }
		//public List<StockRevenue> Get()
		//{ 
		//	return this._StockStrategyService.GetStockRevenueList();
		//}
		public bool Put(StockRevenue s)
		{
			//this._StockStrategyService.UpdateStockRevenue(s);
			return true;
		}
		public bool Delete(int id)
		{
			//this._StockStrategyService.DeleteStockRevenue(id);
			return true;
		}
	}
}
