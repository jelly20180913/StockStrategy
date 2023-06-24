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
using DocumentFormat.OpenXml.Spreadsheet;
using NLog.Fluent;
using DataModel.Common;
using DocumentFormat.OpenXml.Wordprocessing;
using static ClosedXML.Excel.XLPredefinedFormat;
using DateTime = System.DateTime;
using System.IO;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
//using DataModel.Stock;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
//using DocumentFormat.OpenXml.Drawing.Charts;

namespace StockStrategy
{
	public partial class ScheduleJob : Form
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		string ConnectionString = "", Token = "";
		List<string> _StockIdList = new List<string>();
		bool stockIndex = false, bTAIEX = false, bStockAll = false, bStockAllLackOff = false, bUpdateStockLineNotify = false;
		string Id = "Price1_lbTPrice";
		string Change = "Price1_lbTChange";
		string Percent = "Price1_lbTPercent";
		string Volume = "Price1_lbTVolume";
		string GoodStock = "", CtuStock = "", GainType = "", BadStock = "";
		DataAccess _DataAccess = new DataAccess();
		private bool bGoodStockLineNotify = false, bGoodStockByJsonLineNotify = false,bStockGroupTrend=false,bStockThreeInstitutional=false;
		private bool bBadStockLineNotify;
		List<Holiday> HolidayList = new List<Holiday>();
		Dictionary<int, string> GoodStockClass = new Dictionary<int, string>() { };
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
			string _Username = Tool.Decrypt(ConfigurationManager.AppSettings["Account"], "20220801", "B1050520");
			string _Password = Tool.Decrypt(ConfigurationManager.AppSettings["Password"], "20220801", "B1050520");
			LoginData _LoginData = new LoginData();
			_LoginData.Username = _Username;
			_LoginData.Password = _Password;
			string _Action = "Token";
			string json = JsonConvert.SerializeObject(_LoginData);
			string _Uri = ConnectionString + _Action;
			Token = CallWebApi.Login(json, _Uri);
			string _Log = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " user login.\r\n";
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
			var _Ip = Tool.GetIpAddresses();
			if (_Ip.Length > 0)
			{
				this.toolStripStatusLabelIP.Text = _Ip[_Ip.Length - 1].ToString();
			}
			btnAdminLogin.PerformClick();
			// loginWebApi(); 
			string _Date = DateTime.Now.ToString("yyyy");
			HolidayList = _DataAccess.getHolidayList().Where(x => x.Enable == true).Where(x => x.Year == _Date || x.HolidayDate == "0000").ToList();
			GoodStockClass.Add(1, "起漲放量");
			GoodStockClass.Add(5, "起漲放量");
			GoodStockClass.Add(10, "十倍放量");
			GoodStockClass.Add(6, "盤月放量");
			GoodStockClass.Add(7, "盤季放量");
			GoodStockClass.Add(8, "半年放量");
			GoodStockClass.Add(9, "盤年放量");
			GoodStockClass.Add(11, "萬張放量");
			this.cbApprove.SelectedIndex = 0; 
			this.ContextMenuStrip = PopupMenu; 
			PopupMenu.Show();
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
				DataAccess _DataAccess = new DataAccess();

				this.btnLogin.PerformClick();
				string _Id = "Price1_lbTPrice";
				string _Change = "Price1_lbTChange";
				string _Percent = "Price1_lbTPercent";
				string _Volume = "Price1_lbTVolume";
				List<StockStrategy.Model.Stock.GetRealtimeStockPrice> _PriceList = new List<StockStrategy.Model.Stock.GetRealtimeStockPrice>();
				StockStrategy.Model.Stock.GetRealtimePriceIn _RealtimePriceIn = new StockStrategy.Model.Stock.GetRealtimePriceIn();
				_RealtimePriceIn.Sample1_Symbol = "t00";
				string _PHLX_URL = ConfigurationManager.AppSettings["PHLX_URL"];
				string _DJI_URL = ConfigurationManager.AppSettings["DJI_URL"];
				string _NASDAQ_URL = ConfigurationManager.AppSettings["NASDAQ_URL"];
				string _MTX_URL = ConfigurationManager.AppSettings["MTX_URL"];
				string _TX_URL = ConfigurationManager.AppSettings["TX_URL"];
				string _TPEx_URL = ConfigurationManager.AppSettings["TPEx_URL"];
				string _HiStockIndex_URL = ConfigurationManager.AppSettings["HiStock_URL"];
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
				//DateTime _Dt = DateTime.Now;
				DateTime _Dt = Convert.ToDateTime(this.dtpStockIndex.Text);
				int _Yestoday = _Dt.DayOfWeek.ToString() == "Monday" ? -3 : -1;
				DateTime _YestodayDate = skipHoliday(_Dt.AddDays(_Yestoday));
				//  _Yestoday = -4;
				// string _TopIndex = getTopIndex(DateTime.Now.AddDays(_Yestoday).ToString("yyyyMMdd"));
				List<StockIndex> _ListStockAll = _DataAccess.getStockIndexList();
				string _TopIndex = _ListStockAll.Where(x => x.Date == _YestodayDate.ToString("yyyyMMdd")).First().TAIEX;
				string _TopIndexDJI = _ListStockAll.Where(x => x.Date == _YestodayDate.ToString("yyyyMMdd")).First().DJI;
				string _TopIndexOIL = _ListStockAll.Where(x => x.Date == _YestodayDate.ToString("yyyyMMdd")).First().OIL_Index;
				string _USD_Index = Common.Job.GetTaiwanFutures(_HiStockIndex_URL + "USDX", _Id, true);
				string _USD_IndexQuote = Common.Job.GetTaiwanFutures(_HiStockIndex_URL + "USDX", _Change, true).Replace('▼', ' ').Replace('▲', '+');
				string _USD_IndexPercent = Common.Job.GetTaiwanFutures(_HiStockIndex_URL + "USDX", _Percent, true);

				string _OIL_Index = Common.Job.GetTaiwanFutures(_HiStockIndex_URL + "N1CL", _Id, true);
				string _OIL_IndexQuote = Convert.ToString(Convert.ToDecimal(_OIL_Index) - Convert.ToDecimal(_TopIndexOIL));
				//string _OIL_IndexQuote = Common.Job.GetTaiwanFutures(_HiStockIndex_URL + "N1CL", _Change, true).Replace('▼', ' ').Replace('▲', '+');
				string _BTC_Index = Common.Job.GetTaiwanFutures(_HiStockIndex_URL + "BTC", _Id, true);
				string _BTC_IndexQuote = Common.Job.GetTaiwanFutures(_HiStockIndex_URL + "BTC", _Change, true).Replace('▼', ' ').Replace('▲', '+');
				string _Gold_Index = Common.Job.GetTaiwanFutures(_HiStockIndex_URL + "GOLD", _Id, true);
				string _Gold_IndexQuote = Common.Job.GetTaiwanFutures(_HiStockIndex_URL + "GOLD", _Change, true).Replace('▼', ' ').Replace('▲', '+');

				string _VHSI_Index = Common.Job.GetTaiwanFutures(_HiStockIndex_URL + "VHSI", _Id, true);
				string _VHSI_IndexQuote = Common.Job.GetTaiwanFutures(_HiStockIndex_URL + "VHSI", _Change, true).Replace('▼', ' ').Replace('▲', '+');
				string _DAX_Index = Common.Job.GetTaiwanFutures(_HiStockIndex_URL + "DAX", _Id, true);
				string _DAX_IndexQuote = Common.Job.GetTaiwanFutures(_HiStockIndex_URL + "DAX", _Change, true).Replace('▼', ' ').Replace('▲', '+');
				string _NIKKEI_Index = Common.Job.GetTaiwanFutures(_HiStockIndex_URL + "NIKKEI", _Id, true);
				string _NIKKEI_IndexQuote = Common.Job.GetTaiwanFutures(_HiStockIndex_URL + "NIKKEI", _Change, true).Replace('▼', ' ').Replace('▲', '+');
				List<StockIndex> _ListStock = new List<StockIndex>();
				StockIndex s = new StockIndex();
				s.ContinueName = "";
				s.DJI_QuotePercent = Common.Job.GetTaiwanFutures(_DJI_URL, _Volume, true);
				s.Date = _Dt.ToString("yyyyMMdd");
				s.DayOfWeek = Mapping.DayOfWeekList.Where(x => x.Day == _Dt.DayOfWeek.ToString()).First().Show;
				s.DJI = _DJI_Index;
				s.DJI_QuoteChange = Common.Job.GetTaiwanFutures(_DJI_URL, _Change, true).Replace('▼', ' ').Replace('▲', '+');
				s.DJI_QuotePercent = Common.Job.GetTaiwanFutures(_DJI_URL, _Percent, true);
				s.MTX = Common.Job.GetTaiwanFutures(_MTX_URL, _Id, true);
				s.MTX_High = "";
				s.MTX_Open = "";
				s.MTX_QuoteChange = Common.Job.GetTaiwanFutures(_MTX_URL, _Change, true).Replace('▼', '-').Replace('▲', '+');
				s.MTX_QuotePercent = Common.Job.GetTaiwanFutures(_MTX_URL, _Percent, true);
				s.MTX_Volume = Common.Job.GetTaiwanFutures(_MTX_URL, _Volume, true).Replace('口', ' ');
				s.NASDAQ = Common.Job.GetTaiwanFutures(_NASDAQ_URL, _Id, true);
				s.NASDAQ_QuoteChange = Common.Job.GetTaiwanFutures(_NASDAQ_URL, _Change, true).Replace('▼', ' ').Replace('▲', '+');
				s.NASDAQ_QuotePercent = Common.Job.GetTaiwanFutures(_NASDAQ_URL, _Percent, true);
				s.PHLX = Common.Job.GetTaiwanFutures(_PHLX_URL, _Id, true);
				s.PHLX_QuoteChange = Common.Job.GetTaiwanFutures(_PHLX_URL, _Change, true).Replace('▼', ' ').Replace('▲', '+');
				s.PHLX_QuotePercent = Common.Job.GetTaiwanFutures(_PHLX_URL, _Percent, true);
				s.TX = Common.Job.GetTaiwanFutures(_MTX_URL, _Id, true);
				s.TX_High = "";
				s.TX_Open = "";
				s.TX_QuoteChange = Common.Job.GetTaiwanFutures(_MTX_URL, _Change, true).Replace('▼', '-').Replace('▲', '+');
				s.TX_QuotePercent = Common.Job.GetTaiwanFutures(_MTX_URL, _Percent, true);
				s.TX_Volume = Common.Job.GetTaiwanFutures(_MTX_URL, _Volume, true).Replace('口', ' ');
				s.TAIEX = _StockPrice.Price;
				s.TAIEX_High = _StockPrice.HighPrice;
				s.TAIEX_Open = _StockPrice.OpenPrice;
				s.TAIEX_QuoteChange = Convert.ToString(Math.Round(Convert.ToDouble(_StockPrice.Price) - Convert.ToDouble(_TopIndex), 2));
				s.TAIEX_QuotePercent = Convert.ToString(Math.Round(Convert.ToDouble(s.TAIEX_QuoteChange) * 100 / Convert.ToDouble(_StockPrice.Price), 2));
				s.Volume = _StockPrice.TotalDealQty;
				s.TPEx = Common.Job.GetTaiwanFutures(_TPEx_URL, _Id, true);
				s.TPEx_QuoteChange = Common.Job.GetTaiwanFutures(_TPEx_URL, _Change, true).Replace('▼', '-').Replace('▲', '+');
				s.TPEx_QuotePercent = Common.Job.GetTaiwanFutures(_TPEx_URL, _Percent, true);
				s.USD_Index = _USD_Index;
				s.USD_IndexQuoteChange = _USD_IndexQuote;
				s.USD_IndexQuotePercent = _USD_IndexPercent;
				if (_TopIndexDJI == _DJI_Index)
				{
					s.DJI = "0";
					s.NASDAQ = "0";
					s.PHLX = "0";
				}
				s.OIL_Index = _OIL_Index;
				s.OIL_QuoteChange = _OIL_IndexQuote;
				s.BTC_Index = _BTC_Index;
				s.BTC_QuoteChange = _BTC_IndexQuote;
				s.Gold_Index = _Gold_Index;
				s.Gold_QuoteChange = _Gold_IndexQuote;
				s.DAX_Index = _DAX_Index;
				s.DAX_QuoteChange = _DAX_IndexQuote;
				s.NIKKEI_Index = _NIKKEI_Index;
				s.NIKKEI_QuoteChange = _NIKKEI_IndexQuote;
				s.VHSI_Index = _VHSI_Index;
				s.VHSI_QuoteChange = _VHSI_IndexQuote;
				_ListStock.Add(s);
				_DataAccess.InsertStockIndex(_ListStock);
				stockIndex = true;
				_Log = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " insert stock index ok.\r\n";
				this.txtErrMsg.Text += _Log;
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " GetIndexToInsert:" + ex.Message + "\r\n";
				logger.Error(_Log);
				this.txtErrMsg.Text += _Log;
			}
		}
		/// <summary>
		/// 取得加權指數
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		//private void btnGetTopIndex_Click(object sender, EventArgs e)
		//{
		//    this.btnLogin.PerformClick();
		//    DataAccess _DataAccess = new DataAccess();
		//    List<StockIndex> _ListStock = _DataAccess.getStockIndexList();
		//}
		/// <summary>
		/// 取得前一日的指數
		/// </summary>
		/// <param name="yestoday"></param>
		/// <returns></returns>
		private string getTopIndex(string yestoday)
		{
			DataAccess _DataAccess = new DataAccess();
			List<StockIndex> _ListStock = _DataAccess.getStockIndexList();
			return _ListStock.Where(x => x.Date == yestoday).First().TAIEX;
		}
		private string getDJITopIndex(string yestoday)
		{
			DataAccess _DataAccess = new DataAccess();
			List<StockIndex> _ListStock = _DataAccess.getStockIndexList();
			return _ListStock.Where(x => x.Date == yestoday).First().DJI;
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
			if (_Hour == "7" && _Minute == "30")
			{
				stockIndex = false;
				bTAIEX = false;
				bStockAll = false;
				bStockAllLackOff = false;
				bUpdateStockLineNotify = false;
				bGoodStockLineNotify = false;
				bBadStockLineNotify = false;
				bGoodStockByJsonLineNotify = false;
				bStockGroupTrend = false;
				bStockThreeInstitutional = false;
				this.dtpGoodDate.Value = DateTime.Now;
				this.dtpBadDate.Value = DateTime.Now.AddDays(-1);
				this.dtpCtuDate.Value = DateTime.Now;
				this.dtpStockIndex.Value = DateTime.Now;
				this.dTPReport.Value = DateTime.Now;
				this.dtpStockYesterday.Value = DateTime.Now;
			}
			if (_DayOfWeek != "Sunday" && _DayOfWeek != "Saturday")
			{
				if (_Hour == "8" && _Minute == "35")
				{
					if (!bStockAll)
					{
						bStockAll = true;
						btnGetStockPrice.PerformClick();
						btnClear.PerformClick();
						btnStockJurical.PerformClick();

					}
					if (!bUpdateStockLineNotify)
					{
						btnResetLinePoint.PerformClick();
					}

				}
				if (_Hour == "8" && _Minute == "40")
				{
					if (!bStockThreeInstitutional)
					{
						bStockThreeInstitutional = true;
						btnLineThree.PerformClick(); 
					} 
				}
				if (_Hour == "8" && _Minute == "45")
				{
					if (!bBadStockLineNotify)
					{
						bBadStockLineNotify = true;
						btnLineBad.PerformClick();
					}
				}
				if (_Hour == "7" && _Minute == "35" && !stockIndex)
				{
					btnGetIndexToInsert.PerformClick();
					btnLineStockIndex.PerformClick();
				}

				if (_Hour == "13" && _Minute == "45")
				{
					if (!bTAIEX)
						btnUpdateStockIndex.PerformClick();
				}
				if (_Hour == "13" && _Minute == "55")
				{
					bUpdateStockLineNotify = false;
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
				if (_Hour == "18" && _Minute == "10")
				{
					if (!bGoodStockLineNotify)
					{
						bGoodStockLineNotify = true;
						//	btnLineGoodStock.PerformClick();
						btnLineCtuStock.PerformClick();
						btnLineCtuRed.PerformClick();
						btnStockResult.PerformClick();
						btnGetGoodStockByJson.PerformClick();
						btnInsertGroupTrend.PerformClick();
					}

				}
				if (_Hour == "18" && _Minute == "15")
				{
					if (!bGoodStockByJsonLineNotify)
					{
						bGoodStockByJsonLineNotify = true;
						btnLineGoodbyJson.PerformClick();
					}
				}
				if (_Hour == "18" && _Minute == "20")
				{
					if (!bStockGroupTrend)
					{
						bStockGroupTrend = true; 
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
				DateTime _Dt = Convert.ToDateTime(this.dtpStockIndex.Text);
				int _Yestoday = DateTime.Now.DayOfWeek.ToString() == "Monday" ? -3 : -1;
				DateTime _YestodayDate = skipHoliday(_Dt.AddDays(_Yestoday));

				//List<DataModel.Stock.Stock> _YestodayStockDayAllList = _DataAccess.getStockBySqlList(_YestodayDate.Date.ToString("yyyyMMdd"), "Date");
				List<Stock> _YestodayStockDayAllList = _DataAccess.getStockYestodayList(_YestodayDate.Date.ToString("yyyyMMdd"));
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
				_DataAccess.insertStock(_StockAddList);
				_Log = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " insert stock  ok.\r\n";
				this.txtErrMsg.Text += _Log;
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " GetStockPrice:" + ex.Message + "\r\n";
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
			DateTime _Dt = Convert.ToDateTime(this.dtpStockIndex.Text);
			int _Tuesday = DateTime.Now.DayOfWeek.ToString() == "Tuesday" ? -4 : -2;
			int _Yestoday = DateTime.Now.DayOfWeek.ToString() == "Monday" ? -3 : -1;
			DateTime _TuesdayDate = skipHoliday(_Dt.AddDays(_Tuesday));
			DateTime _YestodayDate = skipHoliday(_Dt.AddDays(_Yestoday));
			List<Stock> _YestodayStockDayAllList = _DataAccess.getStockYestodayList(_TuesdayDate.Date.ToString("yyyyMMdd"));
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
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " setStockList:" + ex.Message + "\r\n";
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
		private List<DataModel.Stock.Stock> getGoodStockList()
		{
			string _Log = "";
			DataAccess _DataAccess = new DataAccess();
			DateTime _Dt = Convert.ToDateTime(this.dtpGoodDate.Text);
			string _Code = "";
			string _WhereDate = _Dt.ToString("yyyyMMdd");
			string _DayOfWeek = _Dt.DayOfWeek.ToString();
			int _Yestoday = _DayOfWeek == "Monday" ? -3 : -1;
			DateTime _YestodayDate = skipHoliday(_Dt.AddDays(_Yestoday));

			List<DataModel.Stock.Stock> _YestodayStockDayAllList = _DataAccess.getStockBySqlList(_YestodayDate.Date.ToString("yyyyMMdd"), "Date");
			List<DataModel.Stock.Stock> _StockDayAllList = _DataAccess.getStockBySqlList(_WhereDate, "Date");
			List<DataModel.Stock.Stock> _StockList = new List<DataModel.Stock.Stock>();
			decimal _Gain = this.nupGain.Value != 0 ? this.nupGain.Value : 3;
			int _Volumn = this.txtVolumn.Text != "" ? Convert.ToInt32(this.txtVolumn.Text) * 1000 : 700000;
			int multiple = this.txtMultiple.Text != "" ? Convert.ToInt32(this.txtMultiple.Text) : 5;
			int _PreDays = this.txtPreDays.Text != "" ? Convert.ToInt32(this.txtPreDays.Text) : 10;
			int _LessVolumn = this.txtLessVolumn.Text != "" ? Convert.ToInt32(this.txtLessVolumn.Text) * 1000 : 1000000;
			int _PreDays2 = this.txtPreDays2.Text != "" ? Convert.ToInt32(this.txtPreDays2.Text) : 10;
			int _MoreGain = this.txtMoreGain.Text != "" ? Convert.ToInt32(this.txtMoreGain.Text) : 10;
			try
			{
				List<DataModel.Stock.Stock> _GoodStockList = _StockDayAllList.Where(x => Convert.ToDecimal(x.Gain) > _Gain && Convert.ToDouble(x.TradeVolume) > _Volumn).ToList();
				foreach (DataModel.Stock.Stock s in _GoodStockList)
				{
					_Code = s.Code;
					if (_YestodayStockDayAllList.Where(x => x.Code == s.Code).ToList().Count > 0)
					{
						DataModel.Stock.Stock _YestodayStock = _YestodayStockDayAllList.Where(x => x.Code == s.Code).First();
						double _Multiple = Convert.ToDouble(s.TradeVolume) / Convert.ToDouble(_YestodayStock.TradeVolume);
						if (_YestodayStock.TradeVolume != "0")
						{
							if (_Multiple > multiple)
							{
								bool _Less = true, _NotHigherThan = true;
								List<DataModel.Stock.Stock> _StockListByCode = new List<DataModel.Stock.Stock>();
								if (chkLess.Checked || chkNotHigherThan.Checked)
								{
									_StockListByCode = _DataAccess.getStockBySqlList(s.Code, "Code").Where(x => Convert.ToInt32(x.Date) <= Convert.ToInt32(_WhereDate)).OrderByDescending(x => x.Date).ToList();
									if (chkLess.Checked)
									{
										_Less = _StockListByCode.Take(_PreDays + 1).Where(x => Convert.ToDouble(x.TradeVolume) < _LessVolumn).ToList().Count >= _PreDays;
									}
									if (chkNotHigherThan.Checked)
									{
										double _Price = Convert.ToDouble(s.ClosingPrice) + Convert.ToDouble(s.ClosingPrice) * _MoreGain / 100;
										_NotHigherThan = _StockListByCode.Take(_PreDays2).Where(x => Convert.ToDouble(x.ClosingPrice) > _Price).ToList().Count == 0;
									}
								}
								if (_Less && _NotHigherThan) _StockList.Add(s);
							}
						}
					}

				}
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + _Code + " getGoodStockList:" + ex.Message + "\r\n";
				logger.Error(_Log);
			}
			return _StockList;
		}

		private List<DataModel.Stock.Stock> getGoodStockList(StockStrategy.Model.GoodStock goodStock)
		{
			string _Log = "";
			DataAccess _DataAccess = new DataAccess();
			DateTime _Dt = Convert.ToDateTime(this.dtpGoodDate.Text);
			string _Code = "";
			string _WhereDate = _Dt.ToString("yyyyMMdd");
			string _DayOfWeek = _Dt.DayOfWeek.ToString();
			int _Yestoday = _DayOfWeek == "Monday" ? -3 : -1;
			DateTime _YestodayDate = skipHoliday(_Dt.AddDays(_Yestoday));

			List<DataModel.Stock.Stock> _YestodayStockDayAllList = _DataAccess.getStockBySqlList(_YestodayDate.Date.ToString("yyyyMMdd"), "Date");
			List<DataModel.Stock.Stock> _StockDayAllList = _DataAccess.getStockBySqlList(_WhereDate, "Date");
			List<DataModel.Stock.Stock> _StockList = new List<DataModel.Stock.Stock>();
			decimal _Gain = this.nupGain.Value != 0 ? this.nupGain.Value : 3;
			int _Volumn = this.txtVolumn.Text != "" ? Convert.ToInt32(this.txtVolumn.Text) * 1000 : 700000;
			int multiple = this.txtMultiple.Text != "" ? Convert.ToInt32(this.txtMultiple.Text) : 5;
			int _PreDays = this.txtPreDays.Text != "" ? Convert.ToInt32(this.txtPreDays.Text) : 10;
			int _LessVolumn = this.txtLessVolumn.Text != "" ? Convert.ToInt32(this.txtLessVolumn.Text) * 1000 : 1000000;
			int _PreDays2 = this.txtPreDays2.Text != "" ? Convert.ToInt32(this.txtPreDays2.Text) : 10;
			int _MoreGain = this.txtMoreGain.Text != "" ? Convert.ToInt32(this.txtMoreGain.Text) : 10;
			bool _ChkLess = chkLess.Checked;
			bool _ChkNotHigherThan = chkNotHigherThan.Checked;
			if (goodStock.Name != null)
			{
				_Gain = goodStock.Gain;
				_Volumn = goodStock.Volumn;
				multiple = goodStock.Multiple;
				_PreDays = goodStock.PreDays;
				_LessVolumn = goodStock.LessVolumn;
				_PreDays2 = goodStock.PreDays2;
				_MoreGain = goodStock.MoreGain;
				_ChkLess = goodStock.ChkLess;
				_ChkNotHigherThan = goodStock.ChkNotHigherThan;
			}
			try
			{
				List<DataModel.Stock.Stock> _GoodStockList = _StockDayAllList.Where(x => Convert.ToDecimal(x.Gain) > _Gain && Convert.ToDouble(x.TradeVolume) > _Volumn).ToList();
				foreach (DataModel.Stock.Stock s in _GoodStockList)
				{
					_Code = s.Code;
					if (_YestodayStockDayAllList.Where(x => x.Code == s.Code).ToList().Count > 0)
					{
						DataModel.Stock.Stock _YestodayStock = _YestodayStockDayAllList.Where(x => x.Code == s.Code).First();
						double _Multiple = Convert.ToDouble(s.TradeVolume) / Convert.ToDouble(_YestodayStock.TradeVolume);
						if (_YestodayStock.TradeVolume != "0")
						{
							if (_Multiple > multiple)
							{
								bool _Less = true, _NotHigherThan = true;
								List<DataModel.Stock.Stock> _StockListByCode = new List<DataModel.Stock.Stock>();
								if (_ChkLess || _ChkNotHigherThan)
								{
									_StockListByCode = _DataAccess.getStockBySqlList(s.Code, "Code").Where(x => Convert.ToInt32(x.Date) <= Convert.ToInt32(_WhereDate)).OrderByDescending(x => x.Date).ToList();
									if (_ChkLess)
									{
										_Less = _StockListByCode.Take(_PreDays + 1).Where(x => Convert.ToDouble(x.TradeVolume) < _LessVolumn).ToList().Count >= _PreDays;
									}
									if (_ChkNotHigherThan)
									{
										double _Price = Convert.ToDouble(s.ClosingPrice) + Convert.ToDouble(s.ClosingPrice) * _MoreGain / 100;
										_NotHigherThan = _StockListByCode.Take(_PreDays2).Where(x => Convert.ToDouble(x.ClosingPrice) > _Price).ToList().Count == 0;
									}
								}
								if (_Less && _NotHigherThan) _StockList.Add(s);
							}
						}
					}

				}
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + _Code + " getGoodStockList:" + ex.Message + "\r\n";
				logger.Error(_Log);
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
		private List<DataModel.Stock.Stock> getBadStockList()
		{
			Stopwatch sw = new Stopwatch();
			sw.Reset();
			sw.Start();
			int _N = 0;
			double _Ma5 = 0, _Ma10 = 0, _Ma20 = 0;
			string _Code = "";
			string _Log = "";
			DataAccess _DataAccess = new DataAccess();
			DateTime _Dt = Convert.ToDateTime(this.dtpBadDate.Text);
			string _WhereDate = _Dt.ToString("yyyyMMdd");
			string _DayOfWeek = _Dt.DayOfWeek.ToString();
			int _Yestoday = _DayOfWeek == "Monday" ? -3 : -1;
			DateTime _YestodayDate = skipHoliday(_Dt.AddDays(_Yestoday));

			List<DataModel.Stock.Stock> _YestodayStockDayAllList = _DataAccess.getStockBySqlList(_YestodayDate.Date.ToString("yyyyMMdd"), "Date");
			//List<DataModel.Stock.Stock> _YestodayStockDayAllList = _DataAccess.getStockBySqlList(_Dt.AddDays(_Yestoday).Date.ToString("yyyyMMdd"), "Date");
			List<DataModel.Stock.Stock> _StockDayAllList = _DataAccess.getStockBySqlList(_WhereDate, "Date");
			List<DataModel.Stock.Stock> _StockList = new List<DataModel.Stock.Stock>();
			decimal _Diff = this.nudDiff.Value != 0 ? this.nudDiff.Value : 3;
			int _Volumn = this.txtVolumnBad.Text != "" ? Convert.ToInt32(this.txtVolumnBad.Text) * 1000 : 7000000;
			int _Drop = this.nudDropBad.Value != 0 ? Convert.ToInt32(this.nudDropBad.Value) : 5;
			int _PreDayBad = this.txtPreDaysBad.Text != "" ? Convert.ToInt32(this.txtPreDaysBad.Text) : 10;
			try
			{
				List<DataModel.Stock.Stock> _BadStockList = _StockDayAllList.Where(x => x.TradeVolume != "" && Convert.ToDouble(x.TradeVolume) > Convert.ToDouble(_Volumn)).ToList();
				foreach (DataModel.Stock.Stock s in _BadStockList)
				{
					_Code = s.Code;
					List<DataModel.Stock.Stock> _StockListByCode = new List<DataModel.Stock.Stock>();

					if (_YestodayStockDayAllList.Where(x => x.Code == s.Code).Count() > 0)
					{
						DataModel.Stock.Stock _YestodayStock = _YestodayStockDayAllList.Where(x => x.Code == s.Code).First();
						if (_YestodayStock.ClosingPrice != "")
						{
							if (s.HighestPrice == null)
							{
								_Log = "當日資料表無高價可判斷";

							}
							decimal _Gain = Math.Round((Convert.ToDecimal(s.HighestPrice) - Convert.ToDecimal(s.ClosingPrice)) * 100 / Convert.ToDecimal(_YestodayStock.ClosingPrice), 2);
							if (_Gain >= _Diff)
							{
								bool _Less = true, _TotalVolumne = true, _MA5 = true, _MA10 = true, _MA20 = true;
								if (chkDrop.Checked)
								{
									_Less = Convert.ToDouble(s.Gain) <= Convert.ToDouble(-_Drop);
								}
								if (_Less)
								{
									//判斷破線以及爆量才須跟雲端資料庫by code要資料
									if (this.chkMA.Checked || chkVolumnTotal.Checked)
									{
										_StockListByCode = _DataAccess.getStockBySqlList(s.Code, "Code").Where(x => Convert.ToInt32(x.Date) <= Convert.ToInt32(_WhereDate)).OrderByDescending(x => x.Date).ToList();
										if (_StockListByCode.Count > 5) _Ma5 = Math.Round(_StockListByCode.OrderByDescending(x => x.Date).ToList().Take(5).Sum(x => Convert.ToDouble(x.ClosingPrice)) / 5, 2);
										if (_StockListByCode.Count > 10) _Ma10 = Math.Round(_StockListByCode.OrderByDescending(x => x.Date).ToList().Take(10).Sum(x => Convert.ToDouble(x.ClosingPrice)) / 10, 2);
										if (_StockListByCode.Count > 20) _Ma20 = Math.Round(_StockListByCode.OrderByDescending(x => x.Date).ToList().Take(20).Sum(x => Convert.ToDouble(x.ClosingPrice)) / 20, 2);
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
								}
								if (_Less && _TotalVolumne && _MA5 && _MA10 && _MA20)
								{
									_StockList.Add(s);
									_N++;
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "Code:" + _Code + " getBadStockList:" + ex.Message + "\r\n";
				logger.Error(_Log);
			}
			sw.Stop();
			//this.lbBadPerformance.Text = sw.ElapsedMilliseconds.ToString() + "毫秒,筆數:" + _N;
			//MessageBox.Show(sw.ElapsedMilliseconds.ToString() + "毫秒,筆數:"+ _N);
			if (_Log != "" && _N == 0)
			{
				logger.Warn(_Log);
				//MessageBox.Show(_Log);
			}
			return _StockList;
		}
		/// <summary>
		/// 連續型態
		/// 1.	前X日漲/跌Y天,超過Z%/日
		/// 2.	總漲幅大於/小於I%
		/// </summary>
		/// <returns></returns>
		private List<DataModel.Stock.Stock> getCtuStockList()
		{
			int _N = 0;
			Stopwatch sw = new Stopwatch();
			sw.Reset();
			sw.Start();
			string _Code = "";
			string _Log = "";
			DataAccess _DataAccess = new DataAccess();
			DateTime _Dt = Convert.ToDateTime(this.dtpCtuDate.Text);
			string _WhereDate = _Dt.ToString("yyyyMMdd");
			string _DayOfWeek = _Dt.DayOfWeek.ToString();
			int _Yestoday = _DayOfWeek == "Monday" ? -3 : -1;
			DateTime _YestodayDate = skipHoliday(_Dt.AddDays(_Yestoday));
			List<DataModel.Stock.Stock> _StockDayAllList = _DataAccess.getStockBySqlList(_WhereDate, "Date");

			List<DataModel.Stock.Stock> _StockList = new List<DataModel.Stock.Stock>();
			decimal _Gain = this.nupCtuGain.Value != 0 ? this.nupCtuGain.Value : 3;
			int _PreDays = this.txtPreDaysCtu.Text != "" ? Convert.ToInt32(this.txtPreDaysCtu.Text) : 10;
			int _CtuDays = this.txtCtuDays.Text != "" ? Convert.ToInt32(this.txtCtuDays.Text) : 10;
			double _MoreGain = this.nupCtuGain.Value != 0 ? Convert.ToDouble(this.nupCtuGain.Value) : 10;
			double _TotalGain = this.txtTotalGain.Text != "" ? Convert.ToDouble(this.txtTotalGain.Text) : 10;
			int _Volumn = this.txtVolumnCtu.Text != "" ? Convert.ToInt32(this.txtVolumnCtu.Text) * 1000 : 700000;
			//List<DataModel.Stock.Stock> _StockBySqlListPredays = _DataAccess.getStockBySqlList(_WhereDate, "Predays" );
			try
			{
				List<DataModel.Stock.Stock> _GoodStockList = _StockDayAllList.Where(x => Convert.ToDouble(x.TradeVolume) > _Volumn).ToList();
				foreach (DataModel.Stock.Stock s in _GoodStockList)
				{

					_Code = s.Code;
					bool _Ctu = false, _TotalAmp = true;
					//這樣會導致不斷的跟雲端資料庫要資料,會很慢,改成先撈一個月的資料四萬多筆一樣都要跑60幾秒
					List<DataModel.Stock.Stock> _StockListByCode = _DataAccess.getStockBySqlList(s.Code, "Code").Where(x => Convert.ToInt32(x.Date) <= Convert.ToInt32(_WhereDate)).OrderByDescending(x => x.Date).Take(_PreDays).ToList();
					//List<DataModel.Stock.Stock> _StockListByCode = _StockBySqlListPredays.Where(x=>x.Code==s.Code).OrderByDescending(x => x.Date).Take(_PreDays).ToList();
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
					if (_Ctu && _TotalAmp) { _StockList.Add(s); _N++; }
				}
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " Code:" + _Code + " getCtuStockList:" + ex.Message + "\r\n";
				logger.Error(_Log);
			}
			sw.Stop();
			// this.lbCtnPerformance.Text= sw.ElapsedMilliseconds.ToString() + "毫秒";
			//MessageBox.Show(sw.ElapsedMilliseconds.ToString() + "毫秒,筆數:" + _N);
			return _StockList;
		}
		/// <summary>
		/// 將UI參數解耦從外部帶
		/// </summary>
		/// <param name="dtCtuDate"></param>
		/// <param name="gain"></param>
		/// <param name="preDays">前N日</param>
		/// <param name="ctuDays">漲N日</param>
		/// <param name="moreGain">N%</param>
		/// <param name="totalGain">總漲幅</param>
		/// <param name="volumn">成交量</param>
		/// <param name="chkTotals">是否檢查總幅度</param>
		/// <param name="comboReds">是否連續小紅型態</param>
		/// <returns></returns>
		private List<DataModel.Stock.Stock> getCtuStockList(DateTime dtCtuDate, decimal gain, int preDays, int ctuDays, double moreGain, double totalGain, int volumn, bool chkTotals, bool comboReds)
		{
			int _N = 0;
			string _Code = "";
			string _Log = "";
			DataAccess _DataAccess = new DataAccess();
			//DateTime _Dt = Convert.ToDateTime(this.dtpCtuDate.Text);
			string _WhereDate = dtCtuDate.ToString("yyyyMMdd");
			string _DayOfWeek = dtCtuDate.DayOfWeek.ToString();
			int _Yestoday = _DayOfWeek == "Monday" ? -3 : -1;
			List<DataModel.Stock.Stock> _StockDayAllList = _DataAccess.getStockBySqlList(_WhereDate, "Date");

			List<DataModel.Stock.Stock> _StockList = new List<DataModel.Stock.Stock>();
			//List<DataModel.Stock.Stock> _StockBySqlListPredays = _DataAccess.getStockBySqlList(_WhereDate, "Predays" );
			try
			{
				List<DataModel.Stock.Stock> _GoodStockList = _StockDayAllList.Where(x => x.TradeVolume != "").Where(x => Convert.ToDouble(x.TradeVolume) > volumn).ToList();
				foreach (DataModel.Stock.Stock s in _GoodStockList)
				{

					_Code = s.Code;
					bool _Ctu = false, _TotalAmp = true;
					//這樣會導致不斷的跟雲端資料庫要資料,會很慢,改成先撈一個月的資料四萬多筆一樣都要跑60幾秒
					List<DataModel.Stock.Stock> _StockListByCode = _DataAccess.getStockBySqlList(s.Code, "Code").Where(x => Convert.ToInt32(x.Date) <= Convert.ToInt32(_WhereDate)).OrderByDescending(x => x.Date).Take(preDays).ToList();
					//List<DataModel.Stock.Stock> _StockListByCode = _StockBySqlListPredays.Where(x=>x.Code==s.Code).OrderByDescending(x => x.Date).Take(_PreDays).ToList();
					int _GainDays = 0;
					if (comboReds)
						_GainDays = _StockListByCode.Where(x => Convert.ToDouble(x.Gain) <= moreGain && Convert.ToDouble(x.Gain) > 1).ToList().Count;
					else _GainDays = _StockListByCode.Where(x => Convert.ToDouble(x.Gain) > moreGain).ToList().Count;
					if (GainType == "跌") _GainDays = _StockListByCode.Where(x => Convert.ToDouble(x.Gain) < -moreGain).ToList().Count;
					if (_GainDays >= ctuDays)
					{
						_Ctu = true;
					}
					if (chkTotals)
					{
						if (s.ClosingPrice != "")
						{
							double _LastPrice = Convert.ToDouble(s.ClosingPrice);
							if (_StockListByCode.OrderBy(x => x.Date).First().ClosingPrice != "")
							{
								double _FirstPrice = Convert.ToDouble(_StockListByCode.OrderBy(x => x.Date).First().ClosingPrice);
								double _TotalAmpGain = Math.Round((_LastPrice - _FirstPrice) / _FirstPrice, 2);
								_TotalAmp = _TotalAmpGain > (totalGain / 100);
								if (GainType == "跌")
								{
									_TotalAmp = _TotalAmpGain < -(totalGain / 100);
								}
							}
						}
					}
					if (_Ctu && _TotalAmp) { _StockList.Add(s); _N++; }
				}
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " Code:" + _Code + " getCtuStockList:" + ex.Message + "\r\n";
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
					_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " btnOpen_Click:" + ex.Message + "\r\n";
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
			List<StockGroup> _StockGroupList = _DataAccess.getStockGroupList();
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
				DataAccess _DataAccess = new DataAccess();
				List<Stock> _StockList = setStockLackOffList();
				_DataAccess.insertStock(_StockList);

			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " insertStockLackOffList:" + ex.Message + "\r\n";
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
				DataAccess _DataAccess = new DataAccess();
				string _Juridica = "MII_1_4";
				string _TX_URL = ConfigurationManager.AppSettings["TX_URL"];
				StockIndex s = new StockIndex();
				List<StockIndex> _ListStock = _DataAccess.getStockIndexList();
				s = _ListStock.OrderByDescending(x => x.Date).First();
				s.JuridicaPerson = Common.Job.GetTaiwanFutures(_TX_URL, _Juridica, true);
				_DataAccess.UpdateStockIndex(s);
				_Log = "\r\n" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " update stock juridica ok.";
				this.txtErrMsg.Text += _Log;
				bTAIEX = true;
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " btnUpdateJuridicaPerson:" + ex.Message + "\r\n";
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
				DataAccess _DataAccess = new DataAccess();
				StockIndex s = new StockIndex();
				List<StockIndex> _ListStock = _DataAccess.getStockIndexList();
				s = _ListStock.OrderByDescending(x => x.Date).First();
				StockStrategy.Model.Stock.JuridicaPerson _JuridicaPerson = new Model.Stock.JuridicaPerson();
				_JuridicaPerson.queryType = "1";
				_JuridicaPerson.goDay = "";
				_JuridicaPerson.doQuery = "1";
				_JuridicaPerson.dateaddcnt = "";
				_JuridicaPerson.queryDate = "2022/06/20";
				_DataAccess.getJuridicaPerson(_JuridicaPerson);
				//  s.JuridicaPerson = Common.Job.GetTaiwanFutures(_TX_URL, _Juridica, true);
				_DataAccess.UpdateStockIndex(s);
				_Log = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " update stock juridica ok.\r\n";
				this.txtErrMsg.Text += _Log;
				bTAIEX = true;
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " btnUpdateJuridicaPerson:" + ex.Message + "\r\n";
				logger.Error(_Log);
				this.txtErrMsg.Text += _Log;
			}
		}
		/// <summary>
		/// 起漲放量:1 連續型態:2 連續小紅:3 爆量出貨:4 
		/// 起漲五倍量:5 起漲十倍量:10 盤底放量 盤月放量:6  盤季放量:7  盤半年放量:8
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnGetGoodStock_Click(object sender, EventArgs e)
		{
			progressBar2.Style = ProgressBarStyle.Marquee;
			progressBar2.MarqueeAnimationSpeed = 30;
			progressBar2.Show();
			var s = Task.Run(() => getGoodStock());
			//var s = Task.Run(() => getGoodStockList());
			var _StockList = s.Result;
			if (chkPickingGood.Checked) insertStockPicking(_StockList, 1);
		}
		private List<DataModel.Stock.Stock> getGoodStock()
		{
			string _Log = "";
			GoodStock = "";
			List<DataModel.Stock.Stock> _StockList = new List<DataModel.Stock.Stock>();
			StockStrategy.Model.GoodStock _GoodStock = new Model.GoodStock();
			try
			{
				_StockList = getGoodStockList(_GoodStock);

				foreach (DataModel.Stock.Stock s in _StockList)
				{
					GoodStock = GoodStock + s.Code + ":" + s.Name + ";";
				}
				MethodInvoker mi = new MethodInvoker(this.UpdateUIGoodStock);
				this.BeginInvoke(mi, null);

			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " GetGoodStock:" + ex.Message + "\r\n";
				logger.Error(_Log);
			}
			return _StockList;
		}
		private List<DataModel.Stock.Stock> getGoodStock(StockStrategy.Model.GoodStock goodStock)
		{
			string _Log = "";
			GoodStock = "";
			List<DataModel.Stock.Stock> _StockList = new List<DataModel.Stock.Stock>();
			try
			{
				_StockList = getGoodStockList(goodStock);

				foreach (DataModel.Stock.Stock s in _StockList)
				{
					GoodStock = GoodStock + s.Code + ":" + s.Name + ";";
				}
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " GetGoodStock(Json):" + ex.Message + "\r\n";
				logger.Error(_Log);
			}
			return _StockList;
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

		private List<DataModel.Stock.Stock> getBadStock()
		{
			string _Log = "";
			BadStock = "";
			List<DataModel.Stock.Stock> _StockList = new List<DataModel.Stock.Stock>();
			try
			{
				_StockList = getBadStockList();
				foreach (DataModel.Stock.Stock s in _StockList)
				{
					BadStock = BadStock + s.Code + ":" + s.Name + ";";
				}
				MethodInvoker mi = new MethodInvoker(this.UpdateUIBadStock);
				this.BeginInvoke(mi, null);
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " GetBadStock:" + ex.Message + "\r\n";
				logger.Error(_Log);
			}
			return _StockList;
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
				//string _UpdatTime = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
				// foreach (Stock s in _StockList) {
				//     s.Gain = s.Change.Replace('+',' ');
				//     s.Change = "";
				//     s.UpdateTime = _UpdatTime;
				//     _DataAccess.UpdateStock(s);
				//     _No++;
				// } 

				List<Stock> _StockList = _DataAccess.getStockAllList().Where(x => x.HighestPrice != "" && x.Gain == null).ToList();
				string _UpdatTime = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
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
				_Log = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " Update Stock Gain ok.\r\n";
				this.txtErrMsg.Text += _Log;
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " btnUpdateStockGain_Click:" + ex.Message + "\r\n";
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
			bUpdateStockLineNotify = true;
			_Log = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " reset Line notify ok." + "\r\n";
			this.txtErrMsg.Text += _Log;
		}
		/// <summary>
		/// Line通知選股
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnLineGoodStock_Click(object sender, EventArgs e)
		{
			progressBar2.Style = ProgressBarStyle.Marquee;
			progressBar2.MarqueeAnimationSpeed = 30;
			progressBar2.Show();
			var s = Task.Run(() => getGoodStockList());
			var _StockList = s.Result;
			string _DateTime = this.dtpGoodDate.Text;
			lineAIStock(_StockList, "Good", "起漲放量:", _DateTime);
			insertStockPicking(_StockList, 1);
		}
		public async void CallLineNotifyApi(string token, string lineMsg)
		{
			string _Log = "";
			try
			{
				await _DataAccess.postLineMsg(lineMsg, token);
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " CallLineNotifyApi:" + ex.Message + "\r\n";
				logger.Error(_Log);
			}
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


			List<StockGroup> _StockGroupList = _DataAccess.getStockGroupList();
			string _UpdateTime = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
			foreach (StockGroup s in _StockGroupList)
			{
				try
				{
					if (_StockIdList.IndexOf(s.Code) < 0)
					{
						//多執行緒 沒比較快
						//var t = new Task<Stock>(() => getHiStockData( s, _UpdateTime));
						//t.Start();
						Stock _Stock = getHiStockData(s, _UpdateTime);
						//Stock _Stock = t.Result;
						if (_Stock != null)
						{
							_Code = _Stock.Code;
							_StockLackOffList.Add(_Stock);
						}
						_No++;

						MethodInvoker mi = new MethodInvoker(this.UpdateUI);
						this.BeginInvoke(mi, null);
					}
				}
				catch (Exception ex)
				{
					_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " btnInsertStockLackOff_Click:" + _Code + ex.Message + "\r\n";
					logger.Error(_Log);
				}
			}
			sw.Stop();
			logger.Info(sw.ElapsedMilliseconds.ToString() + "毫秒,筆數:" + _No + ":" + _StockLackOffList.Count);
			//MessageBox.Show(sw.ElapsedMilliseconds.ToString() + "毫秒,筆數:" + _No);
			return _StockLackOffList;
		}
		private Stock getHiStockData(StockGroup s, string updateTime)
		{
			Stock _Stock = new Stock();
			string _HiStock_URL = ConfigurationManager.AppSettings["HiStock_URL"];
			HtmlAgilityPack.HtmlDocument _HtmlDoc = Common.Job.GetHtml(_HiStock_URL + s.Code);
			if (_HtmlDoc.GetElementbyId(Volume) != null)
			{
				string _TradeVolume = _HtmlDoc.GetElementbyId(Volume).InnerText;
				_Stock.Code = s.Code;
				_Stock.Name = s.Name;
				string _Volume = _TradeVolume.TrimEnd('張');
				double _VolumeValue = Convert.ToDouble(_Volume) * 1000;
				_Stock.TradeVolume = _Volume != "0" ? Convert.ToString(_VolumeValue) : "0";
				_Stock.ClosingPrice = _HtmlDoc.GetElementbyId(Id).InnerText;
				_Stock.TradeValue = _Volume != "0" ? Convert.ToString(_VolumeValue * Convert.ToDouble(_Stock.ClosingPrice)) : "0";
				string _Change = _HtmlDoc.GetElementbyId(Change).InnerText;
				_Change = _Change.Replace('▲', ' ').Replace('▼', '-');
				_Stock.Change = _Change;
				string _Gain = _HtmlDoc.GetElementbyId(Percent).InnerText;
				_Gain = _Gain.Replace('+', ' ').TrimEnd('%');
				_Stock.Gain = _Gain;
				_Stock.Date = DateTime.Now.ToString("yyyyMMdd");
				_Stock.UpdateTime = updateTime;
			}
			return _Stock;
		}
		/// <summary>
		/// 根據日期取得證交所資料打算補該日期的股票資料,目前交易過於頻繁會被鎖IP
		/// </summary>
		/// <returns></returns>
		private List<Stock> setStockByDateList()
		{
			DataAccess _DataAccess = new DataAccess();
			DateTime _Dt = Convert.ToDateTime(this.dtpStockIndex.Text);
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
			List<StockGroup> _StockGroupList = _DataAccess.getStockGroupList();
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
					_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " setStockByDateList:" + _Code + ex.Message + "\r\n";
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
			List<StockGroup> _StockGroupList = _DataAccess.getStockGroupList();
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
				DataAccess _DataAccess = new DataAccess();
				List<Stock> _StockList = setStockByDateList();
				_DataAccess.insertStock(_StockList);
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " insertStockByDateList(:" + ex.Message + "\r\n";
				logger.Error(_Log);
			}
		}
		/// <summary>
		/// 更新往日三大法人資料-改為更新前一個workday的資料
		/// 新需求:三大法人前十大買賣超個股
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnStockJurical_Click(object sender, EventArgs e)
		{
			DateTime _Dt = Convert.ToDateTime(this.dtpStockYesterday.Text);
			int _Yestoday = DateTime.Now.DayOfWeek.ToString() == "Monday" ? -3 : -1;
			DateTime _YestodayDate = skipHoliday(_Dt.AddDays(_Yestoday));
			//若沒勾選則補前一個工作天的三大法人資料,否則補選擇的日期資料
			if (!chkJurical.Checked) _Dt = _YestodayDate;
			System.Globalization.TaiwanCalendar tc = new System.Globalization.TaiwanCalendar();
			string CHDate = tc.GetYear(_Dt).ToString() + "/" + tc.GetMonth(_Dt).ToString().PadLeft(2, '0') + "/" + tc.GetDayOfMonth(_Dt).ToString().PadLeft(2, '0');
			string _WhereDate = _Dt.ToString("yyyyMMdd");
			List<string> _RecoveryDate = new List<string>();
			_RecoveryDate.Add(_WhereDate);
			//updateStockJuridical(_WhereDate); 
			//            string[] _RecoveryDate = {
			//                "20220527" 
			//            };
			foreach (string s in _RecoveryDate)
			{
				updateStockJuridical(s);
			}
		}
		/// <summary>
		/// 取得證交所股票三大法人資料
		/// </summary>
		/// <param name="whereDate"></param>
		private void updateStockJuridical(string whereDate)
		{
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
				_Log = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " update stock jurical ok.\r\n";
				this.txtErrMsg.Text += _Log;
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " btnStockJurical:" + ex.Message + "\r\n";
				logger.Error(_Log);
				this.txtErrMsg.Text += _Log;
			}
		}

		private void btnLineCtuStock_Click(object sender, EventArgs e)
		{
			pgBarCtu.Style = ProgressBarStyle.Marquee;
			pgBarCtu.MarqueeAnimationSpeed = 30;
			pgBarCtu.Show();
			var s = Task.Run(() => getCtuStockList());
			var _StockList = s.Result;
			string _DateTime = this.dtpCtuDate.Text;
			lineAIStock(_StockList, "Ctu", "連續型態", _DateTime);
			insertStockPicking(_StockList, 2);
		}
		private void lineAIStock(List<DataModel.Stock.Stock> stockList, string className, string name, string dateTime)
		{
			string _Log = "";
			try
			{
				string _Stock = "";
				foreach (DataModel.Stock.Stock s in stockList)
				{
					_Stock = _Stock + s.Code + ":" + s.Name + ";";
				}
				List<WebApiService.Models.StockLineNotify> _StockLineNotifyList = _DataAccess.getStockLineNotifyList();
				foreach (WebApiService.Models.StockLineNotify s in _StockLineNotifyList.Where(x => x.NotifyClass == className && x.Enable == true).ToList())
				{
					string _Msg = "日期:" + dateTime + ":" + name + _Stock;
					CallLineNotifyApi(s.Token, _Msg);
				}
				this.txtErrMsg.Text += "Line  " + className + " Stock List OK" + "\r\n";
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " lineAIStock:" + ex.Message + "\r\n";
				logger.Error(_Log);
				this.txtErrMsg.Text += _Log;
			}
		}
		private void btnLineBad_Click(object sender, EventArgs e)
		{
			pgBarBad.Style = ProgressBarStyle.Marquee;
			pgBarBad.MarqueeAnimationSpeed = 30;
			pgBarBad.Show();
			var s = Task.Run(() => getBadStockList());
			var _StockList = s.Result;
			string _DateTime = this.dtpBadDate.Text;
			lineAIStock(_StockList, "Bad", "爆量出貨", _DateTime);
			insertStockPicking(_StockList, 4);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.btnLogin.PerformClick();
			var test = _DataAccess.getHolidayList();
		}

		private void btnGetTopIndex_Click(object sender, EventArgs e)
		{

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
			config.AppSettings.Settings["Password"].Value = _Password;
			config.Save(ConfigurationSaveMode.Modified);
		}

		/// <summary>
		/// 連續小紅:連漲3天,1%<漲幅<3%,成交量需>300張
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnLineCtuRed_Click(object sender, EventArgs e)
		{
			var s = Task.Run(() => getCtuStockList(Convert.ToDateTime(this.dtpCtuDate.Text), 3, 3, 3, 3, 3, 300000, false, true));
			var _StockList = s.Result;
			string _DateTime = this.dtpCtuDate.Text;
			lineAIStock(_StockList, "Ctu", "連續小紅:", _DateTime);
			insertStockPicking(_StockList, 3);
		}

		private void LineStockIndex_Click(object sender, EventArgs e)
		{
			string _Log = "";
			try
			{
				string _Stock = "";
				List<StockIndex> _ListStockAll = _DataAccess.getStockIndexList();
				//string dateTime = DateTime.Now.ToString("yyyyMMdd");
				string dateTime = Convert.ToDateTime(this.dtpStockIndex.Text).ToString("yyyyMMdd");
				StockIndex _StockIndex = _ListStockAll.Where(x => x.Date == dateTime).First();
				List<WebApiService.Models.StockLineNotify> _StockLineNotifyList = _DataAccess.getStockLineNotifyList();
				DateTime _Dt = Convert.ToDateTime(this.dtpStockIndex.Text);
				int _Yestoday = DateTime.Now.DayOfWeek.ToString() == "Monday" ? -3 : -1;
				DateTime _YestodayDate = skipHoliday(_Dt.AddDays(_Yestoday));

				//List<DataModel.Stock.Stock> _YestodayStockDayAllList = _DataAccess.getStockBySqlList(_YestodayDate.Date.ToString("yyyyMMdd"), "Date");
				List<Stock> _YestodayStockDayAllList = _DataAccess.getStockYestodayList(_YestodayDate.Date.ToString("yyyyMMdd"));
				int _Rise = _YestodayStockDayAllList.Where(x => Convert.ToDouble(x.Gain) > 0).ToList().Count;
				int _Fall = _YestodayStockDayAllList.Where(x => Convert.ToDouble(x.Gain) < 0).ToList().Count;
				int _RiseHalf = _YestodayStockDayAllList.Where(x => Convert.ToDouble(x.Gain) > 4.9).ToList().Count;
				int _FallHalf = _YestodayStockDayAllList.Where(x => Convert.ToDouble(x.Gain) < -4.9).ToList().Count;
				_StockIndex.Ups = _Rise.ToString();
				_StockIndex.Downs = _Fall.ToString();
				_StockIndex.UpsHalf=_RiseHalf.ToString();
				_StockIndex.DownsHalf=_FallHalf.ToString();
				foreach (WebApiService.Models.StockLineNotify s in _StockLineNotifyList.Where(x => x.NotifyClass == "USA_Index" && x.Enable == true).ToList())
				{
					string _Msg = String.Format("日期:{0}({1})\r\n", dateTime, _StockIndex.DayOfWeek);
					_Msg += String.Format("道瓊:{0} 漲跌:{1}\r\n", _StockIndex.DJI, _StockIndex.DJI_QuoteChange);
					_Msg += String.Format("NQ:{0} 漲跌:{1}\r\n", _StockIndex.NASDAQ, _StockIndex.NASDAQ_QuoteChange);
					_Msg += String.Format("費半:{0} 漲跌:{1}\r\n", _StockIndex.PHLX, _StockIndex.PHLX_QuoteChange);
					_Msg += String.Format("夜盤:{0} 漲跌:{1}\r\n", _StockIndex.MTX, _StockIndex.MTX_QuoteChange);
					_Msg += String.Format("櫃買:{0} 漲跌:{1}\r\n", _StockIndex.TPEx, _StockIndex.TPEx_QuoteChange);
					_Msg += String.Format("美元:{0} 漲跌:{1}\r\n", _StockIndex.USD_Index, _StockIndex.USD_IndexQuoteChange);
					_Msg += String.Format("輕原油:{0} 漲跌:{1}\r\n", _StockIndex.OIL_Index, _StockIndex.OIL_QuoteChange);
					_Msg += String.Format("比特幣:{0} 漲跌:{1}\r\n", _StockIndex.BTC_Index, _StockIndex.BTC_QuoteChange);
					_Msg += String.Format("黃金:{0} 漲跌:{1}\r\n", _StockIndex.Gold_Index, _StockIndex.Gold_QuoteChange);
					_Msg += String.Format("日經:{0} 漲跌:{1}\r\n", _StockIndex.NIKKEI_Index, _StockIndex.NIKKEI_QuoteChange);
					_Msg += String.Format("德國:{0} 漲跌:{1}\r\n", _StockIndex.DAX_Index, _StockIndex.DAX_QuoteChange);
					_Msg += String.Format("恆生:{0} 漲跌:{1}\r\n", _StockIndex.VHSI_Index, _StockIndex.VHSI_QuoteChange);
					_Msg += String.Format("漲跌家數:▲{0}家 ▼{1}家\r\n", _Rise, _Fall);
					_Msg += String.Format("半根:▲{0}家 ▼{1}家\r\n", _RiseHalf, _FallHalf);
					_DataAccess.UpdateStockIndex(_StockIndex);
					_Log = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " update stock half ok.";
					this.txtErrMsg.Text += _Log;
					CallLineNotifyApi(s.Token, _Msg);
				}
				this.txtErrMsg.Text += "Line Stock Index OK" + "\r\n";
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " LineStockIndex:" + ex.Message + "\r\n";
				logger.Error(_Log);
				this.txtErrMsg.Text += _Log;
			}
		}

		private void btnInsertStockPicking_Click(object sender, EventArgs e)
		{
			string _Log = "";
			try
			{
				DataAccess _DataAccess = new DataAccess();
				this.btnLogin.PerformClick();
				List<StockPicking> _StockPickingList = new List<StockPicking>();
				StockPicking stockPicking = new StockPicking();
				stockPicking.Class = 0;
				stockPicking.Date = "20230419";
				stockPicking.Enabled = true;
				stockPicking.Code = "3663";
				stockPicking.StartPrice = "90";
				stockPicking.StartVolume = "100";
				stockPicking.YesterdayVolume = "20";
				stockPicking.Remark = "";
				_StockPickingList.Add(stockPicking);
				_DataAccess.InsertStockPicking(_StockPickingList);
				_Log = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " insert stock picking ok.\r\n";
				this.txtErrMsg.Text += _Log;
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " InsertStockPicking:" + ex.Message + "\r\n";
				logger.Error(_Log);
				this.txtErrMsg.Text += _Log;
			}
		}
		/// <summary>
		/// 新增選股
		/// </summary>
		/// <param name="stockList"></param>
		/// <param name="iclass"></param>
		private void insertStockPicking(List<DataModel.Stock.Stock> stockList, int iclass)
		{
			//停用日
			string _Log = "";
			try
			{
				DateTime _Dt = Convert.ToDateTime(this.dtpGoodDate.Text);
				string _DayOfWeek = _Dt.DayOfWeek.ToString();
				int _Yestoday = _DayOfWeek == "Monday" ? -3 : -1;
				DateTime _YestodayDate = skipHoliday(_Dt.AddDays(_Yestoday));
				List<DataModel.Stock.Stock> _YestodayStockDayAllList = _DataAccess.getStockBySqlList(_YestodayDate.Date.ToString("yyyyMMdd"), "Date");
				List<StockPicking> _StockPickingList = new List<StockPicking>();
				List<StockPicking> _ListStockPicking = _DataAccess.getStockPickingList().Where(x => x.Enabled == true).OrderByDescending(x => x.Date).ToList();
				foreach (DataModel.Stock.Stock s in stockList)
				{
					DataModel.Stock.Stock _YestodayStock = _YestodayStockDayAllList.Where(x => x.Code == s.Code).First();
					StockPicking stockPicking = new StockPicking();
					stockPicking.Class = iclass;
					stockPicking.Date = _Dt.ToString("yyyyMMdd");
					stockPicking.Enabled = true;
					stockPicking.Code = s.Code;
					stockPicking.StartPrice = s.ClosingPrice;
					stockPicking.StartVolume = s.TradeVolume;
					stockPicking.YesterdayVolume = _YestodayStock.TradeVolume;
					stockPicking.Remark = "";
					stockPicking.StockPickingId = iclass + s.Code.PadLeft(6, '0') + stockPicking.Date;
					stockPicking.CreateDate = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
					stockPicking.CreateId = "System";
					stockPicking.Approve = true;
					stockPicking.AccumulatedGain = "0";
					stockPicking.AccumulatedDate = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
					//if (_ListStockPicking.Where(x => x.StockPickingId == stockPicking.StockPickingId).Count() == 0)
					if (_ListStockPicking.Where(x => x.Code == stockPicking.Code && x.Date == stockPicking.Date).Count() > 0)
					{
						foreach (StockPicking sp in _ListStockPicking.Where(x => x.Code == stockPicking.Code && x.Date == stockPicking.Date).ToList())
						{
							//int _Id = sp.Id;
							_DataAccess.DeleteStockPicking(sp.Id);
						}
					}
					_StockPickingList.Add(stockPicking);

				}
				_DataAccess.InsertStockPicking(_StockPickingList);
				_Log = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " insert stock picking ok.\r\n";
				this.txtErrMsg.Text += _Log;
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " InsertStockPicking:" + ex.Message + "\r\n";
				logger.Error(_Log);
				this.txtErrMsg.Text += _Log;
			}
		}

		private void btnTestLog_Click(object sender, EventArgs e)
		{
			string _Log = "";
			try
			{
				DataAccess _DataAccess = new DataAccess();
				this.btnLogin.PerformClick();
				List<StockResult> _StockResultList = new List<StockResult>();
				StockResult _StockResult = new StockResult();
				_StockResult.Code = "3663";
				_StockResult.ClosingPrice = "100";
				_StockResult.Gain = "10";
				_StockResult.Date = "20220202";
				_StockResult.UpdateTime = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
				_StockResultList.Add(_StockResult);
				_DataAccess.InsertStockResult(_StockResultList);
				_Log = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " insert stock backtest log ok.\r\n";
				this.txtErrMsg.Text += _Log;
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " btnTestLog:" + ex.Message + "\r\n";
				logger.Error(_Log);
				this.txtErrMsg.Text += _Log;
			}
		}
		/// <summary>
		/// 1. 賴通知
		/// 2. 將漲幅更新回主表 壓回日
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnStockResult_Click(object sender, EventArgs e)
		{
			string _Log = "";
			try
			{
				DataAccess _DataAccess = new DataAccess();
				this.btnLogin.PerformClick();
				//DateTime _Dt = Convert.ToDateTime(this.dTPReport.Text);
				DateTime _Dt = Convert.ToDateTime(this.dtpStockIndex.Text);
				string _DayOfWeek = _Dt.DayOfWeek.ToString();
				string _WhereDate = _Dt.ToString("yyyyMMdd");
				int _Yestoday = _DayOfWeek == "Monday" ? -3 : -1;
				DateTime _YestodayDate = skipHoliday(_Dt.AddDays(_Yestoday));
				List<DataModel.Stock.Stock> _YestodayStockDayAllList = _DataAccess.getStockBySqlList(_YestodayDate.Date.ToString("yyyyMMdd"), "Date");
				List<StockPicking> _ListStockPicking = _DataAccess.getStockPickingList().Where(x => x.Enabled == true).ToList();
				List<StockPicking> _StockPickingList = _ListStockPicking.Where(x => (x.Class == 1 || x.Class == 5 || x.Class == 6 || x.Class == 7 || x.Class == 8 || x.Class == 9 || x.Class == 10 || x.Class == 11) && Convert.ToInt32(x.Date) < Convert.ToInt32(_WhereDate)).ToList();
				List<StockPicking> _StockPickingCtnList = _ListStockPicking.Where(x => (x.Class == 2 || x.Class == 3) && Convert.ToInt32(x.Date) == Convert.ToInt32(_YestodayDate.ToString("yyyyMMdd"))).ToList();
				List<StockPicking> _StockPickingBadList = _ListStockPicking.Where(x => x.Class == 4 && Convert.ToInt32(x.Date) < Convert.ToInt32(_WhereDate)).ToList();
				List<DataModel.Stock.Stock> _StockDayAllList = _DataAccess.getStockBySqlList(_WhereDate, "Date");
				decimal _AccumulatedGain = 0;
				List<StockResult> _StockResultList = new List<StockResult>();

				foreach (StockPicking s in _StockPickingList.Union(_StockPickingCtnList).OrderBy(x => x.Class))
				{
					List<StockResult> stockResultList = _DataAccess.getStockResultList();
					if (_StockDayAllList.Where(x => x.Code == s.Code).ToList().Count > 0)
					{
						DataModel.Stock.Stock _Stock = _StockDayAllList.Where(x => x.Code == s.Code).First();
						StockResult _StockResult = new StockResult();
						_StockResult.Code = s.Code;
						_StockResult.ClosingPrice = _Stock.ClosingPrice;
						_StockResult.Gain = _Stock.Gain;
						_StockResult.Date = _WhereDate;
						_StockResult.UpdateTime = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
						_StockResult.StockPickingId = s.StockPickingId;
						_StockResult.Precise = IsPrecise(s.Class, _Stock.Gain);
						_StockResult.Class = s.Class;
						_AccumulatedGain = Math.Round((Convert.ToDecimal(_Stock.ClosingPrice) - Convert.ToDecimal(s.StartPrice)) / Convert.ToDecimal(s.StartPrice), 4) * 100;
						s.AccumulatedGain = _AccumulatedGain.ToString();
						s.AccumulatedDate = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
						if (Convert.ToInt32(s.Date) == Convert.ToInt32(_YestodayDate.Date.ToString("yyyyMMdd")))
						{
							s.OpeningPrice = _Stock.OpeningPrice;
						}

						_DataAccess.UpdateStockPicking(s);
						if (stockResultList.Where(x => x.Code == s.Code && x.Date == _WhereDate).Count() > 0)
						{
							foreach (StockResult sr in stockResultList.Where(x => x.Code == s.Code && x.Date == _WhereDate).ToList())
							{
								_DataAccess.DeleteStockResult(sr.Id);
							}
						}
						if (_StockResultList.Where(x => x.Code == s.Code).ToList().Count > 0)
							_StockResultList.Remove(_StockResultList.Where(x => x.Code == s.Code).First());
						_StockResultList.Add(_StockResult);
					}
				}
				_DataAccess.InsertStockResult(_StockResultList);
				_Log = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " insert stock Result ok.\r\n";
				this.txtErrMsg.Text += _Log;
				this.lbMessage.Text = _WhereDate + " insert stock Result ok";
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " btnStockResult:" + ex.Message + "\r\n";
				logger.Error(_Log);
				this.txtErrMsg.Text += _Log;
			}
		}
		private bool IsPrecise(int? iclass, string gain)
		{
			bool _IsOK = false;
			if (gain != "")
			{
				if (iclass != 3)
				{
					if (Convert.ToDouble(gain) > 0) _IsOK = true;
				}
			}
			return _IsOK;
		}

		private void btnTestJson_Click(object sender, EventArgs e)
		{
			string _Log = "";
			try
			{
				StreamReader r = new StreamReader("Json\\Setting.json");
				string jsonString = r.ReadToEnd();
				//	StockStrategy.Model.GoodStock _GoodStock = JsonConvert.DeserializeObject<StockStrategy.Model.GoodStock>(jsonString);
				//List<StockStrategy.Model.StockGoodSetting> _StockResultList = JsonConvert.DeserializeObject<List<StockStrategy.Model.StockGoodSetting>>(_GoodStock.GoodSetting);
				List<StockStrategy.Model.GoodStock> _GoodStockList = JsonConvert.DeserializeObject<List<StockStrategy.Model.GoodStock>>(jsonString);

				//List<StockStrategy.Model.StockGoodSetting> _StockResultList = new List<StockStrategy.Model.StockGoodSetting>();

			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " btnTestJson:" + ex.Message + "\r\n";
				logger.Error(_Log);
				this.txtErrMsg.Text += _Log;
			}
		}

		private void btnGetGoodStockByJson_Click(object sender, EventArgs e)
		{
			StreamReader r = new StreamReader("Json\\Setting.json");
			string jsonString = r.ReadToEnd();
			List<StockStrategy.Model.GoodStock> _GoodStockList = JsonConvert.DeserializeObject<List<StockStrategy.Model.GoodStock>>(jsonString);
			foreach (StockStrategy.Model.GoodStock g in _GoodStockList)
			{
				//非同步的話會導致重複key,所以改為同步
				//	var s = Task.Run(() => getGoodStock(g));
				var s = getGoodStock(g);
				//var _StockList = s.Result;
				string _DateTime = this.dtpGoodDate.Text;
				//lineAIStock(s, "Good", g.Name, _DateTime);
				if (s.Count > 0) insertStockPicking(s, g.Class);
			}
		}
		/// <summary>
		/// 計算觀察名單的獲利報表
		/// 1. 勾選幾日後獲利
		/// 2. 篩選是否核放條件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnQuery_Click(object sender, EventArgs e)
		{
			progressBar3.Style = ProgressBarStyle.Marquee;
			progressBar3.MarqueeAnimationSpeed = 30;
			progressBar3.Show();
			string _Log = "";
			int _Num = 1;
			string Code = "";
			try
			{
				DataAccess _DataAccess = new DataAccess();
				this.btnLogin.PerformClick();
				DateTime _Dt = Convert.ToDateTime(this.dTPReport.Text);
				string _DayOfWeek = _Dt.DayOfWeek.ToString();
				string _WhereDate = _Dt.ToString("yyyyMMdd");
				
				int _Yestoday = _DayOfWeek == "Monday" ? -3 : -1;
				DateTime _YestodayDate = skipHoliday(_Dt.AddDays(_Yestoday));
				DateTime _LastDate = skipHoliday(DateTime.Now.AddDays(-1));
				string _Last = _LastDate.ToString("yyyyMMdd");
				//撈取50萬筆一年的股票row data要一分鐘
				//List<Stock> _StockList = _DataAccess.getStockAllList().ToList();
				List<StockPicking> _ListStockPicking = _DataAccess.getStockPickingList().Where(x => x.Enabled == true).ToList();
				List<StockPicking> _StockPickingList = _ListStockPicking.Where(x => (x.Class == 1 || x.Class == 5 || x.Class == 6 || x.Class == 7 || x.Class == 8 || x.Class == 9 || x.Class == 10 || x.Class == 11) && Convert.ToInt32(x.Date) < Convert.ToInt32(_WhereDate)).ToList();
				_StockPickingList = _StockPickingList.Where(x => Convert.ToInt32(x.Date) >= Convert.ToInt32(this.dtpReportStart.Value.ToString("yyyyMMdd"))).ToList();
				List<StockGroup> _ListStockGroup = _DataAccess.getStockGroupList();
				List<DataModel.Stock.Stock> _StockDayAllList = _DataAccess.getStockBySqlList(_WhereDate, "Date");
				List<DataModel.Stock.Stock> _StockTodayList = _DataAccess.getStockBySqlList(_Last, "Date");
				decimal _AccumulatedGain = 0;
				List<StockResult> _StockResultList = _DataAccess.getStockResultList();
				//List<DataModel.Stock.StockReport> _StockReportList = new List<DataModel.Stock.StockReport>();
				SortableBindingList<DataModel.Stock.StockReport> _StockReportList = new SortableBindingList<DataModel.Stock.StockReport>();
				if (chkApproved.Checked) _StockPickingList = _StockPickingList.Where(x => x.Approve == true).ToList();
				int _Increase = 0, _Decrease = 0;
				foreach (StockPicking s in _StockPickingList.OrderBy(x => x.Date).ToList())
				{
					Code = s.Code;
					//改成更新open price
					//if (_Num == 1) this.txtStartDate.Text = s.Date;
					//List<DataModel.Stock.Stock> _StockList = _DataAccess.getStockBySqlList(s.Date, "Date");
					//DataModel.Stock.Stock _Stock = _StockList.Where(x => x.Code == s.Code).First();
					DataModel.Stock.StockReport _StockReport = new DataModel.Stock.StockReport();
					_StockReport.StockClass = GoodStockClass.Where(x => x.Key == s.Class) != null ? GoodStockClass.Where(x => x.Key == s.Class).First().Value : "";
					_StockReport.StartDate = s.Date;
					_StockReport.Code = s.Code;
					if (_ListStockGroup.Where(x => x.Code == s.Code).ToList().Count > 0)
						_StockReport.Name = _ListStockGroup.Where(x => x.Code == s.Code).First().Name;
					_StockReport.StartPrice = s.StartPrice;

					_StockReport.Remark = s.Remark;
					
					//_StockReport.SecondDayOpeningPrice = _Stock.OpeningPrice;
					_StockReport.SecondDayOpeningPrice = s.OpeningPrice;
					if (chkDay.Checked)
					{
						_WhereDate = Convert.ToString(Convert.ToDouble(s.Date) + Convert.ToDouble(txtManyDays.Text));
					}
					List<DataModel.Stock.Stock> _StockList = _DataAccess.getStockBySqlList(s.Code, "Code").ToList();
					string _ClosingPrice = s.StartPrice;
					if(_StockList.Where(x => x.Date == _WhereDate).ToList().Count>0)
						_ClosingPrice = _StockList.Where(x => x.Date == _WhereDate).First().ClosingPrice;
					
					_StockReport.ClosingPrice = _StockResultList.Where(x => x.Date == _WhereDate && x.Code == s.Code).ToList().Count > 0 ? _StockResultList.Where(x => x.Date == _WhereDate && x.Code == s.Code).First().ClosingPrice : _ClosingPrice;
					if (s.SuspendDate != ""&& s.SuspendDate != null) _StockReport.ClosingPrice = _StockList.Where(x => x.Date == s.SuspendDate).First().ClosingPrice;
					decimal _StartPrice = 0m;
					_StartPrice = Convert.ToDecimal(_StockReport.StartPrice);
					if (chkOpenningPrice.Checked)
					{
						if (_StockReport.SecondDayOpeningPrice != null)
							_StartPrice = Convert.ToDecimal(_StockReport.SecondDayOpeningPrice);
					} 
					_AccumulatedGain = Convert.ToDecimal(_StockReport.ClosingPrice) - _StartPrice;
					_StockReport.AccumulatedGain = s.AccumulatedGain != "0" ? Convert.ToDecimal(s.AccumulatedGain) : _AccumulatedGain;
					if (chkDay.Checked) _StockReport.AccumulatedGain = Math.Round(_AccumulatedGain / _StartPrice, 2);
					_StockReport.GainProfit = _AccumulatedGain * 1000;
					if (_StockReport.GainProfit > 0) _Increase++;
					else _Decrease++;
					_StockReport.StartVolumn =Convert.ToString( Convert.ToInt32( s.StartVolume)/1000);
					_StockReport.YesterdayVolumn = Convert.ToString(Convert.ToInt32(s.YesterdayVolume) / 1000);
					_StockReport.StockPickingId = s.StockPickingId;
					_StockReport.Approve = s.Approve == true ? "核放" : "不核放";
					_StockReport.TodayPrice = _StockTodayList.Where(x => x.Code == s.Code).Count()>0? _StockTodayList.Where(x => x.Code == s.Code).First().ClosingPrice:s.StartPrice;
					decimal _TotalGain = Convert.ToDecimal(_StockReport.TodayPrice) - Convert.ToDecimal(_StockReport.StartPrice);
					_StockReport.TotalGain =Math.Round(_TotalGain / Convert.ToDecimal(_StockReport.StartPrice) ,2);
					_StockReportList.Add(_StockReport);
					_Num++;
				}
				this.lbIncrease.Text = _Increase.ToString();
				this.lbDecrease.Text = _Decrease.ToString();
				decimal _TotalGainProfit = _StockReportList.Sum(x => Convert.ToDecimal(x.GainProfit));
				decimal _TotalCost = _StockReportList.Sum(x => Convert.ToDecimal(x.StartPrice) * 1000);
				decimal _Profit = Math.Round(_TotalGainProfit / _TotalCost, 2) * 100;
				this.txtTotalCost.Text = _TotalCost.ToString();
				this.txtProfit.Text = _Profit.ToString();
				this.txtTotalProfit.Text = _TotalGainProfit.ToString(); 
				this.dgvStockReport.DataSource = _StockReportList;
				_Log = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " query stock report ok.\r\n";
				this.txtErrMsg.Text += _Log;
				progressBar3.Style = ProgressBarStyle.Continuous;
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " btnQuery:第" + _Num + "筆 "+Code + ex.Message + "\r\n";
				logger.Error(_Log);
				this.txtErrMsg.Text += _Log;
			}
		}
		private List<DataModel.Stock.StockReport> getStockReport()
		{

			DataAccess _DataAccess = new DataAccess();
			string _Log = "";
			int _Num = 1;
			List<DataModel.Stock.StockReport> _StockReportList = new List<DataModel.Stock.StockReport>();
			try
			{
				this.btnLogin.PerformClick();
				DateTime _Dt = Convert.ToDateTime(this.dTPReport.Text);
				string _DayOfWeek = _Dt.DayOfWeek.ToString();
				string _WhereDate = _Dt.ToString("yyyyMMdd");
				int _Yestoday = _DayOfWeek == "Monday" ? -3 : -1;
				DateTime _YestodayDate = skipHoliday(_Dt.AddDays(_Yestoday));
				List<StockPicking> _ListStockPicking = _DataAccess.getStockPickingList().Where(x => x.Enabled == true).ToList();
				List<StockPicking> _StockPickingList = _ListStockPicking.Where(x => (x.Class == 1 || x.Class == 5 || x.Class == 6 || x.Class == 7 || x.Class == 8 || x.Class == 9 || x.Class == 10 || x.Class == 11) && Convert.ToInt32(x.Date) < Convert.ToInt32(_WhereDate)).ToList();
				_StockPickingList = _StockPickingList.Where(x => Convert.ToInt32(x.Date) > Convert.ToInt32(this.dtpReportStart.Value.ToString("yyyyMMdd"))).ToList();
				List<StockGroup> _ListStockGroup = _DataAccess.getStockGroupList();
				List<DataModel.Stock.Stock> _StockDayAllList = _DataAccess.getStockBySqlList(_WhereDate, "Date");
				decimal _AccumulatedGain = 0;
				List<StockResult> _StockResultList = _DataAccess.getStockResultList();

				if (chkApproved.Checked) _StockPickingList = _StockPickingList.Where(x => x.Approve == true).ToList();
				int _Increase = 0, _Decrease = 0;
				foreach (StockPicking s in _StockPickingList.OrderBy(x => x.Date).ToList())
				{
					//if (_Num == 1) this.txtStartDate.Text = s.Date;
					List<DataModel.Stock.Stock> _StockList = _DataAccess.getStockBySqlList(s.Date, "Date");
					DataModel.Stock.Stock _Stock = _StockList.Where(x => x.Code == s.Code).First();
					DataModel.Stock.StockReport _StockReport = new DataModel.Stock.StockReport();
					_StockReport.StockClass = GoodStockClass.Where(x => x.Key == s.Class) != null ? GoodStockClass.Where(x => x.Key == s.Class).First().Value : "";
					_StockReport.StartDate = s.Date;
					_StockReport.Code = s.Code;
					_StockReport.Name = _ListStockGroup.Where(x => x.Code == s.Code).First().Name;
					_StockReport.StartPrice = s.StartPrice;
					_StockReport.Remark = s.Remark;
					_StockReport.AccumulatedGain = Math.Round(Convert.ToDecimal(s.AccumulatedGain), 2);
					_StockReport.SecondDayOpeningPrice = _Stock.OpeningPrice;
					if (chkDay.Checked)
					{
						_WhereDate = Convert.ToString(Convert.ToDouble(s.Date) + Convert.ToDouble(txtManyDays.Text));
					}
					_StockReport.ClosingPrice = _StockResultList.Where(x => x.Date == _WhereDate && x.Code == s.Code).ToList().Count > 0 ? _StockResultList.Where(x => x.Date == _WhereDate && x.Code == s.Code).First().ClosingPrice : s.StartPrice;
					_AccumulatedGain = Math.Round(Convert.ToDecimal(_StockReport.ClosingPrice) - Convert.ToDecimal(_StockReport.StartPrice), 2);
					if (chkDay.Checked) _StockReport.AccumulatedGain = Math.Round(_AccumulatedGain / Convert.ToDecimal(_StockReport.StartPrice), 2);
					_StockReport.GainProfit = _AccumulatedGain * 1000;
					if (_StockReport.GainProfit > 0) _Increase++;
					else _Decrease++;
					_StockReport.StartVolumn = Convert.ToString(Convert.ToDecimal(s.StartVolume) / 1000);
					_StockReport.YesterdayVolumn = Convert.ToString(Convert.ToDecimal(s.YesterdayVolume) / 1000);
					_StockReport.StockPickingId = s.StockPickingId;
					_StockReport.Approve = s.Approve == true ? "核放" : "不核放";
					_StockReportList.Add(_StockReport);
					_Num++;
				}
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " getStockReport:第" + _Num + "筆" + ex.Message + "\r\n";
				logger.Error(_Log);
				this.txtErrMsg.Text += _Log;
			}
			return _StockReportList;

		}
		private void btnNotApproved_Click(object sender, EventArgs e)
		{

		}
		private void addStockPicking()
		{
			string _Log = "";
			try
			{
				DateTime _Dt = Convert.ToDateTime(this.dTPReport.Text);
				string _DayOfWeek = _Dt.DayOfWeek.ToString();
				int _Yestoday = _DayOfWeek == "Monday" ? -3 : -1;
				DateTime _YestodayDate = skipHoliday(_Dt.AddDays(_Yestoday));
				List<DataModel.Stock.Stock> _YestodayStockDayAllList = _DataAccess.getStockBySqlList(_YestodayDate.Date.ToString("yyyyMMdd"), "Date");
				List<DataModel.Stock.Stock> _StockDayAllList = _DataAccess.getStockBySqlList(_Dt.Date.ToString("yyyyMMdd"), "Date");
				DataModel.Stock.Stock _Stock = _StockDayAllList.Where(x => x.Code == this.txtPickingCode.Text).First();
				List<StockPicking> _ListStockPicking = _DataAccess.getStockPickingList().Where(x => x.Enabled == true).OrderByDescending(x => x.Date).ToList();
				List<StockPicking> _StockPickingList = new List<StockPicking>();
				DataModel.Stock.Stock _YestodayStock = _YestodayStockDayAllList.Where(x => x.Code == txtPickingCode.Text).First();
				StockPicking stockPicking = new StockPicking();
				stockPicking.Class = 1;
				stockPicking.Date = _Dt.ToString("yyyyMMdd");
				stockPicking.Enabled = true;
				stockPicking.Code = txtPickingCode.Text;
				stockPicking.StartPrice = _Stock.ClosingPrice;
				stockPicking.StartVolume = _Stock.TradeVolume;
				stockPicking.YesterdayVolume = _YestodayStock.TradeVolume;
				stockPicking.Remark = txtRemark.Text;
				stockPicking.StockPickingId = stockPicking.Class + txtPickingCode.Text.PadLeft(6, '0') + stockPicking.Date;
				stockPicking.CreateDate = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
				stockPicking.CreateId = "Admin";
				stockPicking.Approve = true;
				//if (_ListStockPicking.Where(x => x.StockPickingId == stockPicking.StockPickingId).Count() == 0)
				if (_ListStockPicking.Where(x => x.Code == stockPicking.Code && x.Date == stockPicking.Date).Count() > 0)
				{
					foreach (StockPicking sp in _ListStockPicking.Where(x => x.Code == stockPicking.Code && x.Date == stockPicking.Date).ToList())
					{
						_DataAccess.DeleteStockPicking(sp.Id);
					}
				}
				_StockPickingList.Add(stockPicking);
				_DataAccess.InsertStockPicking(_StockPickingList);
				MessageBox.Show("新增完成");
				_Log = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " insert stock picking " + txtPickingCode.Text + "ok.\r\n";
				this.txtErrMsg.Text += _Log;
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " addStockPicking:" + ex.Message + "\r\n";
				logger.Error(_Log);
				this.txtErrMsg.Text += _Log;
			}
		}
		private void btnAdd_Click(object sender, EventArgs e)
		{
			if (this.txtPickingCode.Text != "") addStockPicking();
			else updateStockPicking();

		}
		private void updateStockPicking()
		{
			DataAccess _DataAccess = new DataAccess();
			string _Log = "";
			try
			{
				//string  _Dt = DateTime.Now.ToString("yyyyMMdd");
				string _Dt = Convert.ToDateTime(this.dtpSuspendDate.Text).ToString("yyyyMMdd");
				List<StockPicking> _ListStockPicking = _DataAccess.getStockPickingList();
				foreach (DataGridViewRow row in this.dgvStockReport.Rows)
				{
					if (row.Selected)
					{
						string _StockPickingId = row.Cells["StockPickingId"].Value.ToString();
						string _SecondDayOpeningPrice = row.Cells["SecondDayOpeningPrice"].Value != null ? row.Cells["SecondDayOpeningPrice"].Value.ToString() : "";
						StockPicking _StockPicking = _ListStockPicking.Where(x => x.StockPickingId == _StockPickingId).First();
						_StockPicking.Remark = row.Cells["Remark"].Value != null ? row.Cells["Remark"].Value.ToString() : "";
						_StockPicking.Approve = cbApprove.Text == "核放" ? true : false;
						_StockPicking.OpeningPrice = _SecondDayOpeningPrice;
						_StockPicking.SuspendDate = chkSuspend.Checked ? _Dt : "";
						_DataAccess.UpdateStockPicking(_StockPicking);
						MessageBox.Show("更新完成");
					}
				}
				_Log = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " update stock picking  ok.\r\n";
				this.txtErrMsg.Text += _Log;
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " updateStockPicking:" + ex.Message + "\r\n";
				logger.Error(_Log);
				this.txtErrMsg.Text += _Log;
			}
		}
		private void txtPickingCode_TextChanged(object sender, EventArgs e)
		{
			if (this.txtPickingCode.Text != "") btnAdd.Text = "Add";
			else btnAdd.Text = "Update";
		}

		private void btnExport_Click(object sender, EventArgs e)
		{
			string _Log = "";
			try
			{
				List<DataModel.Stock.StockReport> _ListStockReport = getStockReport();
				string filepath = $@"./Export/{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
				//建立 xlxs 轉換物件
				XSLXHelper helper = new XSLXHelper();
				//取得轉為 xlsx 的物件
				var xlsx = helper.Export(_ListStockReport);
				//存檔至指定位置
				xlsx.SaveAs(filepath);
				_Log = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " export stock report  ok.\r\n";
				this.txtErrMsg.Text += _Log;
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " btnExport:" + ex.Message + "\r\n";
				logger.Error(_Log);
				this.txtErrMsg.Text += _Log;
			}
		}

		private void highLowToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormHighLow _FormHighLow = new FormHighLow();
			_FormHighLow.ShowDialog();
		}

		private void autoOrderToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormMain _FormMain = new FormMain();
			_FormMain.ShowDialog();
		}

		private void strengthWeaknessToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormStockGroupTrend _FormStockGroupTrend = new FormStockGroupTrend();
			_FormStockGroupTrend.ShowDialog();

		}

		private void btnInsertGroupTrend_Click(object sender, EventArgs e)
		{ 
			var t = new Task(insertStockGroupTrendList);
			t.Start();
		}
		private void insertStockGroupTrendList()
		{
			string _Log = "";
			try
			{
				DataAccess _DataAccess = new DataAccess();
				List<StockGroupTrend> _StockGroupTrendList = setStockGroupTrendList();
				_DataAccess.insertStockGroupTrend(_StockGroupTrendList);

			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " insertStockGroupTrendList:" + ex.Message + "\r\n";
				logger.Error(_Log);
			}
		}

		private void btnLineTrend_Click(object sender, EventArgs e)
		{
			string _Log = "";
			try
			{
				string _WhereDate = Convert.ToDateTime(this.dtpStockIndex.Value).ToString("yyyyMMdd");
				DataAccess _DataAccess = new DataAccess();
				List<StockGroupTotalCount> _StockGroupTotalCountList = _DataAccess.getStockGroupTotalCountList();
				List<StockGroupTrend> _StockGroupTrend = _DataAccess.getStockGroupTrendList().Where(x => x.Date == _WhereDate).ToList();
				List<DataModel.Stock.StockGroupTrend> _ListStockGroupTrend = _StockGroupTrend.Join(_StockGroupTotalCountList,
					c => c.StockGroupId, s => s.Id, (c, s) => (
					 new DataModel.Stock.StockGroupTrend
					 {
						 GroupName = s.GroupName,
						 Date = _WhereDate,
						 StrengthType = c.StrengthType == true ? "強勢" : "弱勢",
						 StrengthPercent = Convert.ToDecimal(c.StrengthPercent),
						 RedLight = c.RedLight.ToString(),
						 GreenLight = c.GreenLight.ToString(),
						 Remark = c.Remark

					 })).ToList();
				string _StrengthGroup = "強勢族群:";
				foreach (DataModel.Stock.StockGroupTrend s in _ListStockGroupTrend.Where(x=>x.StrengthType=="強勢"&&x.StrengthPercent>0.1m).OrderByDescending(x=>x.StrengthPercent).ToList()) {
					_StrengthGroup+=s.GroupName+";";
				}
			    string _WeaknessGroup = "弱勢族群:";
				foreach (DataModel.Stock.StockGroupTrend s in _ListStockGroupTrend.Where(x => x.StrengthType == "弱勢" && x.StrengthPercent > 0.1m).OrderByDescending(x => x.StrengthPercent).ToList())
				{
					_WeaknessGroup += s.GroupName + ";";
				}
				List<WebApiService.Models.StockLineNotify> _StockLineNotifyList = _DataAccess.getStockLineNotifyList();
				foreach (WebApiService.Models.StockLineNotify s in _StockLineNotifyList.Where(x => x.NotifyClass == "Trend" && x.Enable == true).ToList())
				{
					string _Msg = _StrengthGroup+"\r\n"+ _WeaknessGroup;
					CallLineNotifyApi(s.Token, _Msg);
				}
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " btnLineTrend_Click:" + ex.Message + "\r\n";
				logger.Error(_Log);
			}
		}

		private void threeInvestmentToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormThreeInstitutional _FormThreeInstitutional = new FormThreeInstitutional();
			_FormThreeInstitutional.ShowDialog();
		}

		private void maintainToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormMaintain _FormMaintain = new FormMaintain();
			_FormMaintain.ShowDialog();
		}

		private void btnLineThree_Click(object sender, EventArgs e)
		{
			string _Log = "";
			try
			{
				string _WhereDate = Convert.ToDateTime(this.dtpStockIndex.Value).ToString("yyyyMMdd");
				DateTime _Dt = Convert.ToDateTime(this.dtpStockIndex.Value);
				int _Yestoday = DateTime.Now.DayOfWeek.ToString() == "Monday" ? -3 : -1;
				DateTime _YestodayDate = skipHoliday(_Dt.AddDays(_Yestoday));
				_WhereDate= _YestodayDate.ToString("yyyyMMdd");
				DataAccess _DataAccess = new DataAccess();
				List<DataModel.Stock.Stock> _StockDayAllList = _DataAccess.getStockBySqlList(_WhereDate, "Date");
				List<DataModel.Stock.StockThreeInstitutional> _ListStockThreeInstitutional = new List<DataModel.Stock.StockThreeInstitutional>();
				int _Rank = 1; 
				string _Foreign =string.Format( "日期{0} 外資買超:\r\n",_WhereDate);
				foreach (DataModel.Stock.Stock s in _StockDayAllList.OrderByDescending(x => Convert.ToDecimal(x.ForeignInvestment)).ToList().Take(10).ToList())
				{
					_Foreign = _Foreign+string.Format("第{0}名,{1}:{2},{3};\r\n",_Rank,s.Code,s.Name,s.ForeignInvestment);
					_Rank++;
				}
				_Rank = 1;
				string _Investment = string.Format("日期{0} 投信買超:\r\n", _WhereDate);
				foreach (DataModel.Stock.Stock s in _StockDayAllList.OrderByDescending(x => Convert.ToDecimal(x.Investment)).ToList().Take(10).ToList())
				{
					_Investment = _Investment + string.Format("第{0}名,{1}:{2},{3};\r\n", _Rank, s.Code, s.Name,s.Investment);
					_Rank++;
				}
				_Rank = 1;
				string _ForeignSell = string.Format("日期{0} 外資賣超:\r\n", _WhereDate);
				foreach (DataModel.Stock.Stock s in _StockDayAllList.OrderBy(x => Convert.ToDecimal(x.ForeignInvestment)).ToList().Take(10).ToList())
				{
					_ForeignSell = _ForeignSell + string.Format("第{0}名,{1}:{2},{3};\r\n", _Rank, s.Code, s.Name,s.ForeignInvestment);
					_Rank++;
				}
				_Rank = 1;
				string _InvestmentSell = string.Format("日期{0} 投信賣超:\r\n", _WhereDate);
				foreach (DataModel.Stock.Stock s in _StockDayAllList.OrderBy(x => Convert.ToDecimal(x.Investment)).ToList().Take(10).ToList())
				{
					_InvestmentSell = _InvestmentSell + string.Format("第{0}名,{1}:{2},{3};\r\n", _Rank, s.Code, s.Name,s.Investment);
					_Rank++;
				}
				List<WebApiService.Models.StockLineNotify> _StockLineNotifyList = _DataAccess.getStockLineNotifyList();
				foreach (WebApiService.Models.StockLineNotify s in _StockLineNotifyList.Where(x => x.NotifyClass == "Three" && x.Enable == true).ToList())
				{ 
					CallLineNotifyApi(s.Token, _Foreign);
					CallLineNotifyApi(s.Token, _Investment);
					CallLineNotifyApi(s.Token, _ForeignSell);
					CallLineNotifyApi(s.Token, _InvestmentSell);
				}
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " btnLineThree_Click:" + ex.Message + "\r\n";
				logger.Error(_Log);
			}
		}

		private List<StockGroupTrend> setStockGroupTrendList()
		{
			string _Log = "", _GroupName = "";
			List<StockGroupTrend> _StockGroupTrendList = new List<StockGroupTrend>();
			DataAccess _DataAccess = new DataAccess();
			int _No = 0;
			List<StockGroup> _StockGroupList = _DataAccess.getStockGroupList();
			List<StockGroupTotalCount> _StockGroupTotalCountList = _DataAccess.getStockGroupTotalCountList();
			string _WhereDate = Convert.ToDateTime(this.dtpStockIndex.Value).ToString("yyyyMMdd");
			List<DataModel.Stock.Stock> _StockDayAllList = _DataAccess.getStockBySqlList(_WhereDate, "Date");
			List<DataModel.Stock.Stock> _StockDayAllStengthList = _StockDayAllList.Where(x => Convert.ToDecimal(x.Gain) > 3).ToList();
			List<DataModel.Stock.Stock> _StockDayAllWeaknessList = _StockDayAllList.Where(x => Convert.ToDecimal(x.Gain) < -3).ToList();
			var _StockList = _StockDayAllStengthList.Join(_StockGroupList,
					c => c.Code, x => x.Code, (c, x) => (
					 new
					 {
						 Code = c.Code,
						 Name = c.Name,
						 Class = x.Class,
						 Gain = c.Gain,

					 })).ToList();
			var _StockWeaknessList = _StockDayAllWeaknessList.Join(_StockGroupList,
					c => c.Code, x => x.Code, (c, x) => (
					 new
					 {
						 Code = c.Code,
						 Name = c.Name,
						 Class = x.Class,
						 Gain = c.Gain,

					 })).ToList();
			foreach (StockGroupTotalCount s in _StockGroupTotalCountList)
			{
				try
				{
					var _StockByClassList = _StockList.Where(x => x.Class == s.GroupName).ToList();
					StockGroupTrend _StockGroupTrend = new StockGroupTrend();
					_StockGroupTrend.StockGroupId = s.Id;
					_StockGroupTrend.Date = _WhereDate;
					_StockGroupTrend.UpdateTime = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
					_StockGroupTrend.RedLight = _StockByClassList.Where(x => Convert.ToDecimal(x.Gain) > 9.5m).Count();
					_StockGroupTrend.GreenLight = _StockByClassList.Where(x => Convert.ToDecimal(x.Gain) < -9.5m).Count();
					string _Remark = "";
					foreach (var dms in _StockByClassList)
					{
						_Remark = _Remark + dms.Code + ":" + dms.Name + ";";
					}
					_StockGroupTrend.Remark = _Remark;
					_StockGroupTrend.StrengthType = true;
					decimal _StrengthPercent = Math.Round(Convert.ToDecimal(_StockByClassList.Count) / Convert.ToDecimal(s.TotalCount), 2);
					_StockGroupTrend.StrengthPercent = _StrengthPercent;
					if (_StockByClassList.Count > 0)
					{
						_GroupName = s.GroupName;
						_StockGroupTrendList.Add(_StockGroupTrend);
					}
					_No++;
				}
				catch (Exception ex)
				{
					_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " setStockGroupTrendList:" + _GroupName + ex.Message + "\r\n";
					logger.Error(_Log);
				}
			}
			foreach (StockGroupTotalCount s in _StockGroupTotalCountList)
			{
				try
				{
					var _StockByClassWeaknessList = _StockWeaknessList.Where(x => x.Class == s.GroupName).ToList();
					StockGroupTrend _StockGroupTrend = new StockGroupTrend();
					_StockGroupTrend.StockGroupId = s.Id;
					_StockGroupTrend.Date = _WhereDate;
					_StockGroupTrend.UpdateTime = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
					_StockGroupTrend.RedLight = _StockByClassWeaknessList.Where(x => Convert.ToDecimal(x.Gain) > 9.5m).Count();
					_StockGroupTrend.GreenLight = _StockByClassWeaknessList.Where(x => Convert.ToDecimal(x.Gain) < -9.5m).Count();
					string _Remark = "";
					foreach (var dms in _StockByClassWeaknessList)
					{
						_Remark = _Remark + dms.Code + ":" + dms.Name + ";";
					}
					_StockGroupTrend.Remark = _Remark;

					_StockGroupTrend.StrengthType = false;
					decimal _StrengthPercentWeakness = Math.Round(Convert.ToDecimal(_StockByClassWeaknessList.Count) / Convert.ToDecimal(s.TotalCount), 2);
					_StockGroupTrend.StrengthPercent = _StrengthPercentWeakness;
					if (_StockByClassWeaknessList.Count > 0)
					{
						_GroupName = s.GroupName;
						_StockGroupTrendList.Add(_StockGroupTrend);
					}
					_No++;
				}
				catch (Exception ex)
				{
					_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " setStockGroupTrendList:" + _GroupName + ex.Message + "\r\n";
					logger.Error(_Log);
				}
			}
			MethodInvoker mi = new MethodInvoker(this.UpdateUI);
			this.BeginInvoke(mi, null);
			return _StockGroupTrendList;
		}
		/// <summary>
		/// Json檔 起漲放量選股策略賴通知
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnLineGoodbyJson_Click(object sender, EventArgs e)
		{

			string _Log = "";
			try
			{
				DateTime _Dt = Convert.ToDateTime(this.dtpGoodDate.Text);
				List<StockPicking> _StockPickingList = _DataAccess.getStockPickingList().Where(x => x.Enabled == true).ToList();
				_StockPickingList = _StockPickingList.Where(x => x.Date == _Dt.ToString("yyyyMMdd")).ToList();
				List<StockGroup> _StockGroupList = _DataAccess.getStockGroupList();
				var _StockList = _StockPickingList.Join(_StockGroupList, x => x.Code, y => y.Code, (x, y) => new
				{
					Code = x.Code,
					Name = y.Name,
					Class = x.Class
				}).ToList();

				foreach (KeyValuePair<int, string> item in GoodStockClass)
				{
					List<DataModel.Stock.Stock> stockList = new List<DataModel.Stock.Stock>();
					foreach (var s in _StockList.Where(x => x.Class == item.Key).ToList())
					{
						DataModel.Stock.Stock st = new DataModel.Stock.Stock(s.Code, s.Name, "", "", "", "", "", "", "","","","");
						stockList.Add(st);
					}
					if (stockList.Count > 0) lineAIStock(stockList, "Good", item.Value, _Dt.ToString("yyyyMMdd"));
				}

			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " LineGoodbyJson:" + ex.Message + "\r\n";
				logger.Error(_Log);
			}
		}

		private void btnAdminLogin_Click(object sender, EventArgs e)
		{
			this.txtAccount.Text = "Rain";
			this.txtPassword.Text = "B1050520";
			string _Username = Tool.Encrypt(this.txtAccount.Text, "20220801", "B1050520");
			string _Password = Tool.Encrypt(this.txtPassword.Text, "20220801", "B1050520");
			if (_Username == ConfigurationManager.AppSettings["Account"] && _Password == ConfigurationManager.AppSettings["Password"])
			{
				this.pnAdmin.Enabled = true;
				this.btnLineBad.Enabled = true;
				this.btnLineCtuStock.Enabled = true;
				this.btnLineGoodStock.Enabled = true;
			}
		}

		private void btnBad_Click(object sender, EventArgs e)
		{
			Stopwatch sw = new Stopwatch();
			sw.Reset();
			sw.Start();
			pgBarBad.Style = ProgressBarStyle.Marquee;
			pgBarBad.MarqueeAnimationSpeed = 30;
			pgBarBad.Show();
			var s = Task.Run(() => getBadStock());
			var _StockList = s.Result;
			if (chkPickingBad.Checked) insertStockPicking(_StockList, 4);
			sw.Stop();
			logger.Info("爆量出貨:" + sw.ElapsedMilliseconds.ToString() + "毫秒,筆數:" + _StockList.Count);
		}
		/// <summary>
		/// 連續小黑
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCtu_Click(object sender, EventArgs e)
		{
			Stopwatch sw = new Stopwatch();
			sw.Reset();
			sw.Start();
			pgBarCtu.Style = ProgressBarStyle.Marquee;
			pgBarCtu.MarqueeAnimationSpeed = 30;
			pgBarCtu.Show();
			GainType = this.cbGain.Text != "" ? this.cbGain.Text : "漲";
			var s = Task.Run(() => getCtuStock());
			//var s = Task.Run(() => getCtuStockList());
			var _StockList = s.Result;
			if (chkPickingCtu.Checked) insertStockPicking(_StockList, 2);
			sw.Stop();
			logger.Info("連續型態:" + sw.ElapsedMilliseconds.ToString() + "毫秒,筆數:" + _StockList.Count);
		}
		private List<DataModel.Stock.Stock> getCtuStock()
		{
			string _Log = "";
			CtuStock = "";
			List<DataModel.Stock.Stock> _StockList = new List<DataModel.Stock.Stock>();
			try
			{
				_StockList = getCtuStockList();

				foreach (DataModel.Stock.Stock s in _StockList)
				{
					CtuStock = CtuStock + s.Code + ":" + s.Name + ";";
				}
				MethodInvoker mi = new MethodInvoker(this.UpdateUICtuStock);
				this.BeginInvoke(mi, null);

			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " GetCtuStock:" + ex.Message + "\r\n";
				logger.Error(_Log);
			}
			return _StockList;
		}

		/// <summary>
		/// 進度條
		/// </summary>
		private void UpdateUI()
		{
			this.progressBar1.PerformStep();
			List<StockGroup> _StockGroupList = _DataAccess.getStockGroupList();
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
				DataAccess _DataAccess = new DataAccess();
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
					_StockGroup.UpdateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
					_StockGroupList.Add(_StockGroup);
				}
				btnLogin.PerformClick();
				_DataAccess.InsertStockGroup(_StockGroupList);
				MessageBox.Show("ok");
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " btnImport:" + ex.Message + "\r\n";
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
				DataAccess _DataAccess = new DataAccess();
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
				List<StockIndex> _ListStock = _DataAccess.getStockIndexList();
				s = _ListStock.OrderByDescending(x => x.Date).First();
				//s.Id = _ListStock.OrderByDescending(x => x.Date).First().Id;
				s.ContinueName = "";
				s.TX = Common.Job.GetTaiwanFutures(_MTX_URL, _Id, true);
				s.TX_High = "";
				s.TX_Open = "";
				s.TX_QuoteChange = Common.Job.GetTaiwanFutures(_MTX_URL, _Change, true).Replace('▼', ' ').Replace('▲', '+');
				s.TX_QuotePercent = Common.Job.GetTaiwanFutures(_MTX_URL, _Percent, true);
				s.TX_Volume = Common.Job.GetTaiwanFutures(_MTX_URL, _Volume, true).Replace('口', ' ');
				DateTime _Dt = Convert.ToDateTime(this.dtpStockIndex.Text);
				int _Yestoday = DateTime.Now.DayOfWeek.ToString() == "Monday" ? -3 : -1;
				DateTime _YestodayDate = skipHoliday(_Dt.AddDays(_Yestoday));
				// string _TopIndex = getTopIndex(DateTime.Now.AddDays(_Yestoday).ToString("yyyyMMdd"));
				string _TopIndex = _ListStock.Where(x => x.Date == _YestodayDate.ToString("yyyyMMdd")).First().TAIEX;
				s.TAIEX_High = _StockPrice.HighPrice;
				s.TAIEX_Open = _StockPrice.OpenPrice;
				s.TAIEX_QuoteChange = Convert.ToString(Math.Round(Convert.ToDouble(_StockPrice.Price) - Convert.ToDouble(_TopIndex), 2));
				s.TAIEX_QuotePercent = Convert.ToString(Math.Round(Convert.ToDouble(s.TAIEX_QuoteChange) * 100 / Convert.ToDouble(_StockPrice.Price), 2));
				s.TAIEX = s.TAIEX_QuoteChange != "0" ? _StockPrice.Price : "0";
				s.Volume = _StockPrice.TotalDealQty;
				s.TPEx = Common.Job.GetTaiwanFutures(_TPEx_URL, _Id, true);
				s.TPEx_QuoteChange = Common.Job.GetTaiwanFutures(_TPEx_URL, _Change, true).Replace('▼', ' ').Replace('▲', '+');
				s.TPEx_QuotePercent = Common.Job.GetTaiwanFutures(_TPEx_URL, _Percent, true);

				if (s.TAIEX_QuoteChange == "0")
				{
					s.TAIEX = "0";
					s.TPEx = "0";
					s.TX = "0";
				}
				_DataAccess.UpdateStockIndex(s);
				_Log = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " update stock index ok.";
				this.txtErrMsg.Text += _Log;
				bTAIEX = true;
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " GetIndexToInsert:" + ex.Message + "\r\n";
				logger.Error(_Log);
				this.txtErrMsg.Text += _Log;
			}
		}
		private DateTime skipHoliday(DateTime yestoday)
		{
			string _DayOfWeek = yestoday.DayOfWeek.ToString();
			if (HolidayList.Where(x => x.HolidayDate == yestoday.Date.ToString("MMdd")).Count() > 0 || _DayOfWeek == "Sunday" || _DayOfWeek == "Saturday")
				return skipHoliday(yestoday.AddDays(-1));
			else return yestoday;
		}
	}
}
