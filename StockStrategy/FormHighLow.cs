using NLog;
using StockStrategy.BBL;
using StockStrategy.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebApiService.Models;

namespace StockStrategy
{
	public partial class FormHighLow : Form
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		DataAccess _DataAccess = new DataAccess();
		public FormHighLow()
		{
			InitializeComponent();
		}

		private void btnQuery_Click(object sender, EventArgs e)
		{
			List<StockGroup> _StockGroupList = _DataAccess.getStockGroupList();
			this.progressBar1.Maximum = _StockGroupList.Count;
			this.progressBar1.Step = 1;
			var t = new Task(insertStockHighLowList);
			t.Start();

		}
		private void insertStockHighLowList()
		{
			string _Log = "";
			try
			{
				DataAccess _DataAccess = new DataAccess();
				List<StockHighLow> _StockHighLowList = setStockHighLowList();
				_DataAccess.insertStockHighLow(_StockHighLowList);

			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " insertStockHighLowList:" + ex.Message + "\r\n";
				logger.Error(_Log);
			}
		}
		private List<StockHighLow> setStockHighLowList()
		{

			string _Log = "", _Code = "";
			List<StockHighLow> _StockHighLowList = new List<StockHighLow>();
			DataAccess _DataAccess = new DataAccess();
			Stopwatch sw = new Stopwatch();
			sw.Reset();
			sw.Start();
			int _No = 0;


			List<StockGroup> _StockGroupList = _DataAccess.getStockGroupList();
			string _UpdateTime = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
			string _DateRange = DateTime.Now.AddDays(-5).ToString("yyyyMMdd");
			foreach (StockGroup s in _StockGroupList)
			{
				try
				{
					List<DataModel.Stock.Stock> _StockList = _DataAccess.getStockBySqlList(s.Code, "Code").ToList();
				//	DataModel.Stock.Stock _Stock = _StockList.Where(x => x.Date == DateTime.Now.Date.ToString("yyyyMMdd")).First();
					StockHighLow _StockHighLow = new StockHighLow();
					_StockHighLow.Code = s.Code;
					_StockHighLow.Name = s.Name;
					_StockHighLow.UpdateTime = _UpdateTime;
					_StockHighLow.HighestPrice = _StockList.OrderByDescending(x => Convert.ToDecimal(x.ClosingPrice)).Take(1).First().ClosingPrice;
					_StockHighLow.LowestPrice = _StockList.OrderBy(x => Convert.ToDecimal(x.ClosingPrice)).Take(1).First().ClosingPrice;
					_StockHighLow.HighestPriceDate = _StockList.OrderByDescending(x => Convert.ToDecimal(x.ClosingPrice)).Take(1).First().Date;
					_StockHighLow.LowestPriceDate = _StockList.OrderBy(x => Convert.ToDecimal(x.ClosingPrice)).Take(1).First().Date;
					decimal _HighLowRatio = 0m;
					if (Convert.ToInt32(_StockHighLow.HighestPriceDate)>Convert.ToInt32(_StockHighLow.LowestPriceDate))
						_HighLowRatio = Convert.ToDecimal(Math.Round(Convert.ToDecimal(_StockHighLow.HighestPrice) / Convert.ToDecimal(_StockHighLow.LowestPrice), 2));
					else _HighLowRatio = Convert.ToDecimal(Math.Round(Convert.ToDecimal(_StockHighLow.LowestPrice) / Convert.ToDecimal(_StockHighLow.HighestPrice), 2));
					_StockHighLow.HighLowRatio = _HighLowRatio;
					_StockHighLow.CurrentPrice =  _StockList.OrderByDescending(x=>x.Date).First().ClosingPrice;
					_StockHighLow.NewRecord =Convert.ToInt32(_StockHighLow.HighestPriceDate)>Convert.ToInt32(_DateRange)?true:false;

					if (_StockHighLow != null)
					{
						_Code = _StockHighLow.Code;
						_StockHighLowList.Add(_StockHighLow);
					}
					_No++;

					MethodInvoker mi = new MethodInvoker(this.UpdateUI);
					this.BeginInvoke(mi, null);

				}
				catch (Exception ex)
				{
					_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " btnInsertStockHighLow_Click:" + _Code + ex.Message + "\r\n";
					logger.Error(_Log);
				}
			}
			sw.Stop();
			logger.Info(sw.ElapsedMilliseconds.ToString() + "毫秒,筆數:" + _No + ":" + _StockHighLowList.Count);
			//MessageBox.Show(sw.ElapsedMilliseconds.ToString() + "毫秒,筆數:" + _No);
			return _StockHighLowList;
		}
		private void UpdateUI()
		{
			this.progressBar1.PerformStep();
			List<StockGroup> _StockGroupList = _DataAccess.getStockGroupList();
			double _Count = _StockGroupList.Count, _Prog = this.progressBar1.Value;
			this.lbPercent.Text = Math.Floor(((_Prog / _Count) * 100)).ToString() + "%";
			this.lbPercent.Refresh();
			//this.lbMsg.Text = "Insert Stock High Low ok";
		}

		private void btnGetData_Click(object sender, EventArgs e)
		{
			string _Log = "";
			try
			{
				DataAccess _DataAccess = new DataAccess();
				List<StockGroup> _StockGroupList = _DataAccess.getStockGroupList();
				List<StockHighLow> _StockHighLow=_DataAccess.getStockHighLowList();
				List<DataModel.Stock.StockHighLow> _ListStockHighLow = _StockHighLow.Join(_StockGroupList,
					c => c.Code, s => s.Code, (c, s) => (
					 new DataModel.Stock.StockHighLow
					 {
						 Code = c.Code,
						 Name = c.Name,
						 HighestPrice = Convert.ToDecimal(c.HighestPrice),
						 LowestPrice = Convert.ToDecimal(c.LowestPrice),
						 HighestDate = c.HighestPriceDate,
						 LowestDate = c.LowestPriceDate,
						 HighLowRate = Convert.ToDecimal(c.HighLowRatio),
						 NewRecord = Convert.ToBoolean(c.NewRecord),
						 Class = s.Class

					 })).ToList();
					

				SortableBindingList<DataModel.Stock.StockHighLow> _StockHighLowList = new SortableBindingList<DataModel.Stock.StockHighLow>(_ListStockHighLow);
				this.dgvHighLow.DataSource = _StockHighLowList;
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " btnGetData_Click:"  + ex.Message + "\r\n";
				logger.Error(_Log);
			}
		}

		private void btnExport_Click(object sender, EventArgs e)
		{
			string _Log = "";
			try
			{
				DataAccess _DataAccess = new DataAccess();
				List<StockGroup> _StockGroupList = _DataAccess.getStockGroupList();
				List<StockHighLow> _StockHighLow = _DataAccess.getStockHighLowList();
				List<DataModel.Stock.StockHighLow> _ListStockHighLow = _StockHighLow.Join(_StockGroupList,
					c => c.Code, s => s.Code, (c, s) => (
					 new DataModel.Stock.StockHighLow
					 {
						 Code = c.Code,
						 Name = c.Name,
						 HighestPrice = Convert.ToDecimal(c.HighestPrice),
						 LowestPrice = Convert.ToDecimal(c.LowestPrice),
						 HighestDate = c.HighestPriceDate,
						 LowestDate = c.LowestPriceDate,
						 HighLowRate = Convert.ToDecimal(c.HighLowRatio),
						 NewRecord = Convert.ToBoolean(c.NewRecord),
						 Class = s.Class

					 })).OrderByDescending(x=>x.HighLowRate).ToList();
				string filepath = $@"./Export/{DateTime.Now.ToString("yyyyMMddHHmmss")}_StockHighLow.xlsx";
				//建立 xlxs 轉換物件
				XSLXHelper helper = new XSLXHelper();
				//取得轉為 xlsx 的物件
				var xlsx = helper.Export(_ListStockHighLow);
				//存檔至指定位置
				xlsx.SaveAs(filepath);
				_Log = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " export stock high low  ok.\r\n"; 
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " btnExport:" + ex.Message + "\r\n";
				logger.Error(_Log); 
			}
		}
	}
}