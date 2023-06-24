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
    public class StockThreeInstitutionalController : ApiController
    {
        private IStockStrategyService _StockStrategyService;
        public StockThreeInstitutionalController(IStockStrategyService StockStrategyService)
        {
            this._StockStrategyService = StockStrategyService;
        }
        public bool Post(List<StockThreeInstitutional> s)
        {
			return this._StockStrategyService.InsertStockThreeInstitutional(s); 
        }
		public List<StockThreeInstitutional> Get()
		{ 
			return this._StockStrategyService.GetStockThreeInstitutionalList();
		}
		public bool Put(StockThreeInstitutional s)
		{
			//this._StockStrategyService.UpdateStockThreeInstitutional(s);
			return true;
		}
		public bool Delete(int id)
		{
			//this._StockStrategyService.DeleteStockThreeInstitutional(id);
			return true;
		}
	}
}
