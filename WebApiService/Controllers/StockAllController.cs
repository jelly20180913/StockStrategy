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
    public class StockAllController : ApiController
    {
        private IStockService _StockService;
        public StockAllController(IStockService StockService)
        {
            this._StockService = StockService;
        }
        public List<Stock> Get( )
        {
            IEnumerable<Stock> _StockList;
            _StockList = this._StockService.GetAll().ToList();
            return _StockList.ToList();
        }
    }
}
