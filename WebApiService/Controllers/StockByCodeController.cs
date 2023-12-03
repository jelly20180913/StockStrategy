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
    public class StockByCodeController : ApiController
    {
        private IStockStrategyService _StockStrategyService;
        public StockByCodeController(IStockStrategyService StockStrategyService)
        {
            this._StockStrategyService = StockStrategyService;
        } 
        public List<Stock> Get(string parameter, string mode)
		{ 
			return this._StockStrategyService.GetByStockCodeList(  parameter,    mode); 
        } 
    }
}
