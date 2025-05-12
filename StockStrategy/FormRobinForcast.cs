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
		/// <summary>
		/// 可以統計Robin預測模型的結果
		/// 加入停損策略來統計(需要再驗算
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnGetData_Click(object sender, EventArgs e)
		{
			string _Log = "";
			int i = 0;
			try
			{
				List<StockIndexStopLossLog> _StockIndexStopLossLog = new List<StockIndexStopLossLog>();
				if (chkStopLoss.Checked)
					_StockIndexStopLossLog = _DataAccess.getStockIndexStopLossLogList();
				SortableBindingList<StockRobinForcast> _StockRobinForcastList=processRobinForcast(_StockIndexStopLossLog, nUDPoint.Value,chkStopLoss.Checked);
				this.dgvRoginForcast.DataSource = _StockRobinForcastList;
				this.dgvRoginForcast.Refresh();
				this.lbForcastRight.Text = _StockRobinForcastList.Where(x => x.IsRight == true).Count().ToString();
				this.lbForcastWrong.Text = _StockRobinForcastList.Where(x => x.IsRight == false).Count().ToString();
				this.lbProfit.Text = _StockRobinForcastList.Sum(x => x.Point).ToString();
				int _Total = _StockRobinForcastList.Where(x => x.IsRight == true).Count() + _StockRobinForcastList.Where(x => x.IsRight == false).Count();
				this.lbForcastRate.Text = Convert.ToString(Math.Round(Convert.ToDouble(_StockRobinForcastList.Where(x => x.IsRight == true).Count()) / _Total, 2));
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " btnGetData_Click:" + i + ex.Message + "\r\n";
				logger.Error(_Log);
			}
		}

		private SortableBindingList<StockRobinForcast> processRobinForcast(List<StockIndexStopLossLog> stockIndexStopLossLog,decimal stopLossPoint,bool isStopLoss)
		{ 
			SortableBindingList<StockRobinForcast> _StockRobinForcastList = new SortableBindingList<StockRobinForcast>();
			string _Log = "";
			int i = 0;
			try
			{
				DateTime _Dt = Convert.ToDateTime(this.dTPReport.Text);
				string _DayOfWeek = _Dt.DayOfWeek.ToString();
				string _WhereDate = _Dt.ToString("yyyyMMdd");
				List<StockIndexForcast> _StockIndexForcastList = _DataAccess.getStockIndexForcastList();
				List<StockIndex> _StockIndexList = _DataAccess.getStockIndexList(); 
				_StockIndexForcastList = _StockIndexForcastList.Where(x => Convert.ToInt32(x.Date) < Convert.ToInt32(_WhereDate)).ToList();
				_StockIndexForcastList = _StockIndexForcastList.Where(x => Convert.ToInt32(x.Date) >= Convert.ToInt32(this.dtpReportStart.Value.ToString("yyyyMMdd"))).ToList();
				 
				foreach (StockIndexForcast s in _StockIndexForcastList)
				{
					if (_StockIndexList.Where(x => x.Date == s.Date).Count() > 0)
					{
						if (_StockIndexList.Where(x => x.Date == s.Date).First().TX != "")
						{
							bool _IsRight = false;
							if (_StockIndexList.Where(x => x.Date == s.Date).First().TX_Open != "")
							{
								decimal _MTX_Open = Convert.ToDecimal(_StockIndexList.Where(x => x.Date == s.Date).First().TX_Open);
								decimal _Point = Convert.ToDecimal(_StockIndexList.Where(x => x.Date == s.Date).First().TX) - Convert.ToDecimal(_StockIndexList.Where(x => x.Date == s.Date).First().TX_Open);
								if (isStopLoss)
								{
									decimal _MTX_StopLoss = 0;
									if (s.Result > 0)
									{
										_MTX_StopLoss = _MTX_Open - stopLossPoint;
										if (stockIndexStopLossLog.Where(x => x.Date == s.Date).Any(y => Convert.ToDecimal(y.MTX_Index) <= _MTX_StopLoss))
										{
											_IsRight = false;
											_Point = stopLossPoint * -1;
										}
										else
										{
											if (Convert.ToDecimal(_StockIndexList.Where(x => x.Date == s.Date).First().TX_Low) <= _MTX_StopLoss)
											{
												_IsRight = false;
												_Point = stopLossPoint * -1;
											}
											else
											{
												_IsRight = true;
											}
										}
									}
									else
									{
										_MTX_StopLoss = _MTX_Open + stopLossPoint;
										if (stockIndexStopLossLog.Where(x => x.Date == s.Date).Any(y => Convert.ToDecimal(y.MTX_Index) >= _MTX_StopLoss))
										{
											_IsRight = false;
											_Point = stopLossPoint * -1;
										}
										else
										{
											if (Convert.ToDecimal(_StockIndexList.Where(x => x.Date == s.Date).First().TX_High) >= _MTX_StopLoss)
											{
												_IsRight = false;
												_Point = stopLossPoint * -1;
											}
											else
											{
												_IsRight = true;
												_Point = _Point * -1;
											}
										}
									}
								}
								else
								{
									if (_Point > 0 && s.Result > 0)
									{
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
								}
								StockRobinForcast _StockRobinForcast = new StockRobinForcast();
								_StockRobinForcast.Date = s.Date;
								_StockRobinForcast.IsRight = _IsRight;
								_StockRobinForcast.Point = _Point;
								_StockRobinForcast.IsBull = s.Result > 0 ? true : false;
								_StockRobinForcast.TX_Open = _MTX_Open.ToString();
								if (_StockIndexList.Where(x => x.Date == s.Date).First().TX_High != "")
									_StockRobinForcast.TX_High = _StockIndexList.Where(x => x.Date == s.Date).First().TX_High;
								if (_StockIndexList.Where(x => x.Date == s.Date).First().TX_Low != "")
									_StockRobinForcast.TX_Low = _StockIndexList.Where(x => x.Date == s.Date).First().TX_Low;
								if (_StockIndexList.Where(x => x.Date == s.Date).First().TX != "")
									_StockRobinForcast.TX = _StockIndexList.Where(x => x.Date == s.Date).First().TX;
								_StockRobinForcastList.Add(_StockRobinForcast);
							}
						}
					}
					i++;
				} 
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " processRobinForcast:" + i + ex.Message + "\r\n";
				logger.Error(_Log);
			}
			return _StockRobinForcastList;
		}
		/// <summary>
		/// 每10停損點統計個別準確率
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		private void btnStopLoss_Click(object sender, EventArgs e)
		{
			string _Log = ""; 
			try
			{
				List<StockIndexStopLossLog> _StockIndexStopLossLog = new List<StockIndexStopLossLog>(); 
					_StockIndexStopLossLog = _DataAccess.getStockIndexStopLossLogList();
				SortableBindingList<StockRobinForcast> _StockRobinForcastList = new SortableBindingList<StockRobinForcast>();
				SortableBindingList<StockRobinForcastStopLoss> _StockRobinForcastStopLossList = new SortableBindingList<StockRobinForcastStopLoss>();
				for (int i = 0; i <= 200; i = i + 10)
				{
					_StockRobinForcastList = processRobinForcast(_StockIndexStopLossLog,i, true);
					StockRobinForcastStopLoss stockRobinForcastStopLoss = new StockRobinForcastStopLoss();
					stockRobinForcastStopLoss.StopLossPoint = i.ToString();
					stockRobinForcastStopLoss.IsRight=_StockRobinForcastList.Where(x => x.IsRight == true).Count().ToString();
					stockRobinForcastStopLoss.IsFail = _StockRobinForcastList.Where(x => x.IsRight == false).Count().ToString();
					stockRobinForcastStopLoss.Point= _StockRobinForcastList.Sum(x => x.Point).ToString();
					int _Total = _StockRobinForcastList.Where(x => x.IsRight == true).Count() + _StockRobinForcastList.Where(x => x.IsRight == false).Count();
					stockRobinForcastStopLoss.ForcastRate= Convert.ToString(Math.Round(Convert.ToDouble(_StockRobinForcastList.Where(x => x.IsRight == true).Count()) / _Total, 2));
					_StockRobinForcastStopLossList.Add(stockRobinForcastStopLoss);
				}
				this.dgvStopLossResult.DataSource = _StockRobinForcastStopLossList;
				this.dgvStopLossResult.Refresh();  
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " btnStopLoss_Click:" + ex.Message + "\r\n";
				logger.Error(_Log);
			}
		}
	}
}
