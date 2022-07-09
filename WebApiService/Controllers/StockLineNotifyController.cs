using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiService.Filters.Authorization;
using WebApiService.Models;
using WebApiService.Services.Interface.Tables; 
namespace WebApiService.Controllers
{
    [StockJwtAuthAction]
    public class StockLineNotifyController : ApiController
    {
        private IStockLineNotifyService _StockLineNotifyService;
        public StockLineNotifyController(IStockLineNotifyService StockLineNotifyService)
        {
            this._StockLineNotifyService = StockLineNotifyService;
        }
        public bool Post(List<StockLineNotify> s)
        {
            this._StockLineNotifyService.MiltiCreate(s);
            return true;
        }
        public List<StockLineNotify> Get()
        {
            IEnumerable<StockLineNotify> _StockLineNotifyList;
            _StockLineNotifyList = this._StockLineNotifyService.GetAll();
            return _StockLineNotifyList.ToList();
        }
        public bool Put(StockLineNotify s)
        {
            this._StockLineNotifyService.Update(s);
            return true;
        }
    }
}
