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
    public class StockGroupController : ApiController
    {
        private IStockGroupService _StockGroupService;
        public StockGroupController(IStockGroupService StockGroupService)
        {
            this._StockGroupService = StockGroupService;
        }
        public List<StockGroup> Get()
        {
            IEnumerable<StockGroup> _StockGroupList;
            _StockGroupList = this._StockGroupService.GetAll();
            return _StockGroupList.ToList();
        }
        public bool Post(List<StockGroup> s)
        {
            this._StockGroupService.MiltiCreate(s);
            return true;
        }
    }
}
