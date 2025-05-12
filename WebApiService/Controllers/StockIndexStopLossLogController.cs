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
    public class StockIndexStopLossLogController : ApiController
    {
        private IStockIndexStopLossLogService _StockIndexStopLossLogService;    
        public StockIndexStopLossLogController(IStockIndexStopLossLogService StockIndexStopLossLogService)
        {
            this._StockIndexStopLossLogService = StockIndexStopLossLogService;
        }
		public List<StockIndexStopLossLog> Get()
		{
			;
			return this._StockIndexStopLossLogService.GetAll().ToList();
		}
		public string Post(StockIndexStopLossLog s)
		{
			return this._StockIndexStopLossLogService.Create(s);
		}
	}
}
