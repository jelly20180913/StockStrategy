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
    public class StockYestodayController : ApiController
    {
        private IStockService _StockService;
        public StockYestodayController(IStockService StockService)
        {
            this._StockService = StockService;
        }
        public List<Stock> Get(string date)
        {
            IEnumerable<Stock> _StockList;
            _StockList = this._StockService.GetAll().Where(x => x.Date== date).ToList();
            return _StockList.ToList();
        }
    }
}
