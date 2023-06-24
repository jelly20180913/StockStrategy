using Newtonsoft.Json;
using NLog;
using NLog.Fluent;
using StockStrategy.BBL;
using StockStrategy.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebApiService.Models;

namespace StockStrategy
{
	public partial class FormMaintain : Form
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		List<Holiday> HolidayList = new List<Holiday>();
		public FormMaintain()
		{
			InitializeComponent();
		}

		private void btnAddPicking_Click(object sender, EventArgs e)
		{
			Stopwatch sw = new Stopwatch();
			sw.Reset();
			sw.Start();
			string _Log = "";
			int _N = 0;
			StreamReader r = new StreamReader("Json\\Setting.json");
			string jsonString = r.ReadToEnd();
			DateTime _DtStart = this.dtpStart.Value;
			DateTime _DtEnd = this.dtpEnd.Value; 
			try
			{
				int _Start = Convert.ToInt32(_DtStart.Date.ToString("yyyyMMdd"));
				int _End = Convert.ToInt32(_DtEnd.Date.ToString("yyyyMMdd"));
				
				List<StockStrategy.Model.GoodStock> _GoodStockList = JsonConvert.DeserializeObject<List<StockStrategy.Model.GoodStock>>(jsonString);
				while (_Start < _End)
				{
					foreach (StockStrategy.Model.GoodStock g in _GoodStockList)
					{
						//非同步的話會導致重複key,所以改為同步
						//	var s = Task.Run(() => getGoodStock(g));
						var s = getGoodStock(g, _DtStart);
						if (s.Count > 0) insertStockPicking(s, g.Class, _DtStart);
						_N += s.Count;
					}
					int _AddDay = _DtStart.DayOfWeek.ToString() == "Friday" ?  3 :  1;
					_DtStart = _DtStart.AddDays(_AddDay);
					_Start = Convert.ToInt32(_DtStart.Date.ToString("yyyyMMdd"));
				}
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " btnAddPicking_Click:" + ex.Message + "\r\n";
				logger.Error(_Log);
			}
			sw.Stop(); 
			_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " insert picking ok\r\n"+ sw.ElapsedMilliseconds.ToString() + "毫秒,筆數:" + _N;
			this.txtErrMsg.Text = _Log;
		}
		private List<DataModel.Stock.Stock> getGoodStock(StockStrategy.Model.GoodStock goodStock,DateTime dt)
		{
			string _Log = "";
			string GoodStock = ""; 
			List<DataModel.Stock.Stock> _StockList = new List<DataModel.Stock.Stock>();
			try
			{
				_StockList = getGoodStockList(goodStock,dt);

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
		/// <summary>
		/// 新增選股
		/// </summary>
		/// <param name="stockList"></param>
		/// <param name="iclass"></param>
		private void insertStockPicking(List<DataModel.Stock.Stock> stockList, int iclass,DateTime dt)
		{
			//停用日
			string _Log = "";
			try
			{
				DataAccess _DataAccess = new DataAccess();
				DateTime _Dt = dt;
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
					if (_ListStockPicking.Where(x => x.Code == stockPicking.Code && x.Date == stockPicking.Date).Count() > 0)
					{
						foreach (StockPicking sp in _ListStockPicking.Where(x => x.Code == stockPicking.Code && x.Date == stockPicking.Date).ToList())
						{ 
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
		private List<DataModel.Stock.Stock> getGoodStockList(StockStrategy.Model.GoodStock goodStock,DateTime dt)
		{
			string _Log = "";
			DataAccess _DataAccess = new DataAccess();
			DateTime _Dt =dt;//變數
			string _Code = "";
			string _WhereDate = _Dt.ToString("yyyyMMdd");
			string _DayOfWeek = _Dt.DayOfWeek.ToString();
			int _Yestoday = _DayOfWeek == "Monday" ? -3 : -1;
			DateTime _YestodayDate = skipHoliday(_Dt.AddDays(_Yestoday));

			List<DataModel.Stock.Stock> _YestodayStockDayAllList = _DataAccess.getStockBySqlList(_YestodayDate.Date.ToString("yyyyMMdd"), "Date");
			List<DataModel.Stock.Stock> _StockDayAllList = _DataAccess.getStockBySqlList(_WhereDate, "Date");
			List<DataModel.Stock.Stock> _StockList = new List<DataModel.Stock.Stock>();
			decimal _Gain =  3;
			int _Volumn =  700000;
			int multiple =   5;
			int _PreDays =   10;
			int _LessVolumn =  1000000;
			int _PreDays2 =  10;
			int _MoreGain =   10;
			bool _ChkLess = false;
			bool _ChkNotHigherThan = false;
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
		private DateTime skipHoliday(DateTime yestoday)
		{
			string _DayOfWeek = yestoday.DayOfWeek.ToString();
			if (HolidayList.Where(x => x.HolidayDate == yestoday.Date.ToString("MMdd")).Count() > 0 || _DayOfWeek == "Sunday" || _DayOfWeek == "Saturday")
				return skipHoliday(yestoday.AddDays(-1));
			else return yestoday;
		}

		private void FormMaintain_Load(object sender, EventArgs e)
		{
			DataAccess _DataAccess = new DataAccess();
			string _Date = DateTime.Now.ToString("yyyy");
			HolidayList = _DataAccess.getHolidayList().Where(x => x.Enable == true).Where(x => x.Year == _Date || x.HolidayDate == "0000").ToList();
		}
	}
}
