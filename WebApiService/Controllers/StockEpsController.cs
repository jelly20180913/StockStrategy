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
    public class StockEpsController : ApiController
    {
        private IStockStrategyService _StockStrategyService;
        public StockEpsController(IStockStrategyService StockStrategyService)
        {
            this._StockStrategyService = StockStrategyService;
        }
        public bool Post(List<StockEps> s)
        {
			return this._StockStrategyService.InsertStockEps(s); 
        }
		//public List<StockEps> Get()
		//{ 
		//	return this._StockStrategyService.GetStockEpsList();
		//}
		public bool Put(StockEps s)
		{
			//this._StockStrategyService.UpdateStockEps(s);
			return true;
		}
		public bool Delete(int id)
		{
			//this._StockStrategyService.DeleteStockEps(id);
			return true;
		}
	}
}
