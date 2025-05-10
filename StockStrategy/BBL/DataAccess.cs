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
                     ConnectionString = ConfigurationManager.AppSettings["ApiServer2"];
            } 
            loginWebApi();
        }
        public string  loginWebApi()
        {
			string _PublicKey = ConfigurationManager.AppSettings["PublicKey"];
			string _SecretKey = ConfigurationManager.AppSettings["SecretKey"];
            //Login
            string _Username = Tool.Decrypt(ConfigurationManager.AppSettings["Account"], _PublicKey, _SecretKey);
            string _Password = Tool.Decrypt(ConfigurationManager.AppSettings["Password"], _PublicKey, _SecretKey);
            //Login
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
		/// 透過Linq語法取得股票資料-效能
		/// </summary>
		/// <param name="parameter"></param>
		/// <param name="mode">Date/Code</param>
		/// <returns></returns>
		public List<DataModel.Stock.Stock> getStockBySqlList(string parameter, string mode)
		{

			List<DataModel.Stock.Stock> _ListStock = new List<DataModel.Stock.Stock>();
			string _Action = "Stock?parameter=" + parameter + "&mode=" + mode ;
			string _Uri = ConnectionString + _Action;
			ApiResultEntity _ApiResult = CallWebApi.Get(_Uri, Token);
			_ListStock = JsonConvert.DeserializeObject<List<DataModel.Stock.Stock>>(_ApiResult.Data.ToString());
			return _ListStock;
		}

        /// <summary>
        /// 取得單一股票的清單
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
		public List< Stock> getStockByCodeList(string parameter, string mode)
		{ 
			List< Stock> _ListStock = new List<Stock>();
			string _Action = "StockByCode?parameter=" + parameter + "&mode=" + mode;
			string _Uri = ConnectionString + _Action;
			ApiResultEntity _ApiResult = CallWebApi.Get(_Uri, Token);
			_ListStock = JsonConvert.DeserializeObject<List< Stock>>(_ApiResult.Data.ToString());
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
        /// <summary>
        /// 賴通知
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="token"></param>
        /// <returns></returns>
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
        /// 取得假日
        /// </summary>
        /// <returns></returns>
		public List<Holiday> getHolidayList()
		{       
			List<Holiday> _ListHoliday = new List<Holiday>();
			string _Action = "Holiday";
			string _Uri = ConnectionString + _Action;
			ApiResultEntity _ApiResult = CallWebApi.Get(_Uri, Token);
			_ListHoliday = JsonConvert.DeserializeObject<List<Holiday>>(_ApiResult.Data.ToString());
			return _ListHoliday;
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
        /// <summary>
        /// 更新股票清單為完成 好讓爬蟲辨識已經更新三大法人資料
        /// </summary>
        /// <param name="stockGroup"></param>
        /// <returns></returns>
		public string UpdateStockGroupFinish(StockGroup stockGroup)
		{
			string json = JsonConvert.SerializeObject(stockGroup);
			string _Action = "StockGroup";
			string _Uri = ConnectionString + _Action;
			ApiResultEntity _ApiResult = CallWebApi.Put(json, _Uri);
			string _Return = _ApiResult.Data.ToString();
			return _Return;
		}
		/// <summary>
		/// 選股寫入資料庫
		/// </summary>
		/// <param name="listStockPicking"></param>
		/// <returns></returns>
		public string InsertStockPicking(List<StockPicking> listStockPicking)
		{
			string json = JsonConvert.SerializeObject(listStockPicking);
			string _Action = "StockPicking";
			string _Uri = ConnectionString + _Action;
			ApiResultEntity _ApiResult = CallWebApi.Post(json, _Uri);
			string _Return = _ApiResult.Data.ToString();
			return _Return;
		}
        /// <summary>
        /// 更新選股
        /// </summary>
        /// <param name="stockPicking"></param>
        /// <returns></returns>
		public string UpdateStockPicking(StockPicking stockPicking)
		{
			string json = JsonConvert.SerializeObject(stockPicking);
			string _Action = "StockPicking";
			string _Uri = ConnectionString + _Action;
			ApiResultEntity _ApiResult = CallWebApi.Put(json, _Uri);
			string _Return = _ApiResult.Data.ToString();
			return _Return;
		}
        /// <summary>
        /// 刪除選股
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		public string DeleteStockPicking(int id)
		{ 
			string _Action = "StockPicking?id="+id;
			string _Uri = ConnectionString + _Action;
			ApiResultEntity _ApiResult = CallWebApi.Delete("", _Uri);
			string _Return = _ApiResult.Data.ToString();
			return _Return;
		}
		/// <summary>
		/// 每日寫入回測log
		/// </summary>
		/// <param name="listStockResult"></param>
		/// <returns></returns>
		public string InsertStockResult(List<StockResult> listStockResult)
		{
			string json = JsonConvert.SerializeObject(listStockResult);
			string _Action = "StockResult";
			string _Uri = ConnectionString + _Action;
			ApiResultEntity _ApiResult = CallWebApi.Post(json, _Uri);
			string _Return = _ApiResult.Data.ToString();
			return _Return;
		}
        /// <summary>
        /// 取得選股清單
        /// </summary>
        /// <returns></returns>
		public List<StockPicking> getStockPickingList()
		{
			List<StockPicking> _ListStockPicking = new List<StockPicking>();
			string _Action = "StockPicking";
			string _Uri = ConnectionString + _Action;
			ApiResultEntity _ApiResult = CallWebApi.Get(_Uri, Token);
			_ListStockPicking = JsonConvert.DeserializeObject<List<StockPicking>>(_ApiResult.Data.ToString());
			return _ListStockPicking;
		}
        /// <summary>
        /// 取得股票結果清單
        /// </summary>
        /// <returns></returns>
		public List<StockResult> getStockResultList()
		{
			List<StockResult> _ListStockResult = new List<StockResult>();
			string _Action = "StockResult";
			string _Uri = ConnectionString + _Action;
			ApiResultEntity _ApiResult = CallWebApi.Get(_Uri, Token);
			_ListStockResult = JsonConvert.DeserializeObject<List<StockResult>>(_ApiResult.Data.ToString());
			return _ListStockResult;
		}
        /// <summary>
        /// 刪除股票結果
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		public string DeleteStockResult(int id)
		{
			string _Action = "StockResult?id=" + id;
			string _Uri = ConnectionString + _Action;
			ApiResultEntity _ApiResult = CallWebApi.Delete("", _Uri);
			string _Return = _ApiResult.Data.ToString();
			return _Return;
		}
		public string insertStockHighLow(List<StockHighLow> listStockHighLow)
		{
			string json = JsonConvert.SerializeObject(listStockHighLow);
			string _Action = "StockHighLow";
			string _Uri = ConnectionString + _Action;
			ApiResultEntity _ApiResult = CallWebApi.Post(json, _Uri);
			string _Return = _ApiResult.Data.ToString();
			return _Return;
		}
		public List<StockHighLow> getStockHighLowList()
		{
			List<StockHighLow> _ListStockHighLow = new List<StockHighLow>();
			string _Action = "StockHighLow";
			string _Uri = ConnectionString + _Action;
			ApiResultEntity _ApiResult = CallWebApi.Get(_Uri, Token);
			_ListStockHighLow = JsonConvert.DeserializeObject<List<StockHighLow>>(_ApiResult.Data.ToString());
			return _ListStockHighLow;
		}
		public string insertStockGroupTrend(List<StockGroupTrend> listStockStockGroupTrend)
		{
			string json = JsonConvert.SerializeObject(listStockStockGroupTrend);
			string _Action = "StockGroupTrend";
			string _Uri = ConnectionString + _Action;
			ApiResultEntity _ApiResult = CallWebApi.Post(json, _Uri);
			string _Return = _ApiResult.Data.ToString();
			return _Return;
		}
		public List<StockGroupTrend> getStockGroupTrendList()
		{
			List<StockGroupTrend> _ListStockGroupTrend = new List<StockGroupTrend>();
			string _Action = "StockGroupTrend";
			string _Uri = ConnectionString + _Action;
			ApiResultEntity _ApiResult = CallWebApi.Get(_Uri, Token);
			_ListStockGroupTrend = JsonConvert.DeserializeObject<List<StockGroupTrend>>(_ApiResult.Data.ToString());
			return _ListStockGroupTrend;
		}
		public string insertStockThreeInstitutional(List<StockThreeInstitutional> listStockThreeInstitutional)
		{
			string json = JsonConvert.SerializeObject(listStockThreeInstitutional);
			string _Action = "StockThreeInstitutional";
			string _Uri = ConnectionString + _Action;
			ApiResultEntity _ApiResult = CallWebApi.Post(json, _Uri);
			string _Return = _ApiResult.Data.ToString();
			return _Return;
		}
		public List<StockThreeInstitutional> getStockThreeInstitutionalList()
		{
			List<StockThreeInstitutional> _ListStockThreeInstitutional = new List<StockThreeInstitutional>();
			string _Action = "StockThreeInstitutional";
			string _Uri = ConnectionString + _Action;
			ApiResultEntity _ApiResult = CallWebApi.Get(_Uri, Token);
			_ListStockThreeInstitutional = JsonConvert.DeserializeObject<List<StockThreeInstitutional>>(_ApiResult.Data.ToString());
			return _ListStockThreeInstitutional;
		}
		public List<StockGroupTotalCount> getStockGroupTotalCountList()
		{
			List<StockGroupTotalCount> _ListStockGroupTotalCount = new List<StockGroupTotalCount>();
			string _Action = "StockGroupTotalCount";
			string _Uri = ConnectionString + _Action;
			ApiResultEntity _ApiResult = CallWebApi.Get(_Uri, Token);
			_ListStockGroupTotalCount = JsonConvert.DeserializeObject<List<StockGroupTotalCount>>(_ApiResult.Data.ToString());
			return _ListStockGroupTotalCount;
		}
		public List<StockEventNotify> getStockEventNotifyList()
		{
			List<StockEventNotify> _ListStockEventNotify = new List<StockEventNotify>();
			string _Action = "StockEventNotify";
			string _Uri = ConnectionString + _Action;
			ApiResultEntity _ApiResult = CallWebApi.Get(_Uri, Token);
			_ListStockEventNotify = JsonConvert.DeserializeObject<List<StockEventNotify>>(_ApiResult.Data.ToString());
			return _ListStockEventNotify;
		}
		public string insertStockRevenue(List<StockRevenue> listStockRevenue)
		{
			string json = JsonConvert.SerializeObject(listStockRevenue);
			string _Action = "StockRevenue";
			string _Uri = ConnectionString + _Action;
			ApiResultEntity _ApiResult = CallWebApi.Post(json, _Uri);
			string _Return = _ApiResult.Data.ToString();
			return _Return;
		}
		public string insertStockEps(List<StockEps> listStockEps)
		{
			string json = JsonConvert.SerializeObject(listStockEps);
			string _Action = "StockEps";
			string _Uri = ConnectionString + _Action;
			ApiResultEntity _ApiResult = CallWebApi.Post(json, _Uri);
			string _Return = _ApiResult.Data.ToString();
			return _Return;
		}
		public string insertStockChips(List<StockChips> listStockChips)
		{
			string json = JsonConvert.SerializeObject(listStockChips);
			string _Action = "StockChips";
			string _Uri = ConnectionString + _Action;
			ApiResultEntity _ApiResult = CallWebApi.Post(json, _Uri);
			string _Return = _ApiResult.Data.ToString();
			return _Return;
		}
		public List<StockIndexForcast> getStockIndexForcastList()
		{
			List<StockIndexForcast> _ListStockIndexForcast = new List<StockIndexForcast>();
			string _Action = "StockIndexForcast";
			string _Uri = ConnectionString + _Action;
			ApiResultEntity _ApiResult = CallWebApi.Get(_Uri, Token);
			_ListStockIndexForcast = JsonConvert.DeserializeObject<List<StockIndexForcast>>(_ApiResult.Data.ToString());
			return _ListStockIndexForcast;
		}
		public string insertStockIndexForcast(StockIndexForcast stockIndexForcast)
		{
			string json = JsonConvert.SerializeObject(stockIndexForcast);
			string _Action = "StockIndexForcast";
			string _Uri = ConnectionString + _Action;
			ApiResultEntity _ApiResult = CallWebApi.Post(json, _Uri);
			string _Return = _ApiResult.Data.ToString();
			return _Return;
		}
		public string insertStockIndexStopLossLog(StockIndexStopLossLog stockIndexStopLossLog)
		{
			string json = JsonConvert.SerializeObject(stockIndexStopLossLog);
			string _Action = "StockIndexStopLossLog";
			string _Uri = ConnectionString + _Action;
			ApiResultEntity _ApiResult = CallWebApi.Post(json, _Uri);
			string _Return = _ApiResult.Data.ToString();
			return _Return;
		}
	}
}
