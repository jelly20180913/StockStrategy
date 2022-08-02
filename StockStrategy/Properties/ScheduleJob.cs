using DataModel;
using DataModel.Login;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;
using WebApiService.Models;
using NLog;
using System.Linq;
using StockStrategy.Common;
using System.Data;
using System.Diagnostics;
using StockStrategy.BBL;
using System.Globalization;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;
namespace StockStrategy.Properties
{
    public partial class ScheduleJob : Form
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        string ConnectionString = "", Token = "";
        List<string> _StockIdList = new List<string>();
        bool stockIndex = false, bUpdateStockIndex = false, bTAIEX = false, bStockAll = false, bStockAllLackOff = false, bUpdateStockLineNotify = false;
        string Id = "Price1_lbTPrice";
        string Change = "Price1_lbTChange";
        string Percent = "Price1_lbTPercent";
        string Volume = "Price1_lbTVolume";
        string GoodStock = "", CtuStock = "", GainType = "", BadStock = "";
        DataAccess _DataAccess = new DataAccess();
        public ScheduleJob()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            loginWebApi();
        }
        /// <summary>
        /// 登入
        /// </summary>
        private void loginWebApi()
        {
            //Login
            string _Username =Tool.Decrypt( ConfigurationManager.AppSettings["Account"],"20220801","B1050520");
            string _Password = Tool.Decrypt(ConfigurationManager.AppSettings["Password"], "20220801", "B1050520");  
            LoginData _LoginData = new LoginData();
            _LoginData.Username = _Username;
            _LoginData.Password = _Password;
            string _Action = "Token";
            string json = JsonConvert.SerializeObject(_LoginData);
            string _Uri = ConnectionString + _Action;
            Token = CallWebApi.Login(json, _Uri);
            string _Log = DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " user login.\r\n";
            this.txtErrMsg.Text += _Log;
            //Login 
        }
        /// <summary>
        /// 啟動後就先登入WebAPI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScheduleJob_Load(object sender, EventArgs e)
        {
            ConnectionString = ConfigurationManager.AppSettings["ApiServer2"];
            this.Text = "股票策略選股排程機：V" + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion.ToString();
            // loginWebApi();
        }
        /// <summary>
        /// 寫入加權指數到資料庫
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetIndexToInsert_Click(object sender, EventArgs e)
        {
            string _Log = "";
            try
            {

                this.btnLogin.PerformClick();
                string _Id = "Price1_lbTPrice";
                string _Change = "Price1_lbTChange";
                string _Percent = "Price1_lbTPercent";
                string _Volume = "Price1_lbTVolume";
                //  string _Class = "rtVal1";
                List<StockStrategy.Model.Stock.GetRealtimeStockPrice> _PriceList = new List<StockStrategy.Model.Stock.GetRealtimeStockPrice>();
                StockStrategy.Model.Stock.GetRealtimePriceIn _RealtimePriceIn = new StockStrategy.Model.Stock.GetRealtimePriceIn();
                _RealtimePriceIn.Sample1_Symbol = "t00";
                string _PHLX_URL = ConfigurationManager.AppSettings["PHLX_URL"];
                string _DJI_URL = ConfigurationManager.AppSettings["DJI_URL"];
                string _NASDAQ_URL = ConfigurationManager.AppSettings["NASDAQ_URL"];
                string _MTX_URL = ConfigurationManager.AppSettings["MTX_URL"];
                string _TX_URL = ConfigurationManager.AppSettings["TX_URL"];
                string _TPEx_URL = ConfigurationManager.AppSettings["TPEx_URL"];
                string _PHLX_Index = Common.Job.GetTaiwanFutures(_PHLX_URL, _Id, true);
                string _DJI_Index = Common.Job.GetTaiwanFutures(_DJI_URL, _Id, true);
                string _NASDAQ_Index = Common.Job.GetTaiwanFutures(_NASDAQ_URL, _Id, true);
                string _MTX_Index = Common.Job.GetTaiwanFutures(_MTX_URL, _Id, true);
                if (_RealtimePriceIn.Sample1_Symbol != null)
                {
                    _PriceList = StockStrategy.BBL.StockPrice.GetRealtimePrice(_RealtimePriceIn);
                }
                StockStrategy.Model.Stock.GetRealtimeStockPrice _StockPrice = new Model.Stock.GetRealtimeStockPrice();
                if (_PriceList.Count() > 0) _StockPrice = _PriceList.First();
                string _TX_Index = _PriceList.Count() > 0 ? _PriceList.First().Price : "";

                int _Yestoday = DateTime.Now.DayOfWeek.ToString() == "Monday" ? -3 : -1;
                // string _TopIndex = getTopIndex(DateTime.Now.AddDays(_Yestoday).ToString("yyyyMMdd"));
                List<StockIndex> _ListStockAll = getStockIndexList();
                string _TopIndex = _ListStockAll.Where(x => x.Date == DateTime.Now.AddDays(_Yestoday).ToString("yyyyMMdd")).First().TAIEX;
                string _TopIndexDJI = _ListStockAll.Where(x => x.Date == DateTime.Now.AddDays(_Yestoday).ToString("yyyyMMdd")).First().DJI;
                List<StockIndex> _ListStock = new List<StockIndex>();
                StockIndex s = new StockIndex();
                s.ContinueName = "";
                s.DJI_QuotePercent = Common.Job.GetTaiwanFutures(_DJI_URL, _Volume, true);
                s.Date = DateTime.Now.ToString("yyyyMMdd");
                s.DayOfWeek = Mapping.DayOfWeekList.Where(x => x.Day == DateTime.Now.DayOfWeek.ToString()).First().Show;
                s.DJI = _DJI_Index;
                s.DJI_QuoteChange = Common.Job.GetTaiwanFutures(_DJI_URL, _Change, true);
                s.DJI_QuotePercent = Common.Job.GetTaiwanFutures(_DJI_URL, _Percent, true);
                s.MTX = Common.Job.GetTaiwanFutures(_MTX_URL, _Id, true);
                s.MTX_High = "";
                s.MTX_Open = "";
                s.MTX_QuoteChange = Common.Job.GetTaiwanFutures(_MTX_URL, _Change, true);
                s.MTX_QuotePercent = Common.Job.GetTaiwanFutures(_MTX_URL, _Percent, true);
                s.MTX_Volume = Common.Job.GetTaiwanFutures(_MTX_URL, _Volume, true);
                s.NASDAQ = Common.Job.GetTaiwanFutures(_NASDAQ_URL, _Id, true);
                s.NASDAQ_QuoteChange = Common.Job.GetTaiwanFutures(_NASDAQ_URL, _Change, true);
                s.NASDAQ_QuotePercent = Common.Job.GetTaiwanFutures(_NASDAQ_URL, _Percent, true);
                s.PHLX = Common.Job.GetTaiwanFutures(_PHLX_URL, _Id, true);
                s.PHLX_QuoteChange = Common.Job.GetTaiwanFutures(_PHLX_URL, _Change, true);
                s.PHLX_QuotePercent = Common.Job.GetTaiwanFutures(_PHLX_URL, _Percent, true);
                s.TX = Common.Job.GetTaiwanFutures(_MTX_URL, _Id, true);
                s.TX_High = "";
                s.TX_Open = "";
                s.TX_QuoteChange = Common.Job.GetTaiwanFutures(_MTX_URL, _Change, true);
                s.TX_QuotePercent = Common.Job.GetTaiwanFutures(_MTX_URL, _Percent, true);
                s.TX_Volume = Common.Job.GetTaiwanFutures(_MTX_URL, _Volume, true);
                s.TAIEX = _StockPrice.Price;
                s.TAIEX_High = _StockPrice.HighPrice;
                s.TAIEX_Open = _StockPrice.OpenPrice;
                s.TAIEX_QuoteChange = Convert.ToString(Math.Round(Convert.ToDouble(_StockPrice.Price) - Convert.ToDouble(_TopIndex), 2));
                s.TAIEX_QuotePercent = Convert.ToString(Math.Round(Convert.ToDouble(s.TAIEX_QuoteChange) * 100 / Convert.ToDouble(_StockPrice.Price), 2));
                s.Volume = _StockPrice.TotalDealQty;
                s.TPEx = Common.Job.GetTaiwanFutures(_TPEx_URL, _Id, true);
                s.TPEx_QuoteChange = Common.Job.GetTaiwanFutures(_TPEx_URL, _Change, true);
                s.TPEx_QuotePercent = Common.Job.GetTaiwanFutures(_TPEx_URL, _Percent, true);

                if (_TopIndexDJI == _DJI_Index)
                {
                    s.DJI = "0";
                    s.NASDAQ = "0";
                    s.PHLX = "0";
                }

                _ListStock.Add(s);
                insertStockIndex(_ListStock);
                stockIndex = true;
                _Log = DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " insert stock index ok.\r\n";
                this.txtErrMsg.Text += _Log;
            }
            catch (Exception ex)
            {
                _Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " GetIndexToInsert:" + ex.Message + "\r\n";
                logger.Error(_Log);
                this.txtErrMsg.Text += _Log;
            }
        }
        /// <summary>
        /// 取得加權指數
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetTopIndex_Click(object sender, EventArgs e)
        {
            this.btnLogin.PerformClick();
            List<StockIndex> _ListStock = getStockIndexList();
        }
        /// <summary>
        /// 取得前一日的指數
        /// </summary>
        /// <param name="yestoday"></param>
        /// <returns></returns>
        private string getTopIndex(string yestoday)
        {
            List<StockIndex> _ListStock = getStockIndexList();
            return _ListStock.Where(x => x.Date == yestoday).First().TAIEX;
        }
        private string getDJITopIndex(string yestoday)
        {
            List<StockIndex> _ListStock = getStockIndexList();
            return _ListStock.Where(x => x.Date == yestoday).First().DJI;
        }
        private List<StockIndex> getStockIndexList()
        {
            List<StockIndex> _ListStock = new List<StockIndex>();
            string _Action = "StockIndex";
            string _Uri = ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebApi.Get(_Uri, Token);
            _ListStock = JsonConvert.DeserializeObject<List<StockIndex>>(_ApiResult.Data.ToString());
            return _ListStock;
        }
        /// <summary>
        /// 取得股票清單
        /// </summary>
        /// <returns></returns>
        private List<StockGroup> getStockGroupList()
        {
            List<StockGroup> _ListStockGroup = new List<StockGroup>();
            string _Action = "StockGroup";
            string _Uri = ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebApi.Get(_Uri, Token);
            _ListStockGroup = JsonConvert.DeserializeObject<List<StockGroup>>(_ApiResult.Data.ToString());
            return _ListStockGroup;
        }
        /// <summary>
        /// 寫入加權指數
        /// </summary>
        /// <param name="listStockIndex"></param>
        private void insertStockIndex(List<StockIndex> listStockIndex)
        {
            string json = JsonConvert.SerializeObject(listStockIndex);
            string _Action = "StockIndex";
            string _Uri = ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebApi.Post(json, _Uri);
            string _Return = _ApiResult.Data.ToString();
        }
        /// <summary>
        /// 寫入股票價格
        /// </summary>
        /// <param name="listStock"></param>
        private void insertStock(List<Stock> listStock)
        {
            string json = JsonConvert.SerializeObject(listStock);
            string _Action = "Stock";
            string _Uri = ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebApi.Post(json, _Uri);
            string _Return = _ApiResult.Data.ToString();
        }
        /// <summary>
        /// 寫入股票庫存
        /// </summary>
        /// <param name="stockInventory"></param>
        private void insertStockInventory(StockInventory stockInventory)
        {
            string json = JsonConvert.SerializeObject(stockInventory);
            string _Action = "StockInventory";
            string _Uri = ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebApi.Post(json, _Uri);
            string _Return = _ApiResult.Data.ToString();
        }
        /// <summary>
        /// 寫入股票清單
        /// </summary>
        /// <param name="stockGroup"></param>
        private void insertStockGroup(List<StockGroup> stockGroup)
        {
            string json = JsonConvert.SerializeObject(stockGroup);
            string _Action = "StockGroup";
            string _Uri = ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebApi.Post(json, _Uri);
            string _Return = _ApiResult.Data.ToString();
        }
        /// <summary>
        /// 更新加權指數
        /// </summary>
        /// <param name="stockIndex"></param>
        private void updateStockIndex(StockIndex stockIndex)
        {
            string json = JsonConvert.SerializeObject(stockIndex);
            string _Action = "StockIndex";
            string _Uri = ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebApi.Put(json, _Uri);
            string _Return = _ApiResult.Data.ToString();
        }
        /// <summary>
        /// 六日不執行
        /// 1. 08:10 寫入美股加權指數 道瓊 費半 那斯達克
        ///          寫入昨日股票價格
        ///          寫入缺少的股票價格
        /// 2. 14:00 更新台股加權指數 櫃買
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerInsertStockIndex_Tick(object sender, EventArgs e)
        {
            string _Hour = DateTime.Now.Hour.ToString();
            string _Minute = DateTime.Now.Minute.ToString();
            string _DayOfWeek = DateTime.Now.DayOfWeek.ToString();
            string _Day = DateTime.Now.Day.ToString();
            if (_Hour == "7" && _Minute == "0")
            {
                stockIndex = false;
                bUpdateStockIndex = false;
                bTAIEX = false;
                bStockAll = false;
                bStockAllLackOff = false;
                btnResetLinePoint.PerformClick();
            }
            if (_DayOfWeek != "Sunday" && _DayOfWeek != "Saturday")
            {
                if (_Hour == "8" && _Minute == "35")
                {
                    if (!bStockAll)
                    {
                        bStockAll = true;
                        btnGetStockPrice.PerformClick();
                    }
                }
                if (_Hour == "8" && _Minute == "35" && !stockIndex)
                {
                    btnGetIndexToInsert.PerformClick();
                } 
                if (_Hour == "13" && _Minute == "45")
                {
                    if (!bTAIEX)
                        btnUpdateStockIndex.PerformClick();
                }
                if (_Hour == "13" && _Minute == "55")
                {

                    if (!bStockAllLackOff)
                    {
                        bStockAllLackOff = true;
                        btnInsertStockLackOff.PerformClick();
                    }

                }
                if (_Hour == "14" && _Minute == "55")
                {
                    if (!bUpdateStockLineNotify)
                    {
                        bUpdateStockLineNotify = true;
                        btnResetLinePoint.PerformClick();
                    }
                }
            }
        }
        /// <summary>
        /// 當天會先捉取HiStock股價資訊存到資料庫
        /// 隔天從證交所抓到前一天的股價資料則用更新的方式更新回去
        /// 若有新的資料再用新增的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetStockPrice_Click(object sender, EventArgs e)
        {
            string _Log = "";
            try
            {

                DataAccess _DataAccess = new DataAccess();
                int _Yestoday = DateTime.Now.DayOfWeek.ToString() == "Monday" ? -3 : -1;
                List<Stock> _YestodayStockDayAllList = _DataAccess.getStockYestodayList(DateTime.Now.AddDays(_Yestoday).Date.ToString("yyyyMMdd"));
                List<Stock> _StockList = setStockList();
                List<Stock> _StockAddList = new List<Stock>();
                foreach (Stock s in _StockList)
                {
                    if (_YestodayStockDayAllList.Where(x => x.Code == s.Code).ToList().Count > 0)
                    {
                        Stock _Stock = _YestodayStockDayAllList.Where(x => x.Code == s.Code).First();
                        _Stock.OpeningPrice = s.OpeningPrice;
                        _Stock.HighestPrice = s.HighestPrice;
                        _Stock.LowestPrice = s.LowestPrice;
                        _Stock.TransactionValue = s.TransactionValue;
                        _Stock.Shock = s.Shock;
                        _DataAccess.UpdateStock(_Stock);
                    }
                    else
                    {
                        _StockAddList.Add(s);
                    }
                }
                insertStock(_StockAddList);
                _Log = DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " insert stock  ok.\r\n";
                this.txtErrMsg.Text += _Log;
            }
            catch (Exception ex)
            {
                _Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " GetStockPrice:" + ex.Message + "\r\n";
                logger.Error(_Log);
                this.txtErrMsg.Text += _Log;

            }
        }
        /// <summary>
        /// 塞入股票清單物件
        /// </summary>
        /// <returns></returns>
        private List<Stock> setStockList()
        {
            string _Log = "";
            DataAccess _DataAccess = new DataAccess();
            List<StockStrategy.Model.Stock.StockDayAll> _StockDayAllList = StockStrategy.BBL.StockPrice.GetStockDayAll();
            int _Tuesday = DateTime.Now.DayOfWeek.ToString() == "Tuesday" ? -4 : -2;
            int _Yestoday = DateTime.Now.DayOfWeek.ToString() == "Monday" ? -3 : -1;
            List<Stock> _YestodayStockDayAllList = _DataAccess.getStockYestodayList(DateTime.Now.AddDays(_Tuesday).Date.ToString("yyyyMMdd"));
            List<Stock> _StockList = new List<Stock>();
            string _HiStock_URL = ConfigurationManager.AppSettings["HiStock_URL"];
            try
            {
                string _YestodayPrice = "";
                foreach (StockStrategy.Model.Stock.StockDayAll s in _StockDayAllList)
                {
                    Stock _Stock = new Stock();
                    _Stock.Code = s.Code;
                    //string _YestodayPrice = _DataAccess.getStockByCodeList(s.Code).OrderByDescending(x => x.Date).First().ClosingPrice;
                    if (_YestodayStockDayAllList.Where(x => x.Code == s.Code).ToList().Count > 0)
                        _YestodayPrice = _YestodayStockDayAllList.Where(x => x.Code == s.Code).First().ClosingPrice;
                    _Stock.Name = s.Name;
                    _Stock.TradeVolume = s.TradeVolume;
                    _Stock.TradeValue = s.TradeValue;
                    _Stock.OpeningPrice = s.OpeningPrice;
                    _Stock.HighestPrice = s.HighestPrice;
                    _Stock.LowestPrice = s.LowestPrice;
                    _Stock.ClosingPrice = s.ClosingPrice;
                    if (s.HighestPrice != "" && s.LowestPrice != "" && s.ClosingPrice != "")
                    {
                        double _Diff = Convert.ToDouble(s.HighestPrice) - Convert.ToDouble(s.LowestPrice);
                        _Stock.Shock = Convert.ToString(Math.Round((_Diff * 100) / Convert.ToDouble(s.ClosingPrice), 2));
                    }

                    _Stock.Change = s.Change;
                    //if (s.Code == "2454")
                    //{
                    //if (Convert.ToDouble(s.Change) == 0)
                    //    _Stock.Change = Common.Job.GetTaiwanFutures(_HiStock_URL + s.Code, Change, true); 
                    if (_YestodayPrice != "")
                        _Stock.Gain = Convert.ToString(Math.Round(((Convert.ToDouble(s.Change) * 100) / Convert.ToDouble(_YestodayPrice)), 2));
                    // }
                    _Stock.TransactionValue = s.Transaction;
                    _Stock.Date = DateTime.Now.AddDays(_Yestoday).ToString("yyyyMMdd");
                    _Stock.UpdateTime = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
                    _StockList.Add(_Stock); 
                }
            }
            catch (Exception ex)
            {
                _Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " setStockList:" + ex.Message + "\r\n";
                logger.Error(_Log);
                this.txtErrMsg.Text += _Log;
            }
            return _StockList;
        }
        /// <summary>
        /// 飆股選股
        /// 1. 大於700張
        /// 2. 漲幅超過3%
        /// 3. 成交量大於昨日5倍量
        /// 例外
        /// 1. 前x日小於Y量:可判斷是否盤整一個大平台
        /// 2. 前x日不得高於現價y%:可排除套牢頭部
        /// </summary>
        /// <returns></returns>
        private List<Stock> getGoodStockList()
        {
            string _Log = "";
            DataAccess _DataAccess = new DataAccess();
            DateTime _Dt = Convert.ToDateTime(this.dateTimePicker1.Text);
            string _WhereDate = _Dt.ToString("yyyyMMdd");
            string _DayOfWeek = _Dt.DayOfWeek.ToString();
            int _Yestoday = _DayOfWeek == "Monday" ? -3 : -1; 
            List<Stock> _YestodayStockDayAllList = _DataAccess.getStockBySqlList(_Dt.AddDays(_Yestoday).Date.ToString("yyyyMMdd"), "Date"); 
            List<Stock> _StockDayAllList = _DataAccess.getStockBySqlList(_WhereDate, "Date");
            List<Stock> _StockList = new List<Stock>();
            decimal _Gain = this.nupGain.Value != 0 ? this.nupGain.Value : 3;
            int _Volumn = this.txtVolumn.Text != "" ? Convert.ToInt32(this.txtVolumn.Text) * 1000 : 700000;
            int multiple = this.txtMultiple.Text != "" ? Convert.ToInt32(this.txtMultiple.Text) : 5;
            int _PreDays = this.txtPreDays.Text != "" ? Convert.ToInt32(this.txtPreDays.Text) : 10;
            int _LessVolumn = this.txtLessVolumn.Text != "" ? Convert.ToInt32(this.txtLessVolumn.Text) * 1000 : 1000000;
            int _PreDays2 = this.txtPreDays2.Text != "" ? Convert.ToInt32(this.txtPreDays2.Text) : 10;
            int _MoreGain = this.txtMoreGain.Text != "" ? Convert.ToInt32(this.txtMoreGain.Text) : 10;
            try
            {
                List<Stock> _GoodStockList = _StockDayAllList.Where(x => Convert.ToDecimal(x.Gain) > _Gain && Convert.ToDouble(x.TradeVolume) > _Volumn).ToList();
                foreach (Stock s in _GoodStockList)
                {
                    if (_YestodayStockDayAllList.Where(x => x.Code == s.Code).ToList().Count > 0)
                    {
                        Stock _YestodayStock = _YestodayStockDayAllList.Where(x => x.Code == s.Code).First();
                        double _Multiple = Convert.ToDouble(s.TradeVolume) / Convert.ToDouble(_YestodayStock.TradeVolume);
                        if (_Multiple > multiple)
                        {
                            bool   _Less = true, _NotHigherThan = true; 
                            List<Stock> _StockListByCode = _DataAccess.getStockBySqlList(s.Code, "Code").OrderByDescending(x => x.Date).ToList();
                            if (chkLess.Checked)
                            {
                                _Less = _StockListByCode.Take(_PreDays + 1).Where(x => Convert.ToDouble(x.TradeVolume) < _LessVolumn).ToList().Count >= _PreDays;
                            }
                            if (chkNotHigherThan.Checked)
                            {
                                double _Price = Convert.ToDouble(s.ClosingPrice) + Convert.ToDouble(s.ClosingPrice) * _MoreGain / 100;
                                _NotHigherThan = _StockListByCode.Take(_PreDays2).Where(x => Convert.ToDouble(x.ClosingPrice) > _Price).ToList().Count == 0;
                            }
                            if (_Less && _NotHigherThan) _StockList.Add(s);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " getGoodStockList:" + ex.Message + "\r\n";
                logger.Error(_Log);
                this.txtErrMsg.Text += _Log;
            }
            return _StockList;
        }
        /// <summary>
        /// 壞股票選股策略-爆量出貨
        ///1.	高收盤差X%超過Y量
        ///2.	跌Z%
        ///3.	成交量大於I日總和
        ///4.	跌破日/10日/月線

        /// </summary>
        /// <returns></returns>
        private List<Stock> getBadStockList()
        {
            double _Ma5 = 0, _Ma10 = 0, _Ma20 = 0;
            string _Code = "";
            string _Log = "";
            DataAccess _DataAccess = new DataAccess();
            DateTime _Dt = Convert.ToDateTime(this.dtpDateBad.Text);
            string _WhereDate = _Dt.ToString("yyyyMMdd");
            string _DayOfWeek = _Dt.DayOfWeek.ToString();
            int _Yestoday = _DayOfWeek == "Monday" ? -3 : -1;
            List<Stock> _YestodayStockDayAllList = _DataAccess.getStockBySqlList(_Dt.AddDays(_Yestoday).Date.ToString("yyyyMMdd"), "Date");
            List<Stock> _StockDayAllList = _DataAccess.getStockBySqlList(_WhereDate, "Date");
            List<Stock> _StockList = new List<Stock>();
            decimal _Diff = this.nudDiff.Value != 0 ? this.nudDiff.Value : 3;
            int _Volumn = this.txtVolumnBad.Text != "" ? Convert.ToInt32(this.txtVolumnBad.Text) * 1000 : 7000000;
            int _Drop = this.nudDropBad.Value != 0 ? Convert.ToInt32(this.nudDropBad.Value) : 5;
            int _PreDayBad = this.txtPreDaysBad.Text != "" ? Convert.ToInt32(this.txtPreDaysBad.Text) : 10;
            try
            {
                List<Stock> _BadStockList = _StockDayAllList.Where(x => x.TradeVolume != "" && Convert.ToDouble(x.TradeVolume) > Convert.ToDouble(_Volumn)).ToList();
                foreach (Stock s in _BadStockList)
                {
                    _Code = s.Code;
                    List<Stock> _StockListByCode = _DataAccess.getStockBySqlList(s.Code, "Code").Where(x => Convert.ToInt32(x.Date) <= Convert.ToInt32(_WhereDate)).OrderByDescending(x => x.Date).ToList();
                    if (_StockListByCode.Count > 5) _Ma5 = Math.Round(_StockListByCode.OrderByDescending(x => x.Date).ToList().Take(5).Sum(x => Convert.ToDouble(x.ClosingPrice)) / 5, 2);
                    if (_StockListByCode.Count > 10) _Ma10 = Math.Round(_StockListByCode.OrderByDescending(x => x.Date).ToList().Take(10).Sum(x => Convert.ToDouble(x.ClosingPrice)) / 10, 2);
                    if (_StockListByCode.Count > 20) _Ma20 = Math.Round(_StockListByCode.OrderByDescending(x => x.Date).ToList().Take(20).Sum(x => Convert.ToDouble(x.ClosingPrice)) / 20, 2);
                    Stock _YestodayStock = _YestodayStockDayAllList.Where(x => x.Code == s.Code).First();
                    decimal _Gain = Math.Round((Convert.ToDecimal(s.HighestPrice) - Convert.ToDecimal(s.ClosingPrice)) * 100 / Convert.ToDecimal(_YestodayStock.ClosingPrice), 2);
                    if (_Gain > _Diff)
                    {
                        bool _Less = true, _TotalVolumne = true, _MA5 = true, _MA10 = true, _MA20 = true;
                        if (chkDrop.Checked)
                        {
                            _Less = Convert.ToDouble(s.Gain) <= Convert.ToDouble(-_Drop);
                        }
                        if (chkVolumnTotal.Checked)
                        {
                            _StockListByCode.RemoveAt(0);
                            double _Total = _StockListByCode.Take(_PreDayBad).Sum(x => Convert.ToDouble(x.TradeVolume));
                            _TotalVolumne = Convert.ToDouble(s.TradeVolume) > _Total;
                        }
                        if (chkDropMa5.Checked)
                        {
                            _MA5 = Convert.ToDouble(s.ClosingPrice) < _Ma5;
                        }
                        if (chkDropMa10.Checked)
                        {
                            _MA10 = Convert.ToDouble(s.ClosingPrice) < _Ma10;
                        }
                        if (chkDropMa20.Checked)
                        {
                            _MA20 = Convert.ToDouble(s.ClosingPrice) < _Ma20;
                        }
                        if (_Less && _TotalVolumne && _MA5 && _MA10 && _MA20) _StockList.Add(s);
                    }

                }
            }
            catch (Exception ex)
            {
                _Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + "Code:" + _Code + " getBadStockList:" + ex.Message + "\r\n";
                logger.Error(_Log);
                this.txtErrMsg.Text += _Log;
            }
            return _StockList;
        }
        /// <summary>
        /// 連續型態
        /// 1.	前X日漲/跌Y天,超過Z%/日
        /// 2.	總漲幅大於/小於I%
        /// </summary>
        /// <returns></returns>
        private List<Stock> getCtuStockList()
        {
            string _Code = "";
            string _Log = "";
            DataAccess _DataAccess = new DataAccess();
            DateTime _Dt = Convert.ToDateTime(this.dtpCtuDate.Text);
            string _WhereDate = _Dt.ToString("yyyyMMdd");
            string _DayOfWeek = _Dt.DayOfWeek.ToString();
            int _Yestoday = _DayOfWeek == "Monday" ? -3 : -1;
            List<Stock> _StockDayAllList = _DataAccess.getStockBySqlList(_WhereDate, "Date");
            List<Stock> _StockList = new List<Stock>();
            decimal _Gain = this.nupCtuGain.Value != 0 ? this.nupCtuGain.Value : 3;
            int _PreDays = this.txtPreDaysCtu.Text != "" ? Convert.ToInt32(this.txtPreDaysCtu.Text) : 10;
            int _CtuDays = this.txtCtuDays.Text != "" ? Convert.ToInt32(this.txtCtuDays.Text) : 10;
            double _MoreGain = this.nupCtuGain.Value != 0 ? Convert.ToDouble(this.nupCtuGain.Value) : 10;
            double _TotalGain = this.txtTotalGain.Text != "" ? Convert.ToDouble(this.txtTotalGain.Text) : 10; 
            try
            {
                foreach (Stock s in _StockDayAllList)
                {
                    _Code = s.Code; 
                    bool _Ctu = false, _TotalAmp = true;
                    List<Stock> _StockListByCode = _DataAccess.getStockBySqlList(s.Code, "Code").Where(x => Convert.ToInt32(x.Date) <= Convert.ToInt32(_WhereDate)).OrderByDescending(x => x.Date).Take(_PreDays).ToList();
                    int _GainDays = _StockListByCode.Where(x => Convert.ToDouble(x.Gain) > _MoreGain).ToList().Count;
                    if (GainType == "跌") _GainDays = _StockListByCode.Where(x => Convert.ToDouble(x.Gain) < -_MoreGain).ToList().Count; 
                    if (_GainDays >= _CtuDays)
                    {
                        _Ctu = true;
                    }
                    if (chkTotalAmp.Checked)
                    {
                        if (s.ClosingPrice != "")
                        {
                            double _LastPrice = Convert.ToDouble(s.ClosingPrice);
                            if (_StockListByCode.OrderBy(x => x.Date).First().ClosingPrice != "")
                            {
                                double _FirstPrice = Convert.ToDouble(_StockListByCode.OrderBy(x => x.Date).First().ClosingPrice);
                                double _TotalAmpGain = Math.Round((_LastPrice - _FirstPrice) / _FirstPrice, 2);
                                _TotalAmp = _TotalAmpGain > (_TotalGain / 100);
                                if (GainType == "跌")
                                {
                                    _TotalAmp = _TotalAmpGain < -(_TotalGain / 100);
                                }
                            }
                        }
                    }
                    if (_Ctu && _TotalAmp) _StockList.Add(s); 
                }
            }
            catch (Exception ex)
            {
                _Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " Code:" + _Code + " getCtuStockList:" + ex.Message + "\r\n";
                logger.Error(_Log);
            }
            return _StockList;
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            string _Log = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    txtFile.Text = openFileDialog1.FileName;

                }
                catch (Exception ex)
                {
                    _Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " btnOpen_Click:" + ex.Message + "\r\n";
                    logger.Error(_Log);
                    this.txtErrMsg.Text += _Log;
                }
            }
        }
        /// <summary>
        /// 寫入缺少的股票價格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInsertStockLackOff_Click(object sender, EventArgs e)
        {
            this.lbBtnName.Text = "Insert Stock Lack Off";
            List<StockGroup> _StockGroupList = getStockGroupList();
            this.progressBar1.Maximum = _StockGroupList.Count;
            this.progressBar1.Step = 1;
            //因為捉取網站資料會較久所以呼叫另一執行緒處理
            var t = new Task(insertStockLackOffList);
            t.Start();
        }
        private void insertStockLackOffList()
        {
            string _Log = "";
            try
            {
                List<Stock> _StockList = setStockLackOffList();
                insertStock(_StockList);

            }
            catch (Exception ex)
            {
                _Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " insertStockLackOffList:" + ex.Message + "\r\n";
                logger.Error(_Log); 
            }
        }

        /// <summary>
        /// 尚未取得資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateJuridicaPerson1_Click(object sender, EventArgs e)
        {
            string _Log = "";
            try
            {
                string _Juridica = "MII_1_4";
                string _TX_URL = ConfigurationManager.AppSettings["TX_URL"];
                StockIndex s = new StockIndex();
                List<StockIndex> _ListStock = getStockIndexList();
                s = _ListStock.OrderByDescending(x => x.Date).First();
                s.JuridicaPerson = Common.Job.GetTaiwanFutures(_TX_URL, _Juridica, true);
                updateStockIndex(s);
                _Log = "\r\n" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " update stock juridica ok.";
                this.txtErrMsg.Text += _Log;
                bTAIEX = true;
            }
            catch (Exception ex)
            {
                _Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " btnUpdateJuridicaPerson:" + ex.Message + "\r\n";
                logger.Error(_Log);
                this.txtErrMsg.Text += _Log;
            }
        }
        /// <summary>
        /// 更新法人買賣超資料-尚未完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateJuridicaPerson_Click(object sender, EventArgs e)
        {
            string _Log = "";
            try
            {
                StockIndex s = new StockIndex();
                List<StockIndex> _ListStock = getStockIndexList();
                s = _ListStock.OrderByDescending(x => x.Date).First();
                DataAccess _DataAccess = new DataAccess();
                StockStrategy.Model.Stock.JuridicaPerson _JuridicaPerson = new Model.Stock.JuridicaPerson();
                _JuridicaPerson.queryType = "1";
                _JuridicaPerson.goDay = "";
                _JuridicaPerson.doQuery = "1";
                _JuridicaPerson.dateaddcnt = "";
                _JuridicaPerson.queryDate = "2022/06/20";
                _DataAccess.getJuridicaPerson(_JuridicaPerson);
                //  s.JuridicaPerson = Common.Job.GetTaiwanFutures(_TX_URL, _Juridica, true);
                updateStockIndex(s);
                _Log = DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " update stock juridica ok.\r\n";
                this.txtErrMsg.Text += _Log;
                bTAIEX = true;
            }
            catch (Exception ex)
            {
                _Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " btnUpdateJuridicaPerson:" + ex.Message + "\r\n";
                logger.Error(_Log);
                this.txtErrMsg.Text += _Log;
            }
        }

        private void btnGetGoodStock_Click(object sender, EventArgs e)
        {
            progressBar2.Style = ProgressBarStyle.Marquee;
            progressBar2.MarqueeAnimationSpeed = 30;
            progressBar2.Show();
            var t = new Task(getGoodStock);
            t.Start();
        }
        private void getGoodStock()
        { 
            string _Log = "";
            GoodStock = "";
            try
            { 
                List<Stock> _StockList = getGoodStockList();

                foreach (Stock s in _StockList)
                {
                    GoodStock = GoodStock + s.Code + ";";
                }
                MethodInvoker mi = new MethodInvoker(this.UpdateUIGoodStock);
                this.BeginInvoke(mi, null);

            }
            catch (Exception ex)
            {
                _Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " GetGoodStock:" + ex.Message + "\r\n";
                logger.Error(_Log); 
            }
        }
        private void UpdateUIGoodStock()
        {
            this.txtGoodStock.Text = GoodStock;
            progressBar2.Style = ProgressBarStyle.Continuous;
        }
        private void UpdateUICtuStock()
        {
            this.txtCtuStock.Text = CtuStock;
            pgBarCtu.Style = ProgressBarStyle.Continuous;
        }

        private void getBadStock()
        { 
            string _Log = "";
            BadStock = "";
            try
            { 
                List<Stock> _StockList = getBadStockList(); 
                foreach (Stock s in _StockList)
                {
                    BadStock = BadStock + s.Code + ";";
                }
                MethodInvoker mi = new MethodInvoker(this.UpdateUIBadStock);
                this.BeginInvoke(mi, null); 
            }
            catch (Exception ex)
            {
                _Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " GetBadStock:" + ex.Message + "\r\n";
                logger.Error(_Log); 
            }
        }
        private void UpdateUIBadStock()
        {
            this.txtBadStock.Text = BadStock;
            pgBarBad.Style = ProgressBarStyle.Continuous;
        }
        /// <summary>
        /// 只會用到一次,補先前Stock資料的欄位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateStockGain_Click(object sender, EventArgs e)
        {
            string _Log = "";
            DataAccess _DataAccess = new DataAccess();
            Stopwatch sw = new Stopwatch();
            sw.Reset();
            sw.Start();
            int _No = 0;
            try
            {
                //Lack Off
                // List<Stock> _StockList = _DataAccess.getStockAllList().Where(x=>x.HighestPrice==null&&x.Gain==null).ToList();
                //string _UpdatTime = DateTime.Now.ToString("yyyyMMdd hh:mm:ss");
                // foreach (Stock s in _StockList) {
                //     s.Gain = s.Change.Replace('+',' ');
                //     s.Change = "";
                //     s.UpdateTime = _UpdatTime;
                //     _DataAccess.UpdateStock(s);
                //     _No++;
                // } 

                List<Stock> _StockList = _DataAccess.getStockAllList().Where(x => x.HighestPrice != "" && x.Gain == null).ToList();
                string _UpdatTime = DateTime.Now.ToString("yyyyMMdd hh:mm:ss");
                CultureInfo provider = CultureInfo.InvariantCulture;
                string format = "yyyyMMdd";

                foreach (Stock s in _StockList)
                {
                    string _YestodayPrice = "";
                    DateTime _Dt = DateTime.ParseExact(s.Date, format, provider);
                    string _DayOfWeek = _Dt.DayOfWeek.ToString();
                    int _Yestoday = _DayOfWeek == "Monday" ? -3 : -1; 
                    if (_StockList.Where(x => x.Date == _Dt.AddDays(_Yestoday).ToString("yyyyMMdd") && x.Code == s.Code).ToList().Count > 0)
                    {
                        _YestodayPrice = _StockList.Where(x => x.Date == _Dt.AddDays(_Yestoday).ToString("yyyyMMdd") && x.Code == s.Code).First().ClosingPrice;
                    }
                    if (_YestodayPrice != "")
                        s.Gain = Convert.ToString(Math.Round(((Convert.ToDouble(s.Change) * 100) / Convert.ToDouble(_YestodayPrice)), 2));
                    if (s.HighestPrice != "" && s.LowestPrice != "" && s.ClosingPrice != "")
                    {
                        double _Diff = Convert.ToDouble(s.HighestPrice) - Convert.ToDouble(s.LowestPrice);
                        s.Shock = Convert.ToString(Math.Round((_Diff * 100) / Convert.ToDouble(s.ClosingPrice), 2));
                    }
                    s.UpdateTime = _UpdatTime;
                    _DataAccess.UpdateStock(s);
                    _No++;
                }
                sw.Stop();
                MessageBox.Show(sw.ElapsedMilliseconds.ToString() + "毫秒,筆數:" + _No);
                _Log = DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " Update Stock Gain ok.\r\n";
                this.txtErrMsg.Text += _Log;
            }
            catch (Exception ex)
            {
                _Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " btnUpdateStockGain_Click:" + ex.Message + "\r\n";
                logger.Error(_Log);
                this.txtErrMsg.Text += _Log;
            }
        }
        /// <summary>
        /// 重置Line通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResetLinePoint_Click(object sender, EventArgs e)
        {
            DataAccess _DataAccess = new DataAccess();
            List<WebApiService.Models.StockLineNotify> _StockLineNotifyList = _DataAccess.getStockLineNotifyList();
            string _Log = "";
            foreach (WebApiService.Models.StockLineNotify s in _StockLineNotifyList.Where(x => x.Enable == true).ToList())
            {
                s.Point = 30;
                s.PointBear = -30;
                _DataAccess.UpdateStockLineNotify(s);
            }
            _Log = DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " reset Line notify ok." + "\r\n";
            this.txtErrMsg.Text += _Log;
        }
        /// <summary>
        /// Line通知選股
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLineGoodStock_Click(object sender, EventArgs e)
        {
            string _Log = "";
            try
            {
                List<Stock> _StockList = getGoodStockList();
                string _Stock = "";
                foreach (Stock s in _StockList)
                {
                    _Stock = _Stock + s.Code + ";";
                }
                List<WebApiService.Models.StockLineNotify> _StockLineNotifyList = _DataAccess.getStockLineNotifyList();
                foreach (WebApiService.Models.StockLineNotify s in _StockLineNotifyList.Where(x => x.NotifyClass == "Good" && x.Enable == true).ToList())
                {
                    string _Msg = "日期:" + this.dateTimePicker1.Text + " 電腦也會選土豆:" + _Stock;
                    CallLineNotifyApi(s.Token, _Msg);
                } 
            }
            catch (Exception ex)
            {
                _Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " LineGoodStock:" + ex.Message + "\r\n";
                logger.Error(_Log);
                this.txtErrMsg.Text += _Log;

            }
        }
        public async void CallLineNotifyApi(string token, string lineMsg)
        {
            await _DataAccess.postLineMsg(lineMsg, token);
        }
        private List<Stock> setStockLackOffList()
        {

            string _Log = "", _Code = "";
            List<Stock> _StockLackOffList = new List<Stock>();
            DataAccess _DataAccess = new DataAccess();
            Stopwatch sw = new Stopwatch();
            sw.Reset();
            sw.Start();
            int _No = 0;

            string _HiStock_URL = ConfigurationManager.AppSettings["HiStock_URL"];
            List<StockGroup> _StockGroupList = getStockGroupList();
            _StockLackOffList = setStockList();
            string _UpdateTime = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
            foreach (StockGroup s in _StockGroupList)
            {
                try
                {
                    if (_StockIdList.IndexOf(s.Code) < 0)
                    {
                        Stock _Stock = new Stock();
                        _Stock.Code = s.Code;
                        _Code = s.Code;
                        // string _YestodayPrice = _DataAccess.getStockByCodeList(s.Code).OrderByDescending(x => x.Date).First().ClosingPrice;
                        _Stock.Name = s.Name;
                        string _TradeVolume = Common.Job.GetTaiwanFutures(_HiStock_URL + s.Code, Volume, true);
                        string _Volume = _TradeVolume.TrimEnd('張');
                        double _VolumeValue = Convert.ToDouble(_Volume) * 1000;
                        _Stock.TradeVolume = _Volume != "0" ? Convert.ToString(_VolumeValue) : "0";
                        _Stock.ClosingPrice = Common.Job.GetTaiwanFutures(_HiStock_URL + s.Code, Id, true);
                        _Stock.TradeValue = _Volume != "0" ? Convert.ToString(_VolumeValue * Convert.ToDouble(_Stock.ClosingPrice)) : "0";
                        string _Change = Common.Job.GetTaiwanFutures(_HiStock_URL + s.Code, Change, true);
                        // _Change = _Change.TrimEnd('%');
                        _Change = _Change.Replace('▲', ' ').Replace('▼', ' ');
                        _Stock.Change = _Change;
                        string _Gain = Common.Job.GetTaiwanFutures(_HiStock_URL + s.Code, Percent, true);
                        _Gain = _Gain.Replace('+', ' ').TrimEnd('%');
                        _Stock.Gain = _Gain;
                        //if (_YestodayPrice != "")
                        //    _Stock.Gain = Convert.ToString(Math.Round(((Convert.ToDouble(_Stock.Change)*100) / Convert.ToDouble(_YestodayPrice)), 2));
                        // _Stock.Date = DateTime.Now.AddDays(_Yestoday).ToString("yyyyMMdd");
                        _Stock.Date = DateTime.Now.ToString("yyyyMMdd");
                        _Stock.UpdateTime = _UpdateTime;
                        _StockLackOffList.Add(_Stock);
                        _No++;

                        MethodInvoker mi = new MethodInvoker(this.UpdateUI);
                        this.BeginInvoke(mi, null);
                    }
                }
                catch (Exception ex)
                {
                    _Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " btnInsertStockLackOff_Click:" + _Code + ex.Message + "\r\n";
                    logger.Error(_Log);
                    this.txtErrMsg.Text += _Log;
                }
            } 
            sw.Stop();
            //MessageBox.Show(sw.ElapsedMilliseconds.ToString() + "毫秒,筆數:" + _No);
            return _StockLackOffList;
        }
        /// <summary>
        /// 根據日期取得證交所資料打算補該日期的股票資料,目前交易過於頻繁會被鎖IP
        /// </summary>
        /// <returns></returns>
        private List<Stock> setStockByDateList()
        {
            DataAccess _DataAccess = new DataAccess();
            DateTime _Dt = Convert.ToDateTime(this.dateTimePicker2.Text);
            System.Globalization.TaiwanCalendar tc = new System.Globalization.TaiwanCalendar();
            string CHDate = tc.GetYear(_Dt).ToString() + "/" + tc.GetMonth(_Dt).ToString().PadLeft(2, '0') + "/" + tc.GetDayOfMonth(_Dt).ToString().PadLeft(2, '0');
            string _WhereDate = _Dt.ToString("yyyyMMdd");
            string _Log = "", _Code = "";
            List<Stock> _StockByDateList = new List<Stock>();
            Stopwatch sw = new Stopwatch();
            sw.Reset();
            sw.Start();
            int _No = 0;

            int _Tuesday = _Dt.DayOfWeek.ToString() == "Tuesday" ? -4 : -2;
            int _Yestoday = _Dt.DayOfWeek.ToString() == "Monday" ? -3 : -1;
            List<Stock> _YestodayStockDayAllList = _DataAccess.getStockYestodayList(_Dt.AddDays(_Yestoday).Date.ToString("yyyyMMdd"));
            List<StockGroup> _StockGroupList = getStockGroupList();
            string _UpdateTime = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
            foreach (StockGroup s in _StockGroupList)
            {
                try
                {
                    string _YestodayPrice = "";
                    if (_YestodayStockDayAllList.Where(x => x.Code == s.Code).ToList().Count > 0)
                        _YestodayPrice = _YestodayStockDayAllList.Where(x => x.Code == s.Code).First().ClosingPrice;
                    StockStrategy.Model.Stock.GetMonthPriceIn _GetMonthPriceIn = new StockStrategy.Model.Stock.GetMonthPriceIn();
                    _GetMonthPriceIn.Sample3_Date = _WhereDate;
                    _GetMonthPriceIn.Sample3_Symbol = s.Code;
                    StockStrategy.Model.Stock.GetMonthPriceOut _GetMonthPriceOutList = StockPrice.GetMonthPrice(_GetMonthPriceIn);
                    StockStrategy.Model.Stock.StockPriceRow _StockPriceRow = _GetMonthPriceOutList.gridList.Where(x => x.date == CHDate).First();
                    Stock _Stock = new Stock();
                    if (_StockPriceRow.high != "" && _StockPriceRow.low != "" && _StockPriceRow.close != "")
                    {
                        double _Diff = Convert.ToDouble(_StockPriceRow.high) - Convert.ToDouble(_StockPriceRow.low);
                        _Stock.Shock = Convert.ToString(Math.Round((_Diff * 100) / Convert.ToDouble(_StockPriceRow.close), 2));
                    }
                    double _dChange = Convert.ToDouble(_StockPriceRow.close) - Convert.ToDouble(_YestodayPrice);
                    _Stock.Change = _dChange.ToString();
                    if (_YestodayPrice != "")
                        _Stock.Gain = Convert.ToString(Math.Round(((Convert.ToDouble(_Stock.Change) * 100) / Convert.ToDouble(_YestodayPrice)), 2));
                    _Stock.Code = s.Code;
                    _Code = s.Code;
                    _Stock.Name = s.Name;
                    string _TradeVolume = _StockPriceRow.volume;
                    double _VolumeValue = Convert.ToDouble(_TradeVolume);
                    _Stock.TradeVolume = _TradeVolume != "0" ? Convert.ToString(_VolumeValue) : "0";
                    _Stock.ClosingPrice = _StockPriceRow.close;
                    _Stock.TradeValue = _TradeVolume != "0" ? Convert.ToString(_VolumeValue * Convert.ToDouble(_Stock.ClosingPrice)) : "0";
                    _Stock.HighestPrice = _StockPriceRow.high;
                    _Stock.LowestPrice = _StockPriceRow.low;
                    _Stock.OpeningPrice = _StockPriceRow.open;
                    _Stock.Date = _WhereDate;
                    _Stock.UpdateTime = _UpdateTime;
                    _StockByDateList.Add(_Stock);
                    _No++;
                    Thread.Sleep(5000);
                    MethodInvoker mi = new MethodInvoker(this.UpdateUI);
                    this.BeginInvoke(mi, null);

                }
                catch (Exception ex)
                {
                    _Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " setStockByDateList:" + _Code + ex.Message + "\r\n";
                    logger.Error(_Log);
                    this.txtErrMsg.Text += _Log;
                }
            }
            sw.Stop();
            MessageBox.Show(sw.ElapsedMilliseconds.ToString() + "毫秒,筆數:" + _No);
            return _StockByDateList;
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            this.lbPercent.Text = "";
            this.progressBar1.Value = 0;
        }

        private void btnInsertStockByDate_Click(object sender, EventArgs e)
        {
            this.lbBtnName.Text = "Insert Stock By Date";
            List<StockGroup> _StockGroupList = getStockGroupList();
            this.progressBar1.Maximum = _StockGroupList.Count;
            this.progressBar1.Step = 1;
            //因為捉取網站資料會較久所以呼叫另一執行緒處理
            var t = new Task(insertStockByDateList);
            t.Start();
        }
        private void insertStockByDateList()
        {
            string _Log = "";
            try
            {
                List<Stock> _StockList = setStockByDateList();
                insertStock(_StockList); 
            }
            catch (Exception ex)
            {
                _Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " insertStockByDateList(:" + ex.Message + "\r\n";
                logger.Error(_Log); 
            }
        }
        /// <summary>
        /// 更新往日三大法人資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStockJurical_Click(object sender, EventArgs e)
        {
            DateTime _Dt = Convert.ToDateTime(this.dateTimePicker2.Text);
            System.Globalization.TaiwanCalendar tc = new System.Globalization.TaiwanCalendar();
            string CHDate = tc.GetYear(_Dt).ToString() + "/" + tc.GetMonth(_Dt).ToString().PadLeft(2, '0') + "/" + tc.GetDayOfMonth(_Dt).ToString().PadLeft(2, '0');
            string _WhereDate = _Dt.ToString("yyyyMMdd");
            List<string> _RecoveryDate = new List<string>();
            _RecoveryDate.Add(_WhereDate);
            //updateStockJuridical(_WhereDate); 
            //            string[] _RecoveryDate = {
            //                "20220527" 
            //            };
            foreach (string s in _RecoveryDate) {
                updateStockJuridical(s);
            }
        }
        /// <summary>
        /// 取得證交所股票三大法人資料
        /// </summary>
        /// <param name="whereDate"></param>
        private void updateStockJuridical(string whereDate) {
            string _Log = "";
            try
            {
                DataAccess _DataAccess = new DataAccess(); 
                List<Stock> _StockList = _DataAccess.getStockYestodayList(whereDate); 
                List<StockStrategy.Model.Stock.StockJuridical> _ListStockJuridical = StockPrice.GetJuridical(whereDate);
                foreach (Stock s in _StockList)
                {
                    StockStrategy.Model.Stock.StockJuridical _StockJuridical = new Model.Stock.StockJuridical();
                    if (_ListStockJuridical.Where(x => x.Code == s.Code && x.Date == s.Date).ToList().Count > 0)
                        _StockJuridical = _ListStockJuridical.Where(x => x.Code == s.Code && x.Date == s.Date).First();
                    s.ForeignInvestment = Convert.ToString(Math.Round(Convert.ToDouble(_StockJuridical.ForeignInvestment) / 1000));
                    s.Investment = Convert.ToString(Math.Round(Convert.ToDouble(_StockJuridical.Investment) / 1000));
                    s.Dealer = Convert.ToString(Math.Round(Convert.ToDouble(_StockJuridical.Dealer) / 1000));
                    _DataAccess.UpdateStock(s);
                }
                _Log = DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " update stock jurical ok.\r\n";
                this.txtErrMsg.Text += _Log;
            }
            catch (Exception ex)
            {
                _Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " btnStockJurical:" + ex.Message + "\r\n";
                logger.Error(_Log);
                this.txtErrMsg.Text += _Log; 
            } 
        } 
        /// <summary>
        /// 加密後寫入config檔
        /// 公鑰:20220801
        /// 私鑰:B1050520
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetting_Click(object sender, EventArgs e)
        {
            string _Account = Tool.Encrypt(this.txtAccount.Text, "20220801", "B1050520");
            string _Password = Tool.Encrypt(this.txtPassword.Text, "20220801", "B1050520");
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            config.AppSettings.Settings["Account"].Value = _Account;
            config.AppSettings.Settings["Password"].Value = _Password ;
            config.Save(ConfigurationSaveMode.Modified);
        }

        private void btnAdminLogin_Click(object sender, EventArgs e)
        {
            string _Username = Tool.Encrypt(this.txtAccount.Text, "20220801", "B1050520");
            string _Password = Tool.Encrypt(this.txtPassword.Text, "20220801", "B1050520");
            if (_Username == ConfigurationManager.AppSettings["Account"] && _Password == ConfigurationManager.AppSettings["Password"]) {
                this.pnAdmin.Enabled = true;
                this.btnLineBad.Enabled = true;
                this.btnLineCtuStock.Enabled = true;
                this.btnLineGoodStock.Enabled = true; 
            }
        }

        private void btnBad_Click(object sender, EventArgs e)
        {
            pgBarBad.Style = ProgressBarStyle.Marquee;
            pgBarBad.MarqueeAnimationSpeed = 30;
            pgBarBad.Show();
            var t = new Task(getBadStock);
            t.Start();
        }

        private void btnCtu_Click(object sender, EventArgs e)
        {
            pgBarCtu.Style = ProgressBarStyle.Marquee;
            pgBarCtu.MarqueeAnimationSpeed = 30;
            pgBarCtu.Show();
            GainType = this.cbGain.Text != "" ? this.cbGain.Text : "漲";
            var t = new Task(getCtuStock);
            t.Start();
        }
        private void getCtuStock()
        {
            string _Log = "";
            CtuStock = "";
            try
            { 
                List<Stock> _StockList = getCtuStockList();

                foreach (Stock s in _StockList)
                {
                    CtuStock = CtuStock + s.Code + ";";
                }
                MethodInvoker mi = new MethodInvoker(this.UpdateUICtuStock);
                this.BeginInvoke(mi, null);

            }
            catch (Exception ex)
            {
                _Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " GetCtuStock:" + ex.Message + "\r\n";
                logger.Error(_Log); 
            }
        }

        /// <summary>
        /// 進度條
        /// </summary>
        private void UpdateUI()
        {
            this.progressBar1.PerformStep();
            List<StockGroup> _StockGroupList = getStockGroupList();
            double _Count = _StockGroupList.Count, _Prog = this.progressBar1.Value;
            this.lbPercent.Text = Math.Floor(((_Prog / _Count) * 100)).ToString() + "%";
            this.lbPercent.Refresh();
        } 
        /// <summary>
        /// 匯入股票清單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            string _Log = "";
            openFileDialog1 = new OpenFileDialog();
            try
            {
                Common.XSLXHelper _XSLXHelper = new Common.XSLXHelper();
                DataTable dt = _XSLXHelper.Import(txtFile.Text, "Data");
                List<StockGroup> _StockGroupList = new List<StockGroup>();
                StockGroup _StockGroup;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _StockGroup = new StockGroup();
                    string[] _Stock = dt.Rows[i][0].ToString().Split('　');
                    _StockGroup.Code = _Stock[0];
                    _StockGroup.Name = _Stock[1];
                    _StockGroup.Class = dt.Rows[i][1].ToString();
                    _StockGroup.StockType = dt.Rows[i][2].ToString() == "上市" ? true : false;
                    _StockGroup.UpdateTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
                    _StockGroupList.Add(_StockGroup);
                }
                btnLogin.PerformClick();
                insertStockGroup(_StockGroupList);
                MessageBox.Show("ok");
            }
            catch (Exception ex)
            {
                _Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " btnImport:" + ex.Message + "\r\n";
                logger.Error(_Log);
                this.txtErrMsg.Text += _Log;
            }
        } 
        /// <summary>
        /// 更新股票指數
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateStockIndex_Click(object sender, EventArgs e)
        {
            string _Log = "";
            try
            {
                this.btnLogin.PerformClick();
                List<StockStrategy.Model.Stock.GetRealtimeStockPrice> _PriceList = new List<StockStrategy.Model.Stock.GetRealtimeStockPrice>();
                StockStrategy.Model.Stock.GetRealtimePriceIn _RealtimePriceIn = new StockStrategy.Model.Stock.GetRealtimePriceIn();
                _RealtimePriceIn.Sample1_Symbol = "t00";
                if (_RealtimePriceIn.Sample1_Symbol != null)
                {
                    _PriceList = StockStrategy.BBL.StockPrice.GetRealtimePrice(_RealtimePriceIn);
                }
                StockStrategy.Model.Stock.GetRealtimeStockPrice _StockPrice = new Model.Stock.GetRealtimeStockPrice();
                if (_PriceList.Count() > 0) _StockPrice = _PriceList.First();
                string _Id = "Price1_lbTPrice";
                string _Change = "Price1_lbTChange";
                string _Percent = "Price1_lbTPercent";
                string _Volume = "Price1_lbTVolume";
                string _MTX_URL = ConfigurationManager.AppSettings["MTX_URL"];
                string _TPEx_URL = ConfigurationManager.AppSettings["TPEx_URL"];
                StockIndex s = new StockIndex();
                List<StockIndex> _ListStock = getStockIndexList();
                s = _ListStock.OrderByDescending(x => x.Date).First();
                //s.Id = _ListStock.OrderByDescending(x => x.Date).First().Id;
                s.ContinueName = "";
                s.TX = Common.Job.GetTaiwanFutures(_MTX_URL, _Id, true);
                s.TX_High = "";
                s.TX_Open = "";
                s.TX_QuoteChange = Common.Job.GetTaiwanFutures(_MTX_URL, _Change, true);
                s.TX_QuotePercent = Common.Job.GetTaiwanFutures(_MTX_URL, _Percent, true);
                s.TX_Volume = Common.Job.GetTaiwanFutures(_MTX_URL, _Volume, true);
                int _Yestoday = DateTime.Now.DayOfWeek.ToString() == "Monday" ? -3 : -1;
                // string _TopIndex = getTopIndex(DateTime.Now.AddDays(_Yestoday).ToString("yyyyMMdd"));
                string _TopIndex = _ListStock.Where(x => x.Date == DateTime.Now.AddDays(_Yestoday).ToString("yyyyMMdd")).First().TAIEX;
                s.TAIEX_High = _StockPrice.HighPrice;
                s.TAIEX_Open = _StockPrice.OpenPrice;
                s.TAIEX_QuoteChange = Convert.ToString(Math.Round(Convert.ToDouble(_StockPrice.Price) - Convert.ToDouble(_TopIndex), 2));
                s.TAIEX_QuotePercent = Convert.ToString(Math.Round(Convert.ToDouble(s.TAIEX_QuoteChange) * 100 / Convert.ToDouble(_StockPrice.Price), 2));
                s.TAIEX = s.TAIEX_QuoteChange != "0" ? _StockPrice.Price : "0";
                s.Volume = _StockPrice.TotalDealQty;
                s.TPEx = Common.Job.GetTaiwanFutures(_TPEx_URL, _Id, true);
                s.TPEx_QuoteChange = Common.Job.GetTaiwanFutures(_TPEx_URL, _Change, true);
                s.TPEx_QuotePercent = Common.Job.GetTaiwanFutures(_TPEx_URL, _Percent, true);

                if (s.TAIEX_QuoteChange == "0")
                {
                    s.TAIEX = "0";
                    s.TPEx = "0";
                    s.TX = "0";
                }
                updateStockIndex(s);
                _Log = DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " update stock index ok.";
                this.txtErrMsg.Text += _Log;
                bTAIEX = true;
            }
            catch (Exception ex)
            {
                _Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " GetIndexToInsert:" + ex.Message + "\r\n";
                logger.Error(_Log);
                this.txtErrMsg.Text += _Log;
            }
        }
        /// <summary>
        /// 取得股票
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetStock_Click(object sender, EventArgs e)
        {
            List<Stock> _ListStock = new List<Stock>();
            string _Action = "Stock";
            string _Uri = ConnectionString + _Action;
            ApiResultEntity _ApiResult = CallWebApi.Get(_Uri, Token);
            _ListStock = JsonConvert.DeserializeObject<List<Stock>>(_ApiResult.Data.ToString());
        }
    }
}
