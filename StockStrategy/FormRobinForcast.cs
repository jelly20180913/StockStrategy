using DataModel.Stock;
using DocumentFormat.OpenXml.Drawing;
using NLog;
using StockStrategy.BBL;
using StockStrategy.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebApiService.Models;
using StockIndex = WebApiService.Models.StockIndex;

namespace StockStrategy
{
	public partial class FormRobinForcast : Form
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		DataAccess _DataAccess = new DataAccess();
		public FormRobinForcast()
		{
			InitializeComponent();
		}

		private void btnGetData_Click(object sender, EventArgs e)
		{
			string _Log = "";
			try
			{
				DateTime _Dt = Convert.ToDateTime(this.dTPReport.Text);
				string _DayOfWeek = _Dt.DayOfWeek.ToString();
				string _WhereDate = _Dt.ToString("yyyyMMdd"); 
				List<StockIndexForcast> _StockIndexForcastList = _DataAccess.getStockIndexForcastList();
				List<StockIndex> _StockIndexList = _DataAccess.getStockIndexList();
				_StockIndexForcastList = _StockIndexForcastList.Where(x => Convert.ToInt32(x.Date) < Convert.ToInt32(_WhereDate)).ToList();
				_StockIndexForcastList = _StockIndexForcastList.Where(x => Convert.ToInt32(x.Date) >= Convert.ToInt32(this.dtpReportStart.Value.ToString("yyyyMMdd"))).ToList();
				List<StockRobinForcast> _StockRobinForcastList = new List<StockRobinForcast>();
				foreach (StockIndexForcast s in _StockIndexForcastList) {
					if (_StockIndexList.Where(x => x.Date == s.Date).Count() > 0){ 
						if (_StockIndexList.Where(x => x.Date == s.Date).First().TX != "")
						{
							bool _IsRight = false;
							decimal _Point = Convert.ToDecimal(_StockIndexList.Where(x => x.Date == s.Date).First().TX) - Convert.ToDecimal(_StockIndexList.Where(x => x.Date == s.Date).First().TX_Open);
							if (_Point > 0 && s.Result > 0) {
								_IsRight = true;
							} 
							else if (_Point < 0 && s.Result < 0)
							{
								_IsRight = true;
								_Point = _Point * -1;
							}
							else if (_Point > 0 && s.Result < 0)
							{ 
								_Point = _Point * -1;
							}
							StockRobinForcast _StockRobinForcast = new StockRobinForcast();
							_StockRobinForcast.Date = s.Date;
							_StockRobinForcast.IsRight = _IsRight;
							_StockRobinForcast.Point=_Point;
							_StockRobinForcastList.Add(_StockRobinForcast);
						}
					} 
				}
				this.dgvRoginForcast.DataSource = _StockRobinForcastList;
				this.lbForcastRight.Text = _StockRobinForcastList.Where(x => x.IsRight == true).Count().ToString();
				this.lbForcastWrong.Text = _StockRobinForcastList.Where(x => x.IsRight == false).Count().ToString();
				this.lbProfit.Text = _StockRobinForcastList.Sum(x => x.Point).ToString();
				int _Total = _StockRobinForcastList.Where(x => x.IsRight == true).Count() + _StockRobinForcastList.Where(x => x.IsRight == false).Count();
				this.lbForcastRate.Text =Convert.ToString( Math.Round(Convert.ToDouble(_StockRobinForcastList.Where(x => x.IsRight == true).Count()) / _Total,2));
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " btnGetData_Click:" + ex.Message + "\r\n";
				logger.Error(_Log);
			}
		}
	}
}
