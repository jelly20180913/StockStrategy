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
    public class HolidayController : ApiController
    {
        private IStockStrategyService _StockStrategyService;    
        public HolidayController(IStockStrategyService StockStrategyService)
        {
            this._StockStrategyService = StockStrategyService;
        } 
        public List<Holiday> Get()
        {
            // List<Holiday> _Holiday = new List<Holiday>();
            //   return _Holiday;
            return this._StockStrategyService.GetHolidayList(); 
		}
    }
}
