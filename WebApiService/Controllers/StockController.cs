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
    public class StockController : ApiController
    {
        private IStockStrategyService _StockStrategyService;
        public StockController(IStockStrategyService StockStrategyService)
        {
            this._StockStrategyService = StockStrategyService;
        }
		/// <summary>
		/// 改寫下Sql的語句,避免Sql injection
		/// </summary>
		/// <param name="parameter"></param>
		/// <param name="mode"></param>
		/// <returns></returns>
        public List<DataModel.Stock.Stock> Get(string parameter,string mode)
        { 
			return this._StockStrategyService.GetStockList(  parameter,    mode); 
        }
        public bool Post(List<Stock> s)
        {
			return this._StockStrategyService.InsertStock(s); 
        }
        public bool Put(Stock s)
        {
			return this._StockStrategyService.UpdateStock(s); 
        }
    }
}
