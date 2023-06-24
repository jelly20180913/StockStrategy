using NLog;
using StockStrategy.BBL;
using StockStrategy.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebApiService.Models;

namespace StockStrategy
{
	public partial class FormStockGroupTrend : Form
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		DataAccess _DataAccess = new DataAccess();
		public FormStockGroupTrend()
		{
			InitializeComponent();
		}

		private void btnQuery_Click(object sender, EventArgs e)
		{
			List<StockGroup> _StockGroupList = _DataAccess.getStockGroupList();
			this.progressBar1.Maximum = _StockGroupList.Count;
			this.progressBar1.Step = 1;
			var t = new Task(insertStockGroupTrendList);
			t.Start();
		}
		/// <summary>
		/// 寫入強弱族群表
		/// </summary>
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
		/// <summary>
		/// 取得強弱族群表
		/// </summary>
		/// <returns></returns>
		private List<StockGroupTrend> setStockGroupTrendList()
		{

			string _Log = "", _GroupName = "";
			List<StockGroupTrend> _StockGroupTrendList = new List<StockGroupTrend>();
			DataAccess _DataAccess = new DataAccess(); 
			int _No = 0;
			List<StockGroup> _StockGroupList = _DataAccess.getStockGroupList();
			List<StockGroupTotalCount> _StockGroupTotalCountList = _DataAccess.getStockGroupTotalCountList();
			string _WhereDate = Convert.ToDateTime(this.dtpDate.Value).ToString("yyyyMMdd");
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
					foreach(var dms in _StockByClassList) {
						_Remark = _Remark + dms.Code + ":" + dms.Name + ";";
					}
					_StockGroupTrend.Remark = _Remark;	
					_StockGroupTrend.StrengthType = true;
					decimal _StrengthPercent = Math.Round(Convert.ToDecimal(_StockByClassList.Count) / Convert.ToDecimal(s.TotalCount), 2);
					_StockGroupTrend.StrengthPercent = _StrengthPercent;
					if (_StockByClassList.Count>0)
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
		private void UpdateUI()
		{
			MessageBox.Show("insert stock group trend ok");
		}
		private void btnGetData_Click(object sender, EventArgs e)
		{
			string _Log = "";
			try
			{
				string _WhereDate =Convert.ToDateTime( this.dtpDate.Value).ToString("yyyyMMdd");
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
						 Remark=c.Remark

					 })).ToList(); 
				SortableBindingList<DataModel.Stock.StockGroupTrend> _StockGroupTrendList = new SortableBindingList<DataModel.Stock.StockGroupTrend>(_ListStockGroupTrend.Where(x => x.StrengthType == this.cbType.SelectedItem.ToString()).OrderByDescending(x=>x.StrengthPercent).ToList());
				this.dgvGroupTrend.DataSource = _StockGroupTrendList;
				this.dgvGroupTrend.Columns["StrengthType"].Width = 40;
				this.dgvGroupTrend.Columns["StrengthPercent"].Width = 40;
				this.dgvGroupTrend.Columns["RedLight"].Width = 40;
				this.dgvGroupTrend.Columns["GreenLight"].Width = 40; 
				this.dgvGroupTrend.Columns["Remark"].Width = 600;
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " btnGetData_Click:" + ex.Message + "\r\n";
				logger.Error(_Log);
			}
		}

		private void btnExport_Click(object sender, EventArgs e)
		{
			string _Log = "";
			try
			{
				string _WhereDate = Convert.ToDateTime(this.dtpDate.Value).ToString("yyyyMMdd");
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
						 GreenLight = c.GreenLight.ToString()

					 })).ToList(); 
				string filepath = $@"./Export/{DateTime.Now.ToString("yyyyMMddHHmmss")}_StockGroupTrend.xlsx";
				//建立 xlxs 轉換物件
				XSLXHelper helper = new XSLXHelper();
				//取得轉為 xlsx 的物件
				var xlsx = helper.Export(_ListStockGroupTrend);
				//存檔至指定位置
				xlsx.SaveAs(filepath);
				_Log = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " export stock group trend ok.\r\n";
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " btnExport:" + ex.Message + "\r\n";
				logger.Error(_Log);
			}
		}

		private void FormStockGroupTrend_Load(object sender, EventArgs e)
		{
			this.cbType.SelectedIndex = 0;
		}
	}
}
