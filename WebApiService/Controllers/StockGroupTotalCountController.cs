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
    public class StockGroupTotalCountController : ApiController
    {
        private IStockGroupTotalCountService _StockGroupTotalCountService;
        public StockGroupTotalCountController(IStockGroupTotalCountService StockGroupTotalCountService)
        {
            this._StockGroupTotalCountService = StockGroupTotalCountService;
        }
        public List<StockGroupTotalCount> Get()
        {
            IEnumerable<StockGroupTotalCount> _StockGroupTotalCountList;
            _StockGroupTotalCountList = this._StockGroupTotalCountService.GetAll();
            return _StockGroupTotalCountList.ToList();
        }
        public bool Post(List<StockGroupTotalCount> s)
        {
            this._StockGroupTotalCountService.MiltiCreate(s);
            return true;
        }
    }
}
