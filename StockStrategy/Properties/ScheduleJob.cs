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
            string _Username = ConfigurationManager.AppSettings["Account"];
            string _Password = ConfigurationManager.AppSettings["Password"];
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
            ConnectionString = ConfigurationManager.AppSettings["ApiServer"];
            loginWebApi();
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
                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " GetIndexToInsert:" + ex.Message;
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
            if (_Hour == "8" && _Minute == "0")
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
                if (_Hour == "7" && _Minute == "10" && !stockIndex)
                {
                    if (!bStockAll)
                    {
                        bStockAll = true;
                        btnGetStockPrice.PerformClick();
                    }
                }
                if (_Hour == "8" && _Minute == "10" && !stockIndex)
                {
                    btnGetIndexToInsert.PerformClick();
                }
                //if (_Hour == "14" && _Minute == "0" && !bUpdateStockIndex)
                //{
                //    btnUpdateStockIndex.PerformClick();
                //}
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
                    if (!bUpdateStockLineNotify)
                    {
                        bUpdateStockLineNotify = true;
                        btnResetLinePoint.PerformClick();
                    }
                }
            }
        }
        /// <summary>
        /// 當天會先桌取HiStock股價資訊存到資料庫
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
                _Log = "\r\n" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " insert stock  ok.";
                this.txtErrMsg.Text += _Log;
            }
            catch (Exception ex)
            {
                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " GetStockPrice:" + ex.Message;
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
                    _Stock.UpdateTime = DateTime.Now.ToString("yyyyMMdd hh:mm:ss");
                    _StockList.Add(_Stock);
                    //  _StockIdList.Add(s.Code);
                }
            }
            catch (Exception ex)
            {
                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " setStockList:" + ex.Message;
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
            List<Stock> _YestodayStockDayAllList = _DataAccess.getStockYestodayList(_Dt.AddDays(_Yestoday).Date.ToString("yyyyMMdd"));
            List<Stock> _StockDayAllList = _DataAccess.getStockYestodayList(_WhereDate);
            List<Stock> _StockList = new List<Stock>();
            try
            {
                List<Stock> _GoodStockList = _StockDayAllList.Where(x => Convert.ToDouble(x.Gain) > 3 && Convert.ToDouble(x.TradeVolume) > 700000).ToList();
                foreach (Stock s in _GoodStockList)
                {
                    if (_YestodayStockDayAllList.Where(x => x.Code == s.Code).ToList().Count > 0)
                    {
                        Stock _YestodayStock = _YestodayStockDayAllList.Where(x => x.Code == s.Code).First();
                        double _Multiple = Convert.ToDouble(s.TradeVolume) / Convert.ToDouble(_YestodayStock.TradeVolume);
                        if (_Multiple > 5)
                        {
                            _StockList.Add(s);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " getGoodStockList:" + ex.Message;
                logger.Error(_Log);
                this.txtErrMsg.Text += _Log;
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
                    _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " btnOpen_Click:" + ex.Message;
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
            //因為桌取網站資料會較久所以呼叫另一執行續處理
            var t = new Task(insertStockLackOffList);
            t.Start();
        }
        private void insertStockLackOffList() {
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
              //  this.txtErrMsg.Text += _Log;
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
                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " btnUpdateJuridicaPerson:" + ex.Message;
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
                _Log = "\r\n" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " update stock juridica ok.";
                this.txtErrMsg.Text += _Log;
                bTAIEX = true;
            }
            catch (Exception ex)
            {
                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " btnUpdateJuridicaPerson:" + ex.Message;
                logger.Error(_Log);
                this.txtErrMsg.Text += _Log;
            }
        }

        private void btnGetGoodStock_Click(object sender, EventArgs e)
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
                this.txtGoodStock.Text = _Stock;
            }
            catch (Exception ex)
            {
                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " GetGoodStock:" + ex.Message;
                logger.Error(_Log);
                this.txtErrMsg.Text += _Log;

            }
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
                    // Stock _Stock = new Stock();
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
            _Log = "\r\n" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " reset Line notify ok.";
            this.txtErrMsg.Text += _Log;
        }

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
                // this.txtGoodStock.Text = _Stock;
            }
            catch (Exception ex)
            {
                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " LineGoodStock:" + ex.Message;
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

            string _Log = "";
            List<Stock> _StockLackOffList = new List<Stock>();
            DataAccess _DataAccess = new DataAccess();
            Stopwatch sw = new Stopwatch();
            sw.Reset();
            sw.Start();
            int _No = 0;
            try
            {
                string _HiStock_URL = ConfigurationManager.AppSettings["HiStock_URL"];
                List<StockGroup> _StockGroupList = getStockGroupList();
                _StockLackOffList = setStockList();  
                foreach (StockGroup s in _StockGroupList)
                {
                    if (_StockIdList.IndexOf(s.Code) < 0)
                    {
                        Stock _Stock = new Stock();
                        _Stock.Code = s.Code;
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
                        _Stock.UpdateTime = DateTime.Now.ToString("yyyyMMdd hh:mm:ss");
                        _StockLackOffList.Add(_Stock);
                        _No++;

                        MethodInvoker mi = new MethodInvoker(this.UpdateUI);
                        this.BeginInvoke(mi, null); 
                    }
                }
            }
            catch (Exception ex)
            {
                _Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " btnInsertStockLackOff_Click:" + ex.Message + "\r\n";
                logger.Error(_Log);
                this.txtErrMsg.Text += _Log;
            }
            sw.Stop();
            //MessageBox.Show(sw.ElapsedMilliseconds.ToString() + "毫秒,筆數:" + _No);
            return _StockLackOffList;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.lbPercent.Text = "";
            this.progressBar1.Value = 0;
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
        private List<Stock> setStockGainList()
        {
            string _Log = "";
            DataAccess _DataAccess = new DataAccess();
            DateTime _Dt = Convert.ToDateTime(this.dateTimePicker1.Text);
            string _WhereDate = _Dt.ToString("yyyyMMdd");
            string _DayOfWeek = _Dt.DayOfWeek.ToString();
            int _Yestoday = _DayOfWeek == "Monday" ? -3 : -1;
            List<Stock> _YestodayStockDayAllList = _DataAccess.getStockYestodayList(_Dt.AddDays(_Yestoday).Date.ToString("yyyyMMdd"));
            List<Stock> _StockDayAllList = _DataAccess.getStockYestodayList(_WhereDate);
            List<Stock> _StockList = new List<Stock>();
            try
            {

                //foreach (Stock s in _GoodStockList)
                //{


                //}
            }
            catch (Exception ex)
            {
                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " getGoodStockList:" + ex.Message;
                logger.Error(_Log);
                this.txtErrMsg.Text += _Log;
            }
            return _StockList;
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
                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " btnImport:" + ex.Message;
                logger.Error(_Log);
                this.txtErrMsg.Text += _Log;
            }
        }
        /// <summary>
        /// 寫入股票庫存測試
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInsertStockInventory_Click(object sender, EventArgs e)
        {
            string _Log = "";
            try
            {
                this.btnLogin.PerformClick();
                StockInventory s = new StockInventory();
                s.Name = "a";
                s.Qty = 1;
                s.Status = "買";
                s.Target = "80";
                s.Defence = "20";
                s.CloseABSStopLost = true;
                s.CloseHighStopLost = true;
                s.ClosePtLost = true;
                s.CloseStopLost = true;
                s.Code = "1264";
                s.Cost = "12455";
                s.Type = true;//1:future 0:stock
                insertStockInventory(s);
                _Log = DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " insert stock index ok.\r\n";
                this.txtErrMsg.Text += _Log;
            }
            catch (Exception ex)
            {
                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " InsertStockInventory:" + ex.Message;
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
                _Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " GetIndexToInsert:" + ex.Message;
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
