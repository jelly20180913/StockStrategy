using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration; 
using Newtonsoft.Json;
using WebApiService.Models;
using DataModel;
using DataModel.Login; 
namespace StockStrategy.BBL
{
   public class DataAccess
    {
        string ConnectionString;
        string Token;
        public DataAccess()
        { 
         ConnectionString = ConfigurationManager.AppSettings["ApiServer"];
            loginWebApi();
        }
        public string  loginWebApi()
        {
            //Login
            string _Username = ConfigurationManager.AppSettings["Account"];
            string _Password = ConfigurationManager.AppSettings["Password"];
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
        /// 寫入加權指數
        /// </summary>
        /// <param name="listStockIndex"></param>
        public string  insertStockIndex(List<StockIndex> listStockIndex)
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
    }
}
