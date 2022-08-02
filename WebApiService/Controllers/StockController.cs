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
        public List<Stock> Get(string parameter,string mode)
        {
            string _Sql = "SELECT   [Id] " +
                           "     ,[Code]                           " +
                           "     ,[Name]                           " +
                           "     ,[TradeVolume]                    " +
                           "     ,[TradeValue]                     " +
                           "     ,[OpeningPrice]                   " +
                           "     ,[HighestPrice]                   " +
                           "     ,[LowestPrice]                    " +
                           "     ,[ClosingPrice]                   " +
                           "     ,[Change]                         " +
                           "     ,[TransactionValue]               " +
                           "     ,[Date]                           " +
                           "     ,[UpdateTime]                     " +
                           "     ,[Gain]                           " +
                           "     ,[Shock]                          " +
                           "     ,[ForeignInvestment]                " +
                           "     ,[Investment]                     " +
                           "     ,[Dealer]                          " +
                           " FROM[StockWarehouse].[dbo].[Stock] ";
                  if(mode=="Code")        _Sql += " where Code='" + parameter + "'";
                  else if(mode=="Date") _Sql += " where Date='" + parameter + "'";
            IEnumerable<Stock> _StockList;
            _StockList = this._StockService.Filter(_Sql);
           // _StockList = this._StockService.GetAll().Where(x=>x.Code==code).ToList();
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
