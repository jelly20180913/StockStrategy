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
    public class StockResultController : ApiController
    {
        private IStockStrategyService _StockStrategyService;
        public StockResultController(IStockStrategyService StockStrategyService)
        {
            this._StockStrategyService = StockStrategyService;
        }
        public bool Post(List<StockResult> s)
        {
			return this._StockStrategyService.InsertStockResult(s); 
        }
		public List<StockResult> Get()
		{
			return this._StockStrategyService.GetStockResultList();
		}
		public bool Delete(int id)
		{
			this._StockStrategyService.DeleteStockResult(id);
			return true;
		}
	}
}
