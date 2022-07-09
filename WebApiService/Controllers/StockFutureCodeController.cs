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
    public class StockFutureCodeController : ApiController
    {
        private IStockFutureCodeService _StockFutureCodeService;
        public StockFutureCodeController(IStockFutureCodeService StockFutureCodeService)
        {
            this._StockFutureCodeService = StockFutureCodeService;
        }
        public List<StockFutureCode> Get()
        {
            IEnumerable<StockFutureCode> _StockFutureCodeList;
            _StockFutureCodeList = this._StockFutureCodeService.GetAll().ToList();
            return _StockFutureCodeList.ToList();
        }
    }
}
