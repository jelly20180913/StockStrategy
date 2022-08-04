using System;
using System.Collections.Generic; 
using System.Threading.Tasks;
using System.Configuration; 
using Newtonsoft.Json;
using WebApiService.Models;
using DataModel;
using DataModel.Login;
using StockStrategy.Common;
namespace StockStrategy.BBL
{
   public class DataAccess
    {
        string ConnectionString;
        string Token;
        public DataAccess()
        {
            var _Ip =Tool.GetIpAddresses(); 
            if (_Ip.Length > 0)
            {
                if (_Ip[_Ip.Length - 1].ToString() == "114.32.117.3")
                     ConnectionString = ConfigurationManager.AppSettings["ApiServer2"];
                else
                     ConnectionString = ConfigurationManager.AppSettings["ApiServer"];
            } 
            loginWebApi();
        }
        public string  loginWebApi()
        {
            //Login
            string _Username = Tool.Decrypt(ConfigurationManager.AppSettings["Account"], "20220801", "B1050520");
            string _Password = Tool.Decrypt(ConfigurationManager.AppSettings["Password"], "20220801", "B1050520");
            LoginData _LoginData = new LoginData();
            _LoginData.Username = _Username;
            _LoginData.Password = _Password;
            string _Action = "Token";
            string json = JsonConvert.SerializeObject(_LoginData);
            string _Uri = ConnectionString + _Action;
            Token = CallWebApi.Login(json, _Uri);
            string _Log = DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " user login.";
            return _Log;
            //Login 
        }
        /// <summary>
        /// 取得加權指數清單
        /// </summary>
        /// <returns></returns>
        public   List<StockIndex> getStockIndexList()
        {
            List<StockIndex> _ListStock = new List<StockIndex>();
            string _Action = "StockIndex";
            string _Uri = ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebApi.Get(_Uri, Token);
            _ListStock = JsonConvert.DeserializeObject<List<StockIndex>>(_ApiResult.Data.ToString());
            return _ListStock;
        }
        /// <summary>
        /// 取得股票庫存清單
        /// </summary>
        /// <returns></returns>
        public List<StockInventory> getStockInventoryList()
        {
            List<StockInventory> _ListStockInventory = new List<StockInventory>();
            string _Action = "StockInventory";
            string _Uri = ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebApi.Get(_Uri, Token);
            _ListStockInventory = JsonConvert.DeserializeObject<List<StockInventory>>(_ApiResult.Data.ToString());
            return _ListStockInventory;
        }
        /// <summary>
        /// 取得股票清單
        /// </summary>
        /// <returns></returns>
        public List<StockGroup> getStockGroupList()
        {
            List<StockGroup> _ListStockGroup = new List<StockGroup>();
            string _Action = "StockGroup";
            string _Uri = ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebApi.Get(_Uri, Token);
            _ListStockGroup = JsonConvert.DeserializeObject<List<StockGroup>>(_ApiResult.Data.ToString());
            return _ListStockGroup;
        }
        /// <summary>
        /// 取得股票
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<Stock > getStockByCodeList(string code)
        {
            List<Stock> _ListStock = new List<Stock>();
            string _Action = "Stock?code="+ code;
            string _Uri = ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebApi.Get(_Uri, Token);
            _ListStock = JsonConvert.DeserializeObject<List<Stock>>(_ApiResult.Data.ToString());
            return _ListStock;
        }
        /// <summary>
        /// 透過SQL語法取得股票資料-效能
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public List<Stock> getStockBySqlList(string parameter,string mode)
        {
          
            List<Stock> _ListStock = new List<Stock>();
            string _Action = "Stock?parameter=" + parameter+"&mode="+mode;
            string _Uri = ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebApi.Get(_Uri, Token);
            _ListStock = JsonConvert.DeserializeObject<List<Stock>>(_ApiResult.Data.ToString());
            return _ListStock;
        }
        /// <summary>
        /// 透過日期取得股票資料
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<Stock> getStockYestodayList(string date)
        {
            List<Stock> _ListStock = new List<Stock>();
            string _Action = "StockYestoday?date=" + date;
            string _Uri = ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebApi.Get(_Uri, Token);
            _ListStock = JsonConvert.DeserializeObject<List<Stock>>(_ApiResult.Data.ToString());
            return _ListStock;
        }
        public List<Stock> getStockAllList( )
        {
            List<Stock> _ListStock = new List<Stock>();
            string _Action = "StockAll";
            string _Uri = ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebApi.Get(_Uri, Token);
            _ListStock = JsonConvert.DeserializeObject<List<Stock>>(_ApiResult.Data.ToString());
            return _ListStock;
        }
        /// <summary>
        /// 取得個股期貨代號
        /// </summary>
        /// <returns></returns>
        public List<StockFutureCode> getStockFutureCodeList( )
        {
            List<StockFutureCode> _ListStock = new List<StockFutureCode>();
            string _Action = "StockFutureCode";
            string _Uri = ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebApi.Get(_Uri, Token);
            _ListStock = JsonConvert.DeserializeObject<List<StockFutureCode>>(_ApiResult.Data.ToString());
            return _ListStock;
        }
        /// <summary>
        /// 寫入加權指數
        /// </summary>
        /// <param name="listStockIndex"></param>
        public string  InsertStockIndex(List<StockIndex> listStockIndex)
        {
            string json = JsonConvert.SerializeObject(listStockIndex);
            string _Action = "StockIndex";
            string _Uri = ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebApi.Post(json, _Uri);
            string _Return = _ApiResult.Data.ToString();
            return _Return;
        }
        /// <summary>
        /// 寫入股票價格
        /// </summary>
        /// <param name="listStock"></param>
        public string  insertStock(List<Stock> listStock)
        {
            string json = JsonConvert.SerializeObject(listStock);
            string _Action = "Stock";
            string _Uri = ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebApi.Post(json, _Uri);
            string _Return = _ApiResult.Data.ToString();
            return _Return;
        }
        /// <summary>
        /// 寫入股票庫存
        /// </summary>
        /// <param name="stockInventory"></param>
        public string  InsertStockInventory(StockInventory stockInventory)
        {
            string json = JsonConvert.SerializeObject(stockInventory);
            string _Action = "StockInventory";
            string _Uri = ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebApi.Post(json, _Uri);
            string _Return = _ApiResult.Data.ToString();
            return _Return;
        }
        /// <summary>
        /// 寫入股票清單
        /// </summary>
        /// <param name="stockGroup"></param>
        public string  InsertStockGroup(List<StockGroup> stockGroup)
        {
            string json = JsonConvert.SerializeObject(stockGroup);
            string _Action = "StockGroup";
            string _Uri = ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebApi.Post(json, _Uri);
            string _Return = _ApiResult.Data.ToString();
            return _Return;
        }
        /// <summary>
        /// 更新加權指數
        /// </summary>
        /// <param name="stockIndex"></param>
        public string  UpdateStockIndex(StockIndex stockIndex)
        {
            string json = JsonConvert.SerializeObject(stockIndex);
            string _Action = "StockIndex";
            string _Uri = ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebApi.Put(json, _Uri);
            string _Return = _ApiResult.Data.ToString();
            return _Return;
        }
        /// <summary>
        /// 尚未
        /// </summary>
        /// <param name="juridicaPerson"></param>
        /// <returns></returns>
        public string getJuridicaPerson(StockStrategy.Model.Stock.JuridicaPerson juridicaPerson)
        {
            string json = JsonConvert.SerializeObject(juridicaPerson); 
            string _Uri ="https://www.taifex.com.tw/cht/3/totalTableDate";
            ApiResultEntity _ApiResult = CallWebApi.Post(json, _Uri);
            string _Return = _ApiResult.Data.ToString();
            return _Return;
        } 
        public async Task postLineMsg(string  msg, string token)
        { 
            var result = await CallWebApi.CallLineNotifyApi(token,msg); 
        }
        /// <summary>
        /// 取得Line通知清單
        /// </summary>
        /// <returns></returns>
        public List<StockLineNotify> getStockLineNotifyList( )
        {
            List<StockLineNotify> _ListStockLineNotify = new List<StockLineNotify>();
            string _Action = "StockLineNotify";
            string _Uri = ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebApi.Get(_Uri, Token);
            _ListStockLineNotify = JsonConvert.DeserializeObject<List<StockLineNotify>>(_ApiResult.Data.ToString());
            return _ListStockLineNotify;
        }
        /// <summary>
        /// 更新Line通知參數
        /// </summary>
        /// <param name="stockLineNotify"></param>
        /// <returns></returns>
        public string UpdateStockLineNotify(StockLineNotify stockLineNotify)
        {
            string json = JsonConvert.SerializeObject(stockLineNotify);
            string _Action = "StockLineNotify";
            string _Uri = ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebApi.Put(json, _Uri);
            string _Return = _ApiResult.Data.ToString();
            return _Return;
        }
        /// <summary>
        /// 更新股票資料
        /// </summary>
        /// <param name="stock"></param>
        /// <returns></returns>
        public string UpdateStock(Stock stock)
        {
            string json = JsonConvert.SerializeObject(stock);
            string _Action = "Stock";
            string _Uri = ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebApi.Put(json, _Uri);
            string _Return = _ApiResult.Data.ToString();
            return _Return;
        }
    }
}
