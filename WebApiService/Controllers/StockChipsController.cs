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
    public class StockChipsController : ApiController
    {
        private IStockStrategyService _StockStrategyService;
        public StockChipsController(IStockStrategyService StockStrategyService)
        {
            this._StockStrategyService = StockStrategyService;
        }
        public bool Post(List<StockChips> s)
        {
			return this._StockStrategyService.InsertStockChips(s); 
        }
		//public List<StockChips> Get()
		//{ 
		//	return this._StockStrategyService.GetStockChipsList();
		//}
		public bool Put(StockChips s)
		{
			//this._StockStrategyService.UpdateStockChips(s);
			return true;
		}
		public bool Delete(int id)
		{
			//this._StockStrategyService.DeleteStockChips(id);
			return true;
		}
	}
}
