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
    public class StockIndexForcastController : ApiController
    {
        private IStockIndexForcastService _StockIndexForcastService;    
        public StockIndexForcastController(IStockIndexForcastService StockIndexForcastService)
        {
            this._StockIndexForcastService = StockIndexForcastService;
        } 
        public List<StockIndexForcast> Get()
        { ;
            return this._StockIndexForcastService.GetAll().ToList(); 
		}
		public string Post( StockIndexForcast  s)
		{
			return this._StockIndexForcastService.Create(s);
		}
	}
}
