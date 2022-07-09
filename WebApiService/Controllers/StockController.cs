using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApiService.Filters.Authorization;
using WebApiService.Models;
using WebApiService.Services.Interface.Tables;
using System.Web.Http.Cors;
namespace WebApiService.Controllers
{
    [StockJwtAuthAction]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StockController : ApiController
    {
        private IStockService _StockService;
        public StockController(IStockService StockService)
        {
            this._StockService = StockService;
        }
        public List<Stock> Get(string code)
        {
            IEnumerable<Stock> _StockList;
            _StockList = this._StockService.GetAll().Where(x=>x.Code==code).ToList();
            return _StockList.ToList();
        }
        public bool Post(List<Stock> s)
        {
            this._StockService.MiltiCreate(s);
            return true;
        }
        public bool Put(Stock s)
        {
            this._StockService.Update(s);
            return true;
        }
    }
}
