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
    public class StockIndexController : ApiController
    {
        private IStockIndexService _StockIndexService;
        public StockIndexController(IStockIndexService StockIndexService)
        {
            this._StockIndexService = StockIndexService;
        }
        public bool Post(List<StockIndex> s)
        {
            this._StockIndexService.MiltiCreate(s);
            return true;
        }
        public List<StockIndex> Get()
        {
            IEnumerable<StockIndex> _StockIndexList;
            _StockIndexList = this._StockIndexService.GetAll();
            return _StockIndexList.ToList();
        }
        public bool Put(StockIndex s)
        {
            this._StockIndexService.Update(s);
            return true;
        }
    }
}
