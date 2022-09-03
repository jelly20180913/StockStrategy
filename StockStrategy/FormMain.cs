using NLog;
using StockStrategy.BBL;
using StockStrategy.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DataModel;
using Newtonsoft.Json;
using StockStrategy.Common;
namespace StockStrategy
{
	public partial class FormMain : Form
	{

		[StructLayout(LayoutKind.Sequential)]
		public struct CopyDataStruct
		{
			public int dwData;
			public int cbData;
			public string lpData;
		}
		private enum Margin
		{
			TX = 184000,
			MTX = 46000,
		}
		/*1.下單*/
		[DllImport("C:\\JihSun\\HTSAPI3\\HTSAPITradeClient.dll", EntryPoint = "HTSOrder", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern void HTSOrder([MarshalAs(UnmanagedType.LPStr)] StringBuilder APChar);
		/*2.期權_未平倉查詢*/
		[DllImport("C:\\JihSun\\HTSAPI3\\HTSAPITradeClient.dll", EntryPoint = "HTSCurContract", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern int HTSCurContract([MarshalAs(UnmanagedType.LPStr)] StringBuilder APChar);
		/*3.期權_委託成交回報回補函數(1) */
		[DllImport("C:\\JihSun\\HTSAPI3\\HTSAPITradeClient.dll", EntryPoint = "HTSReportFutOpt", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern int HTSReportFutOpt([MarshalAs(UnmanagedType.LPStr)] StringBuilder APChar, IntPtr recHandle);
		/*4.證券_委託成交回報回補函數 */
		[DllImport("C:\\JihSun\\HTSAPI3\\HTSAPITradeClient.dll", EntryPoint = "HTSReportStock", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern int HTSReportStock([MarshalAs(UnmanagedType.LPStr)] StringBuilder APChar, IntPtr recHandle);
		/*5.期權_即時回報*/
		[DllImport("C:\\JihSun\\HTSAPI3\\HTSAPITradeClient.dll", EntryPoint = "HTSReportFutOptRT", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern int HTSReportFutOptRT([MarshalAs(UnmanagedType.LPStr)] StringBuilder APChar, IntPtr recHandle);
		/*6.期權_委託成交回報回補函數(2) */
		[DllImport("C:\\JihSun\\HTSAPI3\\HTSAPITradeClient.dll", EntryPoint = "HTSReportFutOptKWRT", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern int HTSReportFutOptKWRT(IntPtr recHandle);
		/*7..證券_即時回報*/
		[DllImport("C:\\JihSun\\HTSAPI3\\HTSAPITradeClient.dll", EntryPoint = "HTSReportStockRT", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern int HTSReportStockRT([MarshalAs(UnmanagedType.LPStr)] StringBuilder APChar, IntPtr recHandle);
		/*8.減量*/
		[DllImport("C:\\JihSun\\HTSAPI3\\HTSAPITradeClient.dll", EntryPoint = "HTSOrderMinus", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern void HTSOrderMinus([MarshalAs(UnmanagedType.LPStr)] StringBuilder APChar);
		/*9.改價*/
		[DllImport("C:\\JihSun\\HTSAPI3\\HTSAPITradeClient.dll", EntryPoint = "HTSOrderChgPrice", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern void HTSOrderChgPrice([MarshalAs(UnmanagedType.LPStr)] StringBuilder APChar);
		/*10.投資現況函數*/
		[DllImport("C:\\JihSun\\HTSAPI3\\HTSAPITradeClient.dll", EntryPoint = "HTSAcntInfoFutOpt", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern int HTSAcntInfoFutOpt([MarshalAs(UnmanagedType.LPStr)] StringBuilder APChar, IntPtr recHandle);
		/*11.資券配額查詢函*/
		[DllImport("C:\\JihSun\\HTSAPI3\\HTSAPITradeClient.dll", EntryPoint = "HTSStkInfoQuery", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern int HTSStkInfoQuery([MarshalAs(UnmanagedType.LPStr)] StringBuilder APChar, IntPtr recHandle);
		[DllImport("user32.dll", EntryPoint = "PostMessage")]
		public static extern int PostMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
		private Dictionary<string, string> dicOrderType =
	new Dictionary<string, string>();
		private Dictionary<string, string> dicFutureOrderType =
   new Dictionary<string, string>();
		private Dictionary<string, string> dicOpenCloseAuto =
new Dictionary<string, string>();
		private Dictionary<string, string> dicFutures =
new Dictionary<string, string>();
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private static string ConnectionString = "", TAIEX = "";
		DataAccess _DataAccess = new DataAccess();

		bool LineNotify = false;
		List<string> _StockLackOffList = new List<string>();
		List<WebApiService.Models.StockFutureCode> _StockFutureCodeList = new List<WebApiService.Models.StockFutureCode>();
		List<WebApiService.Models.StockGroup> _StockGroupList = new List<WebApiService.Models.StockGroup>();
		public FormMain()
		{
			InitializeComponent();
		}
		private string[] Oberservers = ConfigurationManager.AppSettings["Observer"].Split(';');
		#region button method
		/// <summary>
		/// 傳送參數給日盛API
		/// </summary>
		/// <param name="sender">按鈕</param>
		/// <param name="mode">庫存/觀察</param>
		/// <param name="buySell">S/B</param>
		/// <param name="cashMarginShort">S</param>
		/// <param name="status">券賣/資買/現買/現賣/資賣/券買</param>
		/// <param name="foreColor"></param>
		private void sendParameter(object sender, bool mode, string buySell, string cashMarginShort, string status, Color foreColor)
		{
			string _BuySell = buySell;
			string _CashMarginShort = cashMarginShort;
			string _Units = "0";
			string _OverSell = status == "沖賣" ? ",OverSell=Y" : "";
			string _Price = "0";
			string _PriceParameter = "0";
			string _StockId = "";
			string _OrderType = dicOrderType[this.cbPriceType.SelectedItem.ToString()];
			string _MarketLimit = this.cbPriceType.SelectedItem.ToString() == "市價" ? ",MarketLimit=M" : "";
			string _PriceType = this.cbPriceType.SelectedItem.ToString();
			Button _Btn = sender as Button;
			string _StockStatus = "";
			double _Exposure = 0;
			string _Profit = "";
			if (Convert.ToString(_Btn.Tag) == "")
			{
				_Units = Convert.ToString(nupQty.Value);
				_Price = txtPrice.Text != "" ? txtPrice.Text : lbPrice.Text;
				btnDeal.Text = status;
				_StockId = txtStockId.Text;
			}
			else
			{
				ComboBox _CbPriceType = new ComboBox();
				NumericUpDown _NupQty = new NumericUpDown();
				Button _BtnDeal = new Button();
				TextBox _TxtPrice = new TextBox();
				Label _LbPrice = new Label();
				Label _LbProfit = new Label();
				if (mode)
				{
					_CbPriceType = (ComboBox)flowLayoutPanelStock.Controls.Find("cbPriceType" + _Btn.Tag, true)[0];
					_NupQty = (NumericUpDown)flowLayoutPanelStock.Controls.Find("nupQty" + _Btn.Tag, true)[0];
					_BtnDeal = (Button)flowLayoutPanelStock.Controls.Find("btnDeal" + _Btn.Tag, true)[0];
					_TxtPrice = (TextBox)flowLayoutPanelStock.Controls.Find("txtPrice" + _Btn.Tag, true)[0];
					_LbPrice = (Label)flowLayoutPanelStock.Controls.Find("lbPrice" + _Btn.Tag, true)[0];
					_LbProfit = (Label)flowLayoutPanelStock.Controls.Find("lbProfit" + _Btn.Tag, true)[0];
				}
				else
				{
					_CbPriceType = (ComboBox)flowLayoutPanel1.Controls.Find("cbPriceType" + _Btn.Tag, true)[0];
					_NupQty = (NumericUpDown)flowLayoutPanel1.Controls.Find("nupQty" + _Btn.Tag, true)[0];
					//_BtnDeal = (Button)flowLayoutPanel1.Controls.Find("btnDeal" + _Btn.Tag, true)[0];
					_BtnDeal.Text = _Btn.Text;
					_TxtPrice = (TextBox)flowLayoutPanel1.Controls.Find("txtPrice" + _Btn.Tag, true)[0];
					_LbPrice = (Label)flowLayoutPanel1.Controls.Find("lbPrice" + _Btn.Tag, true)[0];
				}
				_Units = Convert.ToString(_NupQty.Value);
				_Price = _LbPrice.Text;
				if (_TxtPrice.Text != "")
				{
					if (Convert.ToInt32(_TxtPrice.Text) != 0)
						_Price = _TxtPrice.Text;
				}

				_StockStatus = _BtnDeal.Text;
				_BtnDeal.Text = status;
				_OrderType = dicOrderType[_CbPriceType.SelectedItem.ToString()];
				_MarketLimit = _CbPriceType.SelectedItem.ToString() == "市價" ? ",MarketLimit=M" : "";
				_PriceType = _CbPriceType.SelectedItem.ToString();

				_StockId = _Btn.Tag.ToString();
				_Profit = _LbProfit.Text;
				if (_Profit != "")
				{
					if (Convert.ToDouble(_Profit) > 0)
					{
						_Profit = ",賺" + _Profit + "元";
					}
					else
					{
						_Profit = ",賠" + _Profit + "元";
					}
				}
			}
			_PriceParameter = _PriceType == "市價" ? "0" : _Price;
			string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _StockId + ",BuySell=" + _BuySell + ",Units=" + _Units + ",OrderType=" + _OrderType + ",Price=" + _PriceParameter + ",CashMarginShort=" + _CashMarginShort + _OverSell + _MarketLimit;
			StringBuilder sb = new StringBuilder(_Parameter);

			if (_Price != "")
			{
				_Exposure = Convert.ToDouble(_Units) * Convert.ToDouble(_Price) * 1000;
				if (_Exposure > 1000000)
				{
					MessageBox.Show("不得下單超過100萬");
					return;
				}
				DialogResult Result = MessageBox.Show("是否確定下單", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
				if (Result == DialogResult.OK)
				{
					HTSOrder(sb);
					MessageBox.Show(_Parameter);
					string _Msg = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " 你" + status + _StockId + "共" + _Units + "張," + _PriceType + _Price + "元" + _Profit;
					richTxtInfo.SelectionColor = foreColor;
					richTxtInfo.AppendText(_Msg + Environment.NewLine);
					richTxtInfo.SelectionStart = richTxtInfo.TextLength;
					richTxtInfo.ScrollToCaret();
					insertUpdateStockInventory(_StockId, "", status, _Units, _Price, false, true);

					btnReflash.PerformClick();
				}
			}
			else
			{
				MessageBox.Show("請輸入價格");
			}
		}
		/// <summary>
		/// 庫存
		/// </summary>
		/// <param name="stockId">代碼</param>
		/// <param name="name">名稱</param>
		/// <param name="status">狀態</param>
		/// <param name="units">口數</param>
		/// <param name="price">價格</param>
		/// <param name="type">1:期貨 0:股票</param>
		/// <param name="chkPt">1:關閉 0:打開</param>
		private void insertUpdateStockInventory(string stockId, string name, string status, string units, string price, bool type, bool chkPt)
		{
			WebApiService.Models.StockInventory s = new WebApiService.Models.StockInventory();
			s = getStockInventory(stockId);
			s.UpdateTime = DateTime.Now.ToString("yyyyMMdd hh:mm:ss");
			if (s.Code != null)
			{
				if (IsCover(s.Status, status))
				{
					s.Qty -= Convert.ToInt32(units);
					if (s.Qty <= 0)
					{
						s.Qty = Math.Abs(Convert.ToInt32(s.Qty));
						s.Status = status;
						s.Cost = price;
					}
					updateStockInventory(s);

				}
				else
				{
					s.Cost = Convert.ToString(((s.Qty * Convert.ToDouble(s.Cost)) + Convert.ToInt32(units) * Convert.ToDouble(price)) / (s.Qty + Convert.ToInt32(units)));
					s.Cost = Convert.ToString(Math.Round(Convert.ToDouble(s.Cost), 2));
					s.Qty += Convert.ToInt32(units);
					updateStockInventory(s);

				}
			}
			else
			{
				s.Name = name;
				s.Qty = Convert.ToInt32(units);
				s.Status = status;
				s.Target = "";
				s.Defence = "";
				s.CloseABSStopLost = false;
				s.CloseHighStopLost = false;
				s.ClosePtLost = chkPt;
				s.CloseStopLost = true;
				s.Code = stockId;
				s.Cost = price;
				s.Type = type;
				insertStockInventory(s);
			}

		}
		/// <summary>
		/// 期貨參數
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="mode"></param>
		/// <param name="buySell"></param>
		/// <param name="status"></param>
		/// <param name="foreColor"></param>
		private void sendFuturesParameter(object sender, bool mode, string buySell, string status, Color foreColor)
		{
			int n = 0;
			bool IsNum = Int32.TryParse(this.txtFutureName.Text, out n);
			string _BuySell = buySell;
			string _Units = "0";
			string _Price = "0";
			string _PriceParameter = "0";
			string _FutureOrderType = dicFutureOrderType[this.cbOrderType.SelectedItem.ToString()];
			string _FokIocRod = _FutureOrderType == "M" ? "F" : "R";
			string _PriceType = this.cbOrderType.SelectedItem.ToString();
			string _OpenCloseAuto = dicOpenCloseAuto[this.cbOpenCloseAuto.SelectedItem.ToString()];
			Button _Btn = sender as Button;
			string _Profit = "";
			string _StockStatus = "";
			string _FutureName = this.txtFutureName.Text != "" ? this.txtFutureName.Text : dicFutures[this.cbFutureName.SelectedItem.ToString()];
			string _Name = this.txtFutureName.Text != "" ? this.txtFutureName.Text : this.cbFutureName.SelectedItem.ToString() + this.cbDateMM.SelectedItem;
			if (IsNum)
			{
				if (this._StockFutureCodeList.Where(x => x.Code == this.txtFutureName.Text).ToList().Count > 0)
				{
					_FutureName = this._StockFutureCodeList.Where(x => x.Code == this.txtFutureName.Text).First().EnCode;
					_Name = this._StockFutureCodeList.Where(x => x.Code == this.txtFutureName.Text).First().Name + "期" + this.cbDateMM.SelectedItem;
				}
			}
			else
			{
				if (this._StockFutureCodeList.Where(x => x.Name == this.txtFutureName.Text).ToList().Count > 0)
				{
					_FutureName = this._StockFutureCodeList.Where(x => x.Name == this.txtFutureName.Text).First().EnCode;
					_Name = this._StockFutureCodeList.Where(x => x.Name == this.txtFutureName.Text).First().Name + "期" + this.cbDateMM.SelectedItem;
				}
			}
			string _StockId = _FutureName;


			//int _Day = DateTime.Now.Day;
			DateTime _DtContractDate = DateTime.Now;
			//if (_Day > 10)
			//    _DtContractDate = _DtContractDate.AddMonths(1);
			string _ContractYear = _DtContractDate.ToString("yyyy");
			string _ContractDate = _ContractYear + this.cbDateMM.SelectedItem;
			if (Convert.ToString(_Btn.Tag) == "")
			{
				_Units = Convert.ToString(nupLots.Value);
				_Price = this.txtFuturePrice.Text != "" ? this.txtFuturePrice.Text : this.lbFutureStock.Text;
				btnDeal.Text = status;
			}
			else
			{
				ComboBox _CbOrderType = new ComboBox();
				NumericUpDown _NupLots = new NumericUpDown();
				Button _BtnFutureDeal = new Button();
				TextBox _TxtFuturePrice = new TextBox();
				Label _LbPrice = new Label();
				Label _LbProfit = new Label();
				if (mode)
				{
					_CbOrderType = (ComboBox)flowLayoutFutures.Controls.Find("cbOrderType" + _Btn.Tag, true)[0];
					_NupLots = (NumericUpDown)flowLayoutFutures.Controls.Find("nupLots" + _Btn.Tag, true)[0];
					_BtnFutureDeal = (Button)flowLayoutFutures.Controls.Find("btnDeal" + _Btn.Tag, true)[0];
					_TxtFuturePrice = (TextBox)flowLayoutFutures.Controls.Find("txtPrice" + _Btn.Tag, true)[0];
					_LbPrice = (Label)flowLayoutFutures.Controls.Find("lbPrice" + _Btn.Tag, true)[0];
					_LbProfit = (Label)flowLayoutFutures.Controls.Find("lbProfit" + _Btn.Tag, true)[0];
				}
				_Units = Convert.ToString(_NupLots.Value);
				_Price = _TxtFuturePrice.Text != "" ? _TxtFuturePrice.Text : _LbPrice.Text;

				_FutureOrderType = dicFutureOrderType[_CbOrderType.SelectedItem.ToString()];
				_StockStatus = _BtnFutureDeal.Text;
				_BtnFutureDeal.Text = status;
				_StockId = _Btn.Tag.ToString();
				_Profit = _LbProfit.Text;
				if (_Profit != "")
				{
					if (Convert.ToDouble(_Profit) > 0)
					{
						_Profit = ",賺" + _Profit + "元";
					}
					else
					{
						_Profit = ",賠" + _Profit + "元";
					}
				}
			}

			_PriceParameter = _FutureOrderType == "M" ? "" : _Price;
			string _Parameter = "Market=F,Account=" + txtFutureAccount.Text + ",ContractName=" + _FutureName + ",ContractDate=" + _ContractDate + ",OpenCloseAuto=" + _OpenCloseAuto + ",BuySell=" + _BuySell + ",Lots=" + _Units + ",OrderType=" + _FutureOrderType + ",Price=" + _PriceParameter + ",FokIocRod=" + _FokIocRod + ",DayTrade=N";
			StringBuilder sb = new StringBuilder(_Parameter);

			if ((_Price != "" && _FutureOrderType == "L") || _FutureOrderType == "M")
			{
				if (Convert.ToInt32(_Units) > 4)
				{
					MessageBox.Show("不得下單超過4口");
					return;
				}
				DialogResult Result = MessageBox.Show("是否確定下單", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
				if (Result == DialogResult.OK)
				{
					HTSOrder(sb);
					// MessageBox.Show(_Parameter);
					string _Msg = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " 你" + status + _Name + "共" + _Units + "口," + _PriceType + _Price + "元" + _Profit;
					richTxtInfo.SelectionColor = foreColor;
					richTxtInfo.AppendText(_Msg + Environment.NewLine);
					richTxtInfo.SelectionStart = richTxtInfo.TextLength;
					richTxtInfo.ScrollToCaret();
					string _Code = _FutureName + _ContractDate;
					bool _ChkPt = _FutureName == "MXF" ? false : true;
					insertUpdateStockInventory(_Code, _Name, status, _Units, _Price, true, _ChkPt);
					this.lbTimes.Text = Convert.ToString(Convert.ToInt32(this.lbTimes.Text) + 1);

					this.btnFutureRefrash.PerformClick();
				}
			}
			else
			{
				MessageBox.Show("請輸入價格");
			}
		}
		/// <summary>
		/// 新增股票庫存
		/// </summary>
		/// <param name="stockInventory"></param>
		private void insertStockInventory(WebApiService.Models.StockInventory stockInventory)
		{
			string _Msg = "";
			try
			{
				string json = JsonConvert.SerializeObject(stockInventory);
				string _Action = "StockInventory";
				string _Uri = ConnectionString + _Action;
				ApiResultEntity _ApiResult = CallWebApi.Post(json, _Uri);
				string _Return = _ApiResult.Data.ToString();
			}
			catch (Exception ex)
			{
				_Msg = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " InsertStockInventory:" + ex.Message;
				logger.Error(_Msg);
				richTxtInfo.AppendText(_Msg + Environment.NewLine);
			}
		}
		/// <summary>
		/// 更新股票庫存
		/// </summary>
		/// <param name="stockInventory"></param>
		private void updateStockInventory(WebApiService.Models.StockInventory stockInventory)
		{
			string _Msg = "";
			try
			{
				string json = JsonConvert.SerializeObject(stockInventory);
				string _Action = "StockInventory";
				string _Uri = ConnectionString + _Action;
				ApiResultEntity _ApiResult = CallWebApi.Put(json, _Uri);
				string _Return = _ApiResult.Data.ToString();
			}
			catch (Exception ex)
			{
				_Msg = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " InsertStockInventory:" + ex.Message;
				logger.Error(_Msg);
				richTxtInfo.AppendText(_Msg + Environment.NewLine);
			}
		}
		private List<WebApiService.Models.StockInventory> getStockInventoryList()
		{
			List<WebApiService.Models.StockInventory> _StockInventoryList = new List<WebApiService.Models.StockInventory>();
			string _Msg = "";
			try
			{
				_StockInventoryList = _DataAccess.getStockInventoryList();
			}
			catch (Exception ex)
			{
				_Msg = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " getStockInventoryList:" + ex.Message;
				logger.Error(_Msg);
				richTxtInfo.AppendText(_Msg + Environment.NewLine);
			}
			return _StockInventoryList;
		}
		private WebApiService.Models.StockInventory getStockInventory(string code)
		{
			string _Msg = "";
			WebApiService.Models.StockInventory _StockInventory = new WebApiService.Models.StockInventory();
			try
			{
				List<WebApiService.Models.StockInventory> _StockInventoryList = getStockInventoryList();
				if (_StockInventoryList.Where(x => x.Code == code).Count() > 0)
					_StockInventory = _StockInventoryList.Where(x => x.Code == code).First();
			}
			catch (Exception ex)
			{
				_Msg = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " getStockInventory:" + ex.Message;
				logger.Error(_Msg);
				richTxtInfo.AppendText(_Msg + Environment.NewLine);
			}
			return _StockInventory;
		}
		/// <summary>
		/// 合併庫存功能
		/// 1. 若平倉
		/// 1.1. 需將原庫存張數扣除
		/// 1.2. 庫存狀態回原始庫存狀態
		/// 2. 若非平倉則為加碼
		/// 2.1. 需將原庫存張數增加
		/// 2.3. 計算平均成本
		/// </summary>
		/// <param name="stockId">股票代號</param>
		/// <param name="peice">張數</param>
		/// <param name="price">價格</param>
		/// <param name="status">狀態</param>
		/// <param name="originalStatus">庫存狀態</param>
		/// <returns></returns>
		private bool combindStock(string stockId, string peice, string price, string status, string originalStatus)
		{
			bool _IsExist = false;
			Label _LbCost = new Label();
			NumericUpDown _NupQty = new NumericUpDown();
			Label _LbStock = new Label();
			Button _BtnDeal = new Button();
			double _Cost = 0.0;
			foreach (Panel p in flowLayoutPanelStock.Controls.Cast<Panel>())
			{
				foreach (Control contr in p.Controls.Cast<Control>())
				{
					if (contr is TextBox)
					{
						if (contr.Tag != null && contr.Tag.ToString() == stockId)
						{
							_IsExist = true;
							_BtnDeal = (Button)flowLayoutPanelStock.Controls.Find("btnDeal" + stockId, true)[0];
							_LbCost = (Label)flowLayoutPanelStock.Controls.Find("lbCost" + stockId, true)[0];
							_NupQty = (NumericUpDown)flowLayoutPanelStock.Controls.Find("nupQty" + stockId, true)[0];
							_LbStock = (Label)flowLayoutPanelStock.Controls.Find("lbStock" + stockId, true)[0];
							if (IsCover(originalStatus, status))
							{
								_NupQty.Value = Convert.ToDecimal(_LbStock.Text) - Convert.ToDecimal(peice);
								_LbStock.Text = Convert.ToString(Convert.ToDecimal(_LbStock.Text) - Convert.ToDecimal(peice));
								_BtnDeal.Text = originalStatus;
							}
							else if (originalStatus == status)
							{
								_Cost = (Convert.ToDouble(_LbCost.Text) * Convert.ToDouble(_LbStock.Text) + Convert.ToDouble(peice) * Convert.ToDouble(price)) / (Convert.ToDouble(_LbStock.Text) + Convert.ToDouble(peice));
								_LbCost.Text = Convert.ToString(Math.Round(_Cost, 2));
								_NupQty.Value = Convert.ToDecimal(_LbStock.Text) + Convert.ToDecimal(peice);
								_LbStock.Text = Convert.ToString(Convert.ToDecimal(_LbStock.Text) + Convert.ToDecimal(peice));

							}
							if (_LbStock.Text == "" || Convert.ToInt32(_LbStock.Text) == 0) p.Controls.Clear();
						}
					}
				}
			}
			foreach (Panel f in flowLayoutFutures.Controls.Cast<Panel>())
			{
				foreach (Control contr in f.Controls.Cast<Control>())
				{
					if (contr is TextBox)
					{
						if (contr.Tag != null && contr.Tag.ToString() == stockId)
						{
							_IsExist = true;
							_BtnDeal = (Button)flowLayoutFutures.Controls.Find("btnDeal" + stockId, true)[0];
							_LbCost = (Label)flowLayoutFutures.Controls.Find("lbCost" + stockId, true)[0];
							_NupQty = (NumericUpDown)flowLayoutFutures.Controls.Find("nupLots" + stockId, true)[0];
							_LbStock = (Label)flowLayoutFutures.Controls.Find("lbStock" + stockId, true)[0];
							if (IsCover(originalStatus, status))
							{
								_NupQty.Value = Convert.ToDecimal(_LbStock.Text) - Convert.ToDecimal(peice);
								_LbStock.Text = Convert.ToString(Convert.ToDecimal(_LbStock.Text) - Convert.ToDecimal(peice));
								_BtnDeal.Text = originalStatus;

							}
							else if (originalStatus == status)
							{
								_Cost = (Convert.ToDouble(_LbCost.Text) * Convert.ToDouble(_LbStock.Text) + Convert.ToDouble(peice) * Convert.ToDouble(price)) / (Convert.ToDouble(_LbStock.Text) + Convert.ToDouble(peice));
								_LbCost.Text = Convert.ToString(Math.Round(_Cost, 2));
								_NupQty.Value = Convert.ToDecimal(_LbStock.Text) + Convert.ToDecimal(peice);
								_LbStock.Text = Convert.ToString(Convert.ToDecimal(_LbStock.Text) + Convert.ToDecimal(peice));

							}
							if (_LbStock.Text == "" || Convert.ToInt32(_LbStock.Text) == 0) f.Controls.Clear();
						}
					}

				}
			}
			return _IsExist;
		}
		/// <summary>
		/// 判斷是否平倉
		/// </summary>
		/// <param name="stockStatus">庫存狀態</param>
		/// <param name="status">沖銷狀態</param>
		/// <returns></returns>
		private bool IsCover(string stockStatus, string status)
		{
			bool _IsCover = false;
			if (stockStatus == "現買" && status == "現賣") _IsCover = true;
			else if (stockStatus == "資買" && status == "資賣") _IsCover = true;
			else if (stockStatus == "沖賣" && status == "現買") _IsCover = true;
			else if (stockStatus == "現賣" && status == "現買") _IsCover = true;
			else if (stockStatus == "券賣" && status == "資買" || stockStatus == "券賣" && status == "券買") _IsCover = true;
			return _IsCover;
		}
		//private string getPrice(string priceType,string price)
		//{
		//    double _Price = 0.0;
		//    double _InputPrice = Convert.ToDouble(price);
		//    switch (priceType) 
		//    {
		//        case "跌停":
		//            _Price = _InputPrice * 0.9;
		//            break;
		//        case "漲停":
		//            _Price = _InputPrice * 1.1;
		//            break;
		//        case "市價":
		//            _Price = _InputPrice ;
		//            break; 
		//    }
		//    return Convert.ToString(_Price);
		//}
		#endregion
		/// <summary>
		/// 動態券賣按鈕 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSellingShort_Click(object sender, EventArgs e)
		{
			sendParameter(sender, false, "S", "S", "券賣", Color.DarkGreen);
		}
		/// <summary>
		/// 動態券賣按鈕-庫存容器
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSellingShortStock_Click(object sender, EventArgs e)
		{
			sendParameter(sender, true, "S", "S", "券賣", Color.DarkGreen);
		}
		/// <summary>
		/// 庫存減號按鈕
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnMinus_Click(object sender, EventArgs e)
		{
			addMinus(sender, false, false);
		}
		/// <summary>
		/// 動態減號按鈕-庫存容器
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnMinusStock_Click(object sender, EventArgs e)
		{
			addMinus(sender, true, false);
		}
		/// <summary>
		/// 數量加減
		/// </summary>
		/// <param name="sender">按鈕</param>
		/// <param name="mode">庫存/觀察</param>
		/// <param name="status">1:add/0:minus</param>
		private void addMinus(object sender, bool mode, bool status)
		{
			double _Price = 0.0;
			double _Value = 0.0;
			Button _Btn = sender as Button;
			if (_Btn.Tag.ToString() == "")
			{

				_Price = this.txtPrice.Text != "" && this.txtPrice.Text != "0" ? Convert.ToDouble(this.txtPrice.Text) : Convert.ToDouble(this.lbPrice.Text);
				_Value = tick(_Price);
				if (_Price != 0)
				{
					if (status) _Price += _Value;
					else _Price -= _Value;
				}

				this.txtPrice.Text = _Price.ToString();
			}
			else
			{
				TextBox _TxtPrice = new TextBox();
				Label _LbPrice = new Label();
				if (mode)
				{
					_TxtPrice = (TextBox)flowLayoutPanelStock.Controls.Find("txtPrice" + _Btn.Tag, true)[0];
					_LbPrice = (Label)flowLayoutPanelStock.Controls.Find("lbPrice" + _Btn.Tag, true)[0];
				}
				else
				{
					_TxtPrice = (TextBox)flowLayoutPanel1.Controls.Find("txtPrice" + _Btn.Tag, true)[0];
					_LbPrice = (Label)flowLayoutPanel1.Controls.Find("lbPrice" + _Btn.Tag, true)[0];
				}
				if (_LbPrice.Text != "" || _TxtPrice.Text != "")
				{
					_Price = _TxtPrice.Text != "" && _TxtPrice.Text != "0" ? Convert.ToDouble(_TxtPrice.Text) : Convert.ToDouble(_LbPrice.Text);
					_Value = tick(_Price);
					if (_Price != 0)
					{
						if (status) _Price += _Value;
						else _Price -= _Value;
					}
					_TxtPrice.Text = _Price.ToString();
				}
				else
				{
					MessageBox.Show("請填入價格");
				}
			}
		}
		/// <summary>
		/// 期貨+價格
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="mode"></param>
		/// <param name="status"></param>
		private void addMinusFuture(object sender, bool mode, bool status)
		{
			double _Price = 0.0;
			double _Value = 0.0;
			Button _Btn = sender as Button;
			if (_Btn.Tag.ToString() == "")
			{

				_Price = this.txtFuturePrice.Text != "" && this.txtFuturePrice.Text != "0" ? Convert.ToDouble(this.txtFuturePrice.Text) : Convert.ToDouble(this.lbFuturePrice.Text);
				_Value = tick(_Price);
				if (_Price != 0)
				{
					if (status) _Price += _Value;
					else _Price -= _Value;
				}

				this.txtFuturePrice.Text = _Price.ToString();
			}
			else
			{
				TextBox _TxtPrice = new TextBox();
				Label _LbPrice = new Label();
				if (mode)
				{
					_TxtPrice = (TextBox)flowLayoutFutures.Controls.Find("txtPrice" + _Btn.Tag, true)[0];
					_LbPrice = (Label)flowLayoutFutures.Controls.Find("lbPrice" + _Btn.Tag, true)[0];
				}

				if (_LbPrice.Text != "" || _TxtPrice.Text != "")
				{
					_Price = _TxtPrice.Text != "" && _TxtPrice.Text != "0" ? Convert.ToDouble(_TxtPrice.Text) : Convert.ToDouble(_LbPrice.Text);
					_Value = tick(_Price);
					if (_Price != 0)
					{
						if (status) _Price += _Value;
						else _Price -= _Value;
					}
					_TxtPrice.Text = _Price.ToString();
				}
				else
				{
					MessageBox.Show("請填入價格");
				}
			}
		}
		/// <summary>
		/// 動態加號按鈕
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAdd_Click(object sender, EventArgs e)
		{
			addMinus(sender, false, true);
		}
		/// <summary>
		/// 動態加號按鈕-庫存容器
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAddStock_Click(object sender, EventArgs e)
		{
			addMinus(sender, true, true);
		}

		/// <summary>
		/// 資買
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnMarginTrading_Click(object sender, EventArgs e)
		{
			sendParameter(sender, false, "B", "M", "資買", Color.DarkRed);
		}
		private void btnMarginTradingStock_Click(object sender, EventArgs e)
		{
			sendParameter(sender, true, "B", "M", "資買", Color.DarkRed);
		}
		/// <summary>
		/// 沖賣
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnOverSell_Click(object sender, EventArgs e)
		{
			sendParameter(sender, false, "S", "C", "沖賣", Color.Salmon);
		}
		private void btnOverSellStock_Click(object sender, EventArgs e)
		{
			sendParameter(sender, true, "S", "C", "沖賣", Color.Salmon);
		}
		/// <summary>
		/// 現買
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnBuy_Click(object sender, EventArgs e)
		{
			if (prohibitOrder())
			{
				DialogResult Result = MessageBox.Show("請注意該股票是否已超過3%漲幅,請勿追高!", "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
				if (Result == DialogResult.OK)
				{
					sendParameter(sender, false, "B", "C", "現買", Color.Red);
				}
			}
			else
			{
				MessageBox.Show("禁止在九點二十以前下單");
			}

		}
		private void btnBuyStock_Click(object sender, EventArgs e)
		{
			sendParameter(sender, true, "B", "C", "現買", Color.Red);
		}
		/// <summary>
		/// 現賣
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSell_Click(object sender, EventArgs e)
		{
			sendParameter(sender, false, "S", "C", "現賣", Color.Green);

		}
		private void btnSellStock_Click(object sender, EventArgs e)
		{
			sendParameter(sender, true, "S", "C", "現賣", Color.Green);
		}
		private void FormMain_Load(object sender, EventArgs e)
		{
			DataAccess _DataAccess = new DataAccess();
			getAVG();
			_StockFutureCodeList = _DataAccess.getStockFutureCodeList();
			_StockGroupList = _DataAccess.getStockGroupList();
			this.txtOberserver.Text = ConfigurationManager.AppSettings["Observer"];
			ConnectionString = ConfigurationManager.AppSettings["ApiServer"];
			this.Text = "股票策略下單機：V" + FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion.ToString();
			this.cbPriceType.SelectedIndex = 0;

			dicOrderType.Add("限價", "L");
			dicOrderType.Add("市價", "");
			dicOrderType.Add("漲停", "C");
			dicOrderType.Add("跌停", "F");
			dicFutureOrderType.Add("限價", "L");
			dicFutureOrderType.Add("市價", "M");
			dicFutureOrderType.Add("範圍市價", "R");

			dicOpenCloseAuto.Add("新倉", "O");
			dicOpenCloseAuto.Add("平倉", "C");
			dicOpenCloseAuto.Add("自動", "A");

			dicFutures.Add("台股期", "TXF");
			dicFutures.Add("小台期", "MXF");
			dicFutures.Add("道瓊期", "UDF");
			dicFutures.Add("NAS期", "UNF");
			dicFutures.Add("金融期", "FXF");
			this.WindowState = FormWindowState.Maximized;
			this.btnClick.PerformClick();
			this.btnGetPrice.Focus();
			initialRadioButton();
			this.btnReflash.PerformClick();
			//this.txtStockAccount.Text = ConfigurationManager.AppSettings["StockAccount"];
			this.txtStockAccount.Text = Tool.Decrypt(ConfigurationManager.AppSettings["StockAccount"], "20220801", "B1050520");
			this.txtFutureAccount.Text = Tool.Decrypt(ConfigurationManager.AppSettings["FutureAccount"], "20220801", "B1050520");

			this.cbOpenCloseAuto.SelectedIndex = 0;
			this.cbOrderType.SelectedIndex = 1;
			this.cbFutureName.SelectedIndex = 0;

			this.cbDateMM.Items.Add(DateTime.Now.ToString("MM"));
			this.cbDateMM.Items.Add(DateTime.Now.AddMonths(1).ToString("MM"));
			this.cbDateMM.Items.Add(DateTime.Now.AddMonths(2).ToString("MM"));
			int _Day = DateTime.Now.Day;
			if (_Day > 10) this.cbDateMM.SelectedIndex = 1;
			else this.cbDateMM.SelectedIndex = 0;
			//this.btnLogin.PerformClick();
			this.btnFutureRefrash.PerformClick();
			// _DataAccess.loginWebApi();

		}
		#region method
		/// <summary>
		/// 股價>100,一檔0.5
		/// 其餘為0.1
		/// </summary>
		/// <param name="price"></param>
		/// <returns></returns>
		private double tick(double price)
		{
			double _Value = 0.0;
			if (price > 100 && price < 999) { _Value = 0.5; }
			else if (price > 10000) { _Value = 1; }
			else { _Value = 0.1; }
			return _Value;
		}

		/// <summary>
		/// 取得一季的價格清單-呼叫太快被證交所鎖ip
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		private Stock.GetMonthPriceOut getQuaterMonthPriceOutList(string s)
		{
			Stock.GetMonthPriceIn _GetMonthPriceIn = new Stock.GetMonthPriceIn();
			_GetMonthPriceIn.Sample3_Date = DateTime.Now.ToString("yyyyMMdd");
			_GetMonthPriceIn.Sample3_Symbol = s;
			Stock.GetMonthPriceOut _GetMonthPriceOutList = StockPrice.GetMonthPrice(_GetMonthPriceIn);
			_GetMonthPriceIn.Sample3_Date = DateTime.Now.AddMonths(-1).ToString("yyyyMMdd");
			Thread.Sleep(5000);
			_GetMonthPriceOutList.gridList.AddRange(StockPrice.GetMonthPrice(_GetMonthPriceIn).gridList);
			Thread.Sleep(5000);
			//_GetMonthPriceIn.Sample3_Date = DateTime.Now.AddMonths(-2).ToString("yyyyMMdd");
			//Thread.Sleep(5000);
			//_GetMonthPriceOutList.gridList.AddRange(StockPrice.GetMonthPrice(_GetMonthPriceIn).gridList);
			//_GetMonthPriceIn.Sample3_Date = DateTime.Now.AddMonths(-3).ToString("yyyyMMdd");
			//Thread.Sleep(5000);
			//_GetMonthPriceOutList.gridList.AddRange(StockPrice.GetMonthPrice(_GetMonthPriceIn).gridList);
			return _GetMonthPriceOutList;
		}
		/// <summary>
		/// 動態產生觀察名單
		/// 庫存容器:
		/// 1. 計算出MA5/MA10/MA20/MA60均價
		/// 2. 背景色:粉紅色
		/// 3. 顯示股票成本
		/// 4. 新建倉預設關閉均線停損策略
		/// 目前取得證交所的均線方法會吃效能,待改取用資料庫
		/// </summary>
		/// <param name="s"></param>
		/// <param name="piece">張數</param>
		/// <param name="value">股價</param> 
		/// <param name="mode">0:觀察名單,1:庫存</param> 
		/// <param name="status">狀態</param> 
		/// <param name="IsNew">新建倉</param> 
		// private Panel dynamicOberserver(string s, decimal piece, string value, bool mode, string status, bool IsNew, string closeStopLost, string closeABSStopLost, string closeHighStopLost)
		private Panel dynamicOberserver(WebApiService.Models.StockInventory stockInventory, bool mode, bool IsNew)
		{
			double _Ma5 = 0, _Ma10 = 0, _Ma20 = 0, _Ma60 = 0;
			//if (mode)
			//{
			DataAccess _DataAccess = new DataAccess();
			//Stock.GetMonthPriceOut _GetMonthPriceOutList = getQuaterMonthPriceOutList(stockInventory.Code);
			//_Ma5 = Math.Round(_GetMonthPriceOutList.gridList.OrderByDescending(x => x.date).ToList().Take(5).Sum(x => Convert.ToDouble(x.close)) / 5, 2);
			//_Ma10 = Math.Round(_GetMonthPriceOutList.gridList.OrderByDescending(x => x.date).ToList().Take(10).Sum(x => Convert.ToDouble(x.close)) / 10, 2);
			//_Ma20 = Math.Round(_GetMonthPriceOutList.gridList.OrderByDescending(x => x.date).ToList().Take(20).Sum(x => Convert.ToDouble(x.close)) / 20, 2);
			//  List<WebApiService.Models.Stock> _StockList = _DataAccess.getStockByCodeList(stockInventory.Code);
			List<DataModel.Stock.Stock> _StockList = _DataAccess.getStockBySqlList(stockInventory.Code, "Code");
			if (_StockList.Count > 5) _Ma5 = Math.Round(_StockList.OrderByDescending(x => x.Date).ToList().Take(5).Sum(x => Convert.ToDouble(x.ClosingPrice)) / 5, 2);
			if (_StockList.Count > 10) _Ma10 = Math.Round(_StockList.OrderByDescending(x => x.Date).ToList().Take(10).Sum(x => Convert.ToDouble(x.ClosingPrice)) / 10, 2);
			if (_StockList.Count > 20) _Ma20 = Math.Round(_StockList.OrderByDescending(x => x.Date).ToList().Take(20).Sum(x => Convert.ToDouble(x.ClosingPrice)) / 20, 2);
			if (_StockList.Count > 60) _Ma60 = Math.Round(_StockList.OrderByDescending(x => x.Date).ToList().Take(60).Sum(x => Convert.ToDouble(x.ClosingPrice)) / 60, 2);
			//}
			//
			// 
			// btnSell
			// 
			Button btnSell = new Button();
			btnSell.BackColor = System.Drawing.Color.LightGreen;
			btnSell.Location = new System.Drawing.Point(1062, 8);
			btnSell.Margin = new System.Windows.Forms.Padding(4);
			btnSell.Name = "btnSell" + stockInventory.Code;
			btnSell.Size = new System.Drawing.Size(52, 29);
			btnSell.TabIndex = 6;
			btnSell.Tag = stockInventory.Code;
			btnSell.Text = "現賣";
			btnSell.UseVisualStyleBackColor = false;
			btnSell.Enabled = false;

			// 
			// btnBuy
			// 
			Button btnBuy = new Button();
			btnBuy.BackColor = System.Drawing.Color.Red;
			btnBuy.Location = new System.Drawing.Point(1009, 8);
			btnBuy.Margin = new System.Windows.Forms.Padding(4);
			btnBuy.Name = "btnBuy" + stockInventory.Code;
			btnBuy.Size = new System.Drawing.Size(52, 29);
			btnBuy.TabIndex = 5;
			btnBuy.Tag = stockInventory.Code;
			btnBuy.Text = "現買";
			btnBuy.UseVisualStyleBackColor = false;

			// 
			// nupQty
			// 
			NumericUpDown nupQty = new NumericUpDown();
			nupQty.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			nupQty.Location = new System.Drawing.Point(120, 8);
			nupQty.Margin = new System.Windows.Forms.Padding(4);
			nupQty.Name = "nupQty" + stockInventory.Code;
			nupQty.Size = new System.Drawing.Size(35, 25);
			nupQty.TabIndex = 8;
			nupQty.Value = Convert.ToDecimal(stockInventory.Qty);
			nupQty.Maximum = 200;
			//nupQty.Value = new decimal(new int[] {
			//1,
			//0,
			//0,
			//0});
			// 
			// btnOverSell
			// 
			Button btnOverSell = new Button();
			btnOverSell.BackColor = System.Drawing.Color.PeachPuff;
			btnOverSell.Location = new System.Drawing.Point(956, 8);
			btnOverSell.Margin = new System.Windows.Forms.Padding(4);
			btnOverSell.Name = "btnOverSell" + stockInventory.Code;
			btnOverSell.Size = new System.Drawing.Size(52, 29);
			btnOverSell.TabIndex = 4;
			btnOverSell.Tag = stockInventory.Code;
			btnOverSell.Text = "沖賣";
			btnOverSell.UseVisualStyleBackColor = false;

			// 
			// lbPrice
			// 
			Label lbPrice = new Label();
			lbPrice.AutoSize = true;
			lbPrice.Location = new System.Drawing.Point(196, 13);
			lbPrice.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lbPrice.Name = "lbPrice" + stockInventory.Code;
			lbPrice.Size = new System.Drawing.Size(52, 15);
			lbPrice.TabIndex = 62;
			// 
			// txtPrice
			// 
			TextBox txtPrice = new TextBox();
			txtPrice.Location = new System.Drawing.Point(312, 8);
			txtPrice.Margin = new System.Windows.Forms.Padding(4);
			txtPrice.Name = "txtPrice" + stockInventory.Code;
			txtPrice.Size = new System.Drawing.Size(29, 29);
			txtPrice.TabIndex = 9;
			// 
			// btnMarginTrading
			// 
			Button btnMarginTrading = new Button();
			btnMarginTrading.BackColor = System.Drawing.Color.IndianRed;
			btnMarginTrading.Location = new System.Drawing.Point(848, 8);
			btnMarginTrading.Margin = new System.Windows.Forms.Padding(4);
			btnMarginTrading.Name = "btnMarginTrading" + stockInventory.Code;
			btnMarginTrading.Size = new System.Drawing.Size(52, 29);
			btnMarginTrading.TabIndex = 3;
			btnMarginTrading.Tag = stockInventory.Code;
			btnMarginTrading.Text = "資買";
			btnMarginTrading.UseVisualStyleBackColor = false;

			// 
			// btnMinus
			// 
			Button btnMinus = new Button();
			btnMinus.Font = new System.Drawing.Font("新細明體", 12F);
			btnMinus.Location = new System.Drawing.Point(345, 6);
			btnMinus.Margin = new System.Windows.Forms.Padding(4);
			btnMinus.Name = "btnMinus" + stockInventory.Code;
			btnMinus.Size = new System.Drawing.Size(25, 25);
			btnMinus.TabIndex = 10;
			btnMinus.Tag = stockInventory.Code;
			btnMinus.Text = "-";
			btnMinus.UseVisualStyleBackColor = true;

			// 
			// btnSellingShort
			// 
			Button btnSellingShort = new Button();
			btnSellingShort.BackColor = System.Drawing.Color.DarkGreen;
			btnSellingShort.Location = new System.Drawing.Point(793, 8);
			btnSellingShort.Margin = new System.Windows.Forms.Padding(4);
			btnSellingShort.Name = "btnSellingShort" + stockInventory.Code;
			btnSellingShort.Size = new System.Drawing.Size(52, 29);
			btnSellingShort.TabIndex = 2;
			btnSellingShort.Tag = stockInventory.Code;
			btnSellingShort.Text = "券賣";
			btnSellingShort.UseVisualStyleBackColor = false;

			// 
			// btnAdd
			// 
			Button btnAdd = new Button();
			btnAdd.Font = new System.Drawing.Font("新細明體", 12F);
			btnAdd.Location = new System.Drawing.Point(374, 6);
			btnAdd.Margin = new System.Windows.Forms.Padding(4);
			btnAdd.Name = "btnAdd" + stockInventory.Code;
			btnAdd.Size = new System.Drawing.Size(25, 25);
			btnAdd.TabIndex = 11;
			btnAdd.Tag = stockInventory.Code;
			btnAdd.Text = "+";
			btnAdd.UseVisualStyleBackColor = true;

			// 
			// txtStockId
			// 
			TextBox txtStockId = new TextBox();
			txtStockId.Location = new System.Drawing.Point(71, 8);
			txtStockId.Margin = new System.Windows.Forms.Padding(4);
			txtStockId.Name = "txtStockId" + stockInventory.Code;
			txtStockId.Size = new System.Drawing.Size(43, 25);
			txtStockId.TabIndex = 1;
			txtStockId.Text = stockInventory.Code;
			txtStockId.Tag = stockInventory.Code;
			txtStockId.ReadOnly = true;

			// 
			// label6
			// 
			Label lbStockName = new Label();
			lbStockName.Location = new System.Drawing.Point(5, 12);
			lbStockName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lbStockName.Name = "lbStockName" + stockInventory.Code;
			lbStockName.Size = new System.Drawing.Size(50, 15);
			lbStockName.TabIndex = 13;
			lbStockName.Text = stockInventory.Code;
			lbStockName.Tag = stockInventory.Code;
			lbStockName.Click += lbStockName_Click;

			// 
			// lbStock
			// 
			Label lbStock = new Label();
			lbStock.AutoSize = true;
			lbStock.Location = new System.Drawing.Point(58, 12);
			lbStock.Name = "lbStock" + stockInventory.Code;
			lbStock.Size = new System.Drawing.Size(41, 15);
			lbStock.TabIndex = 67;
			lbStock.Text = Convert.ToString(stockInventory.Qty);
			//lbStock.Visible = false;
			// 
			// label4
			// 
			Label lbCoupon = new Label();
			lbCoupon.AutoSize = true;
			lbCoupon.Location = new System.Drawing.Point(775, 15);
			lbCoupon.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lbCoupon.Name = "lbCoupon" + stockInventory.Code;
			lbCoupon.Size = new System.Drawing.Size(14, 15);
			lbCoupon.TabIndex = 47;
			lbCoupon.Text = "0";
			lbCoupon.Tag = stockInventory.Code;
			lbCoupon.Click += new System.EventHandler(this.lbCoupon_Click);
			// 
			// btnDeal
			//  
			Button btnDeal = new Button();
			btnDeal.BackColor = System.Drawing.Color.ForestGreen;
			btnDeal.Enabled = true;
			btnDeal.Location = new System.Drawing.Point(1170, 9);
			btnDeal.Margin = new System.Windows.Forms.Padding(4);
			btnDeal.Name = "btnDeal" + stockInventory.Code;
			btnDeal.Size = new System.Drawing.Size(52, 29);
			btnDeal.TabIndex = 48;
			btnDeal.Text = stockInventory.Status;
			btnDeal.Tag = stockInventory.Code;
			btnDeal.UseVisualStyleBackColor = true;

			// 
			// btnMarginTradingSell
			// 
			Button btnMarginTradingSell = new Button();
			btnMarginTradingSell.BackColor = System.Drawing.Color.Aquamarine;
			btnMarginTradingSell.Location = new System.Drawing.Point(901, 8);
			btnMarginTradingSell.Margin = new System.Windows.Forms.Padding(4);
			btnMarginTradingSell.Name = "btnMarginTradingSell" + stockInventory.Code;
			btnMarginTradingSell.Size = new System.Drawing.Size(52, 29);
			btnMarginTradingSell.TabIndex = 49;
			btnMarginTradingSell.Text = "資賣";
			btnMarginTradingSell.Tag = stockInventory.Code;
			btnMarginTradingSell.UseVisualStyleBackColor = false;
			btnMarginTradingSell.Enabled = false;
			// 
			// btnLending
			// 
			Button btnLending = new Button();
			btnLending.BackColor = System.Drawing.Color.DeepPink;
			btnLending.Location = new System.Drawing.Point(1116, 8);
			btnLending.Margin = new System.Windows.Forms.Padding(4);
			btnLending.Name = "btnLending" + stockInventory.Code;
			btnLending.Size = new System.Drawing.Size(52, 29);
			btnLending.TabIndex = 50;
			btnLending.Text = "券買";
			btnLending.Tag = stockInventory.Code;
			btnLending.UseVisualStyleBackColor = false;
			btnLending.Enabled = false;

			Label lbHigh = new Label();
			lbHigh.AutoSize = true;
			lbHigh.Location = new System.Drawing.Point(400, 11);
			lbHigh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lbHigh.Name = "lbHigh" + stockInventory.Code;
			lbHigh.Size = new System.Drawing.Size(14, 15);
			lbHigh.TabIndex = 47;
			lbHigh.Text = "0";
			lbHigh.Tag = stockInventory.Code;


			// 
			// cbPriceType
			// 
			ComboBox cbPriceType = new ComboBox();
			cbPriceType.FormattingEnabled = true;
			cbPriceType.Items.AddRange(new object[] {
			"限價",
			"市價",
			"漲停",
			"跌停"});
			cbPriceType.Location = new System.Drawing.Point(161, 9);
			cbPriceType.Margin = new System.Windows.Forms.Padding(4);
			cbPriceType.Name = "cbPriceType" + stockInventory.Code;
			cbPriceType.Size = new System.Drawing.Size(34, 23);
			cbPriceType.TabIndex = 0;
			cbPriceType.SelectedIndex = 0;
			cbPriceType.Tag = stockInventory.Code;
			//cbPriceType.SelectedIndex = 1;
			// 
			// lbMa5
			// 
			Label lbMa5 = new Label();
			lbMa5.AutoSize = true;
			lbMa5.Location = new System.Drawing.Point(477, 15);
			lbMa5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lbMa5.Name = "lbMa5" + stockInventory.Code;
			lbMa5.Size = new System.Drawing.Size(22, 15);
			lbMa5.TabIndex = 52;
			lbMa5.Text = "日";
			// 
			// lbMa20
			// 
			Label lbMa20 = new Label();
			lbMa20.AutoSize = true;
			lbMa20.Location = new System.Drawing.Point(624, 15);
			lbMa20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lbMa20.Name = "lbMa20" + stockInventory.Code;
			lbMa20.Size = new System.Drawing.Size(22, 15);
			lbMa20.TabIndex = 53;
			lbMa20.Text = "月";
			// 
			// label12
			// 
			Label label12 = new Label();
			label12.AutoSize = true;
			label12.Location = new System.Drawing.Point(700, 15);
			label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label12.Name = "label12";
			label12.Size = new System.Drawing.Size(22, 15);
			label12.TabIndex = 54;
			label12.Text = "季";


			// 
			// txtMa5
			// 
			TextBox txtMa5 = new TextBox();
			txtMa5.Location = new System.Drawing.Point(493, 9);
			txtMa5.Margin = new System.Windows.Forms.Padding(4);
			txtMa5.Name = "txtMa5" + stockInventory.Code;
			//txtMa5.ReadOnly = true;
			txtMa5.Size = new System.Drawing.Size(45, 25);
			txtMa5.TabIndex = 55;
			txtMa5.Text = Convert.ToString(_Ma5);
			// 
			// txtMa20
			// 
			TextBox txtMa20 = new TextBox();
			txtMa20.Location = new System.Drawing.Point(648, 9);
			txtMa20.Margin = new System.Windows.Forms.Padding(4);
			txtMa20.Name = "txtMa20" + stockInventory.Code;
			//txtMa20.ReadOnly = true;
			txtMa20.Size = new System.Drawing.Size(45, 25);
			txtMa20.TabIndex = 57;
			txtMa20.Text = Convert.ToString(_Ma20);
			// 
			// txtMa60
			// 
			TextBox txtMa60 = new TextBox();
			txtMa60.Location = new System.Drawing.Point(718, 9);
			txtMa60.Margin = new System.Windows.Forms.Padding(4);
			txtMa60.Name = "txtMa60" + stockInventory.Code;
			//txtMa60.ReadOnly = true;
			txtMa60.Size = new System.Drawing.Size(45, 25);
			txtMa60.TabIndex = 56;
			txtMa60.Text = Convert.ToString(_Ma60);
			// 
			// lbMa10
			// 
			Label lbMa10 = new Label();
			lbMa10.AutoSize = true;
			lbMa10.Location = new System.Drawing.Point(546, 14);
			lbMa10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lbMa10.Name = "lbMa10" + stockInventory.Code;
			lbMa10.Size = new System.Drawing.Size(36, 15);
			lbMa10.TabIndex = 58;
			lbMa10.Text = "10日";
			// 
			// txtMa10
			// 
			TextBox txtMa10 = new TextBox();
			txtMa10.Location = new System.Drawing.Point(579, 9);
			txtMa10.Margin = new System.Windows.Forms.Padding(4);
			txtMa10.Name = "txtMa10" + stockInventory.Code;
			//txtMa10.ReadOnly = true;
			txtMa10.Size = new System.Drawing.Size(45, 25);
			txtMa10.TabIndex = 59;
			txtMa10.Text = Convert.ToString(_Ma10);
			// 
			// btnReset
			// 
			Button btnReset = new Button();
			btnReset.BackColor = System.Drawing.Color.Gray;
			btnReset.Location = new System.Drawing.Point(1241, 8);
			btnReset.Margin = new System.Windows.Forms.Padding(4);
			btnReset.Name = "btnReset" + stockInventory.Code;
			btnReset.Size = new System.Drawing.Size(52, 29);
			btnReset.TabIndex = 60;
			btnReset.Text = "重置";
			btnReset.UseVisualStyleBackColor = true;
			btnReset.Tag = stockInventory.Code;

			// 
			// panel1
			// 
			// 
			// lbCost
			// 
			Label lbCost = new Label();
			lbCost.AutoSize = true;
			lbCost.Location = new System.Drawing.Point(236, 13);
			lbCost.Name = "lbCost" + stockInventory.Code;
			lbCost.Size = new System.Drawing.Size(0, 12);
			lbCost.TabIndex = 64;
			// 
			// lbPersent
			// 
			Label lbPersent = new Label();
			lbPersent.AutoSize = true;
			lbPersent.Location = new System.Drawing.Point(270, 13);
			lbPersent.Name = "lbPersent" + stockInventory.Code;
			lbPersent.Size = new System.Drawing.Size(41, 15);
			lbPersent.TabIndex = 65;
			//lbSplit

			Label lbSplit = new Label();
			lbSplit.AutoSize = true;
			lbSplit.Location = new System.Drawing.Point(230, 13);
			lbSplit.Name = "lbSplit" + stockInventory.Code;
			lbSplit.Size = new System.Drawing.Size(8, 12);
			lbSplit.TabIndex = 63;
			lbSplit.Text = "/";

			// 
			// txtStopLost
			// 
			TextBox txtStopLost = new TextBox();
			txtStopLost.Location = new System.Drawing.Point(1356, 8);
			txtStopLost.Margin = new System.Windows.Forms.Padding(4);
			txtStopLost.Name = "txtStopLost" + stockInventory.Code;
			txtStopLost.Size = new System.Drawing.Size(34, 25);
			txtStopLost.TabIndex = 63;
			txtStopLost.Text = stockInventory.Defence;
			// 
			// txtTarget
			// 
			TextBox txtTarget = new TextBox();
			txtTarget.Location = new System.Drawing.Point(1396, 9);
			txtTarget.Margin = new System.Windows.Forms.Padding(4);
			txtTarget.Name = "txtTarget" + stockInventory.Code;
			txtTarget.Size = new System.Drawing.Size(34, 25);
			txtTarget.TabIndex = 64;
			txtTarget.Text = stockInventory.Target;
			// 
			// lbProfit
			// 
			Label lbProfit = new Label();
			lbProfit.AutoSize = true;
			lbProfit.Location = new System.Drawing.Point(1300, 12);
			lbProfit.Name = "lbProfit" + stockInventory.Code;
			lbProfit.Size = new System.Drawing.Size(0, 15);
			lbProfit.TabIndex = 66;
			// 
			// chkAll
			// 
			CheckBox chkAll = new CheckBox();
			chkAll.AutoSize = true;
			chkAll.Location = new System.Drawing.Point(1433, 12);
			chkAll.Name = "chkAll" + stockInventory.Code;
			chkAll.Size = new System.Drawing.Size(59, 19);
			chkAll.TabIndex = 68;
			chkAll.Text = "全砍";
			chkAll.UseVisualStyleBackColor = true;
			chkAll.Checked = Convert.ToBoolean(stockInventory.SellAll);
			// 
			// chkCloseStopLost
			// 
			CheckBox chkCloseStopLost = new CheckBox();
			chkCloseStopLost.AutoSize = true;
			chkCloseStopLost.Location = new System.Drawing.Point(1483, 12);
			chkCloseStopLost.Name = "chkCloseStopLost" + stockInventory.Code;
			chkCloseStopLost.Size = new System.Drawing.Size(59, 19);
			chkCloseStopLost.TabIndex = 80;
			chkCloseStopLost.Text = "AVG";
			chkCloseStopLost.UseVisualStyleBackColor = true;
			chkCloseStopLost.Checked = Convert.ToBoolean(stockInventory.CloseStopLost);
			// 
			// chkCloseABSStopLost
			// 
			CheckBox chkCloseABSStopLost = new CheckBox();
			chkCloseABSStopLost.AutoSize = true;
			chkCloseABSStopLost.Location = new System.Drawing.Point(1533, 12);
			chkCloseABSStopLost.Name = "chkCloseABSStopLost" + stockInventory.Code;
			chkCloseABSStopLost.Size = new System.Drawing.Size(59, 19);
			chkCloseABSStopLost.TabIndex = 80;
			chkCloseABSStopLost.Text = "ABS";
			chkCloseABSStopLost.UseVisualStyleBackColor = true;
			chkCloseABSStopLost.Checked = Convert.ToBoolean(stockInventory.CloseABSStopLost);

			// 
			// chkCloseHighStopLost
			// 
			CheckBox chkCloseHighStopLost = new CheckBox();
			chkCloseHighStopLost.AutoSize = true;
			chkCloseHighStopLost.Location = new System.Drawing.Point(1583, 12);
			chkCloseHighStopLost.Name = "chkCloseHighStopLost" + stockInventory.Code;
			chkCloseHighStopLost.Size = new System.Drawing.Size(59, 19);
			chkCloseHighStopLost.TabIndex = 80;
			chkCloseHighStopLost.Text = "HIGH";
			chkCloseHighStopLost.UseVisualStyleBackColor = true;
			chkCloseHighStopLost.Checked = Convert.ToBoolean(stockInventory.CloseHighStopLost);

			Label lbDealQty = new Label();
			lbDealQty.Location = new System.Drawing.Point(1653, 12);
			lbDealQty.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lbDealQty.Name = "lbDealQty" + stockInventory.Code;
			lbDealQty.Size = new System.Drawing.Size(50, 15);
			lbDealQty.TabIndex = 13;
			lbDealQty.Text = "";
			lbDealQty.ForeColor = Color.DodgerBlue;

			Label lbTotalDealQty = new Label();
			lbTotalDealQty.Location = new System.Drawing.Point(1705, 12);
			lbTotalDealQty.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lbTotalDealQty.Name = "lbTotalDealQty" + stockInventory.Code;
			lbTotalDealQty.Size = new System.Drawing.Size(50, 15);
			lbTotalDealQty.TabIndex = 13;
			lbTotalDealQty.Text = "";
			lbTotalDealQty.ForeColor = Color.Blue;



			Panel panel1 = new Panel();
			if (!mode)
			{
				panel1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
				btnDeal.Click += new System.EventHandler(this.btnDeal_Click);
				btnReset.Click += new System.EventHandler(this.btnReset_Click);
				btnSell.Click += new System.EventHandler(this.btnSell_Click);
				btnMarginTrading.Click += new System.EventHandler(this.btnMarginTrading_Click);
				btnLending.Click += new System.EventHandler(this.btnLending_Click);
				btnBuy.Click += new EventHandler(this.btnBuy_Click);
				btnSellingShort.Click += new EventHandler(this.btnSellingShort_Click);
				btnOverSell.Click += new EventHandler(this.btnOverSell_Click);
				btnReset.Click += new EventHandler(this.btnReset_Click);
				btnAdd.Click += new EventHandler(this.btnAdd_Click);
				btnMinus.Click += new EventHandler(this.btnMinus_Click);
				cbPriceType.SelectedIndexChanged += new System.EventHandler(this.cbPriceType_SelectedIndexChanged);
				btnMarginTradingSell.Click += new EventHandler(this.btnMarginTradingSell_Click);
			}
			else
			{
				lbCost.Text = stockInventory.Cost;
				panel1.Controls.Add(lbSplit);
				panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
				btnDeal.Click += new System.EventHandler(this.btnDealStock_Click);
				btnReset.Click += new System.EventHandler(this.btnResetStock_Click);
				btnSell.Click += new System.EventHandler(this.btnSellStock_Click);
				btnMarginTrading.Click += new System.EventHandler(this.btnMarginTradingStock_Click);
				btnLending.Click += new System.EventHandler(this.btnLendingStock_Click);
				btnBuy.Click += new EventHandler(this.btnBuyStock_Click);
				btnSellingShort.Click += new EventHandler(this.btnSellingShortStock_Click);
				btnOverSell.Click += new EventHandler(this.btnOverSellStock_Click);
				btnReset.Click += new EventHandler(this.btnResetStock_Click);
				btnAdd.Click += new EventHandler(this.btnAddStock_Click);
				btnMinus.Click += new EventHandler(this.btnMinusStock_Click);
				cbPriceType.SelectedIndexChanged += new System.EventHandler(this.cbPriceTypeStock_SelectedIndexChanged);
				btnMarginTradingSell.Click += new EventHandler(this.btnMarginTradingSellStock_Click);
				//若是新建部位需判斷成本是否小於均線,若小於均線則為逆勢交易,先將均線停損策略關閉
				if (IsNew)
				{
					if (Convert.ToDouble(stockInventory.Cost) < _Ma5 || Convert.ToDouble(stockInventory.Cost) < _Ma10 || Convert.ToDouble(stockInventory.Cost) < _Ma20 || Convert.ToDouble(stockInventory.Cost) < _Ma60)
						chkCloseStopLost.Checked = true;
				}
				panel1.Controls.Add(txtStopLost);
				panel1.Controls.Add(txtTarget);
				panel1.Controls.Add(chkAll);
				panel1.Controls.Add(chkCloseStopLost);
				panel1.Controls.Add(chkCloseABSStopLost);
				panel1.Controls.Add(chkCloseHighStopLost);
				panel1.Controls.Add(lbHigh);
			}
			panel1.Controls.Add(txtMa10);
			panel1.Controls.Add(lbMa10);
			panel1.Controls.Add(txtMa60);
			panel1.Controls.Add(txtMa20);
			panel1.Controls.Add(txtMa5);
			panel1.Controls.Add(label12);
			panel1.Controls.Add(lbMa20);
			panel1.Controls.Add(lbMa5);
			panel1.Controls.Add(cbPriceType);
			panel1.Controls.Add(btnLending);
			panel1.Controls.Add(btnMarginTradingSell);
			panel1.Controls.Add(btnDeal);
			panel1.Controls.Add(lbCoupon);
			panel1.Controls.Add(lbStockName);
			panel1.Controls.Add(txtStockId);
			panel1.Controls.Add(btnAdd);
			panel1.Controls.Add(btnSellingShort);
			panel1.Controls.Add(btnMinus);
			panel1.Controls.Add(btnMarginTrading);
			panel1.Controls.Add(lbPrice);
			panel1.Controls.Add(txtPrice);
			panel1.Controls.Add(btnOverSell);
			panel1.Controls.Add(nupQty);
			panel1.Controls.Add(btnBuy);
			panel1.Controls.Add(btnSell);
			panel1.Controls.Add(btnReset);
			panel1.Controls.Add(lbCost);

			panel1.Controls.Add(lbPersent);
			panel1.Controls.Add(lbProfit);
			panel1.Controls.Add(lbStock);
			panel1.Controls.Add(lbDealQty);
			panel1.Controls.Add(lbTotalDealQty);
			panel1.Location = new System.Drawing.Point(23, 103);
			panel1.Margin = new System.Windows.Forms.Padding(4);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(2000, 44);
			panel1.TabIndex = 12;
			return panel1;
		}
		#endregion
		#region 期貨庫存
		/// <summary>
		/// 動態產生期貨庫存
		/// 庫存容器:
		/// 1. 計算出MA5/MA10/MA20/MA60均價
		/// 2. 背景色:粉紅色
		/// 3. 顯示期貨成本
		/// 4.  
		/// </summary>
		/// <param name="s"></param>
		/// <param name="piece">口數</param>
		/// <param name="value">股價</param> 
		/// <param name="mode">0:觀察名單,1:庫存</param> 
		/// <param name="status">狀態</param> 
		/// <param name="IsNew">新建倉</param> 
		///  <param name="s">期貨名</param> 
		private Panel dynamicFutures(string s, decimal piece, string value, string status, string name, bool chkPt)
		{

			double _Ma5 = 0, _Ma10 = 0, _Ma20 = 0, _Ma60 = 0;
			DataAccess _DataAccess = new DataAccess();
			string _EnCode = s.Substring(0, 2);
			string _Code = "";
			if (this._StockFutureCodeList.Where(x => x.EnCode == _EnCode).ToList().Count > 0)
			{
				_Code = this._StockFutureCodeList.Where(x => x.EnCode == _EnCode).First().Code;

				List<DataModel.Stock.Stock> _StockList = _DataAccess.getStockBySqlList(_Code, "Code");
				if (_StockList.Count > 5) _Ma5 = Math.Round(_StockList.OrderByDescending(x => x.Date).ToList().Take(5).Sum(x => Convert.ToDouble(x.ClosingPrice)) / 5, 2);
				if (_StockList.Count > 10) _Ma10 = Math.Round(_StockList.OrderByDescending(x => x.Date).ToList().Take(10).Sum(x => Convert.ToDouble(x.ClosingPrice)) / 10, 2);
				if (_StockList.Count > 20) _Ma20 = Math.Round(_StockList.OrderByDescending(x => x.Date).ToList().Take(20).Sum(x => Convert.ToDouble(x.ClosingPrice)) / 20, 2);
				if (_StockList.Count > 60) _Ma60 = Math.Round(_StockList.OrderByDescending(x => x.Date).ToList().Take(60).Sum(x => Convert.ToDouble(x.ClosingPrice)) / 60, 2);
			}
			//}
			// string s = "t00";
			// 
			// btnSell
			// 
			Button btnSell = new Button();
			btnSell.BackColor = System.Drawing.Color.LightGreen;
			btnSell.Location = new System.Drawing.Point(1062, 8);
			btnSell.Margin = new System.Windows.Forms.Padding(4);
			btnSell.Name = "btnSell" + s;
			btnSell.Size = new System.Drawing.Size(52, 29);
			btnSell.TabIndex = 6;
			btnSell.Tag = s;
			btnSell.Text = "現賣";
			btnSell.UseVisualStyleBackColor = false;

			// 
			// btnBuy
			// 
			Button btnBuy = new Button();
			btnBuy.BackColor = System.Drawing.Color.Red;
			btnBuy.Location = new System.Drawing.Point(1009, 8);
			btnBuy.Margin = new System.Windows.Forms.Padding(4);
			btnBuy.Name = "btnBuy" + s;
			btnBuy.Size = new System.Drawing.Size(52, 29);
			btnBuy.TabIndex = 5;
			btnBuy.Tag = s;
			btnBuy.Text = "現買";
			btnBuy.UseVisualStyleBackColor = false;

			// 
			// nupQty
			// 
			NumericUpDown nupLots = new NumericUpDown();
			nupLots.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			nupLots.Location = new System.Drawing.Point(120, 8);
			nupLots.Margin = new System.Windows.Forms.Padding(4);
			nupLots.Name = "nupLots" + s;
			nupLots.Size = new System.Drawing.Size(35, 25);
			nupLots.TabIndex = 8;
			nupLots.Value = piece;
			nupLots.Maximum = 200;



			// 
			// lbPrice
			// 
			Label lbPrice = new Label();
			lbPrice.AutoSize = true;
			lbPrice.Location = new System.Drawing.Point(196, 13);
			lbPrice.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lbPrice.Name = "lbPrice" + s;
			lbPrice.Size = new System.Drawing.Size(52, 15);
			lbPrice.TabIndex = 62;
			// 
			// txtPrice
			// 
			TextBox txtPrice = new TextBox();
			txtPrice.Location = new System.Drawing.Point(305, 8);
			txtPrice.Margin = new System.Windows.Forms.Padding(4);
			txtPrice.Name = "txtPrice" + s;
			txtPrice.Size = new System.Drawing.Size(40, 29);
			txtPrice.TabIndex = 9;


			// 
			// btnMinus
			// 
			Button btnMinus = new Button();
			btnMinus.Font = new System.Drawing.Font("新細明體", 12F);
			btnMinus.Location = new System.Drawing.Point(345, 6);
			btnMinus.Margin = new System.Windows.Forms.Padding(4);
			btnMinus.Name = "btnMinus" + s;
			btnMinus.Size = new System.Drawing.Size(25, 25);
			btnMinus.TabIndex = 10;
			btnMinus.Tag = s;
			btnMinus.Text = "-";
			btnMinus.UseVisualStyleBackColor = true;


			// 
			// btnAdd
			// 
			Button btnAdd = new Button();
			btnAdd.Font = new System.Drawing.Font("新細明體", 12F);
			btnAdd.Location = new System.Drawing.Point(374, 6);
			btnAdd.Margin = new System.Windows.Forms.Padding(4);
			btnAdd.Name = "btnAdd" + s;
			btnAdd.Size = new System.Drawing.Size(25, 25);
			btnAdd.TabIndex = 11;
			btnAdd.Tag = s;
			btnAdd.Text = "+";
			btnAdd.UseVisualStyleBackColor = true;

			// 
			// txtStockId
			// 
			TextBox txtStockId = new TextBox();
			txtStockId.Location = new System.Drawing.Point(71, 8);
			txtStockId.Margin = new System.Windows.Forms.Padding(4);
			txtStockId.Name = "txtStockId" + s;
			txtStockId.Size = new System.Drawing.Size(43, 25);
			txtStockId.TabIndex = 1;
			txtStockId.Text = s;
			txtStockId.Tag = s;
			txtStockId.ReadOnly = true;

			// 
			// label6
			// 
			Label lbStockName = new Label();
			lbStockName.Location = new System.Drawing.Point(5, 12);
			lbStockName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lbStockName.Name = "lbStockName" + s;
			lbStockName.Size = new System.Drawing.Size(54, 15);
			lbStockName.TabIndex = 13;
			lbStockName.Text = name;

			// 
			// lbStock
			// 
			Label lbStock = new Label();
			lbStock.AutoSize = true;
			lbStock.Location = new System.Drawing.Point(58, 12);
			lbStock.Name = "lbStock" + s;
			lbStock.Size = new System.Drawing.Size(41, 15);
			lbStock.TabIndex = 67;
			lbStock.Text = Convert.ToString(piece);
			//lbStock.Visible = false;

			// 
			// btnDeal
			//  
			Button btnDeal = new Button();
			btnDeal.BackColor = System.Drawing.Color.ForestGreen;
			btnDeal.Enabled = true;
			btnDeal.Location = new System.Drawing.Point(1170, 9);
			btnDeal.Margin = new System.Windows.Forms.Padding(4);
			btnDeal.Name = "btnDeal" + s;
			btnDeal.Size = new System.Drawing.Size(52, 29);
			btnDeal.TabIndex = 48;
			btnDeal.Text = status;
			btnDeal.Tag = s;
			btnDeal.UseVisualStyleBackColor = true;



			// 
			// cbPriceType
			// 
			ComboBox cbOrderType = new ComboBox();
			cbOrderType.FormattingEnabled = true;
			cbOrderType.Items.AddRange(new object[] {
			"限價",
			"市價"});
			cbOrderType.Location = new System.Drawing.Point(161, 9);
			cbOrderType.Margin = new System.Windows.Forms.Padding(4);
			cbOrderType.Name = "cbOrderType" + s;
			cbOrderType.Size = new System.Drawing.Size(34, 23);
			cbOrderType.TabIndex = 0;
			cbOrderType.Tag = s;
			cbOrderType.SelectedIndex = 1;
			// 
			// lbMa5
			// 
			Label lbMa5 = new Label();
			lbMa5.AutoSize = true;
			lbMa5.Location = new System.Drawing.Point(477, 15);
			lbMa5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lbMa5.Name = "lbMa5" + s;
			lbMa5.Size = new System.Drawing.Size(22, 15);
			lbMa5.TabIndex = 52;
			lbMa5.Text = "日";
			// 
			// lbMa20
			// 
			Label lbMa20 = new Label();
			lbMa20.AutoSize = true;
			lbMa20.Location = new System.Drawing.Point(624, 15);
			lbMa20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lbMa20.Name = "lbMa20" + s;
			lbMa20.Size = new System.Drawing.Size(22, 15);
			lbMa20.TabIndex = 53;
			lbMa20.Text = "月";
			// 
			// label12
			// 
			Label label12 = new Label();
			label12.AutoSize = true;
			label12.Location = new System.Drawing.Point(700, 15);
			label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label12.Name = "label12";
			label12.Size = new System.Drawing.Size(22, 15);
			label12.TabIndex = 54;
			label12.Text = "季";


			// 
			// txtMa5
			// 
			TextBox txtMa5 = new TextBox();
			txtMa5.Location = new System.Drawing.Point(493, 9);
			txtMa5.Margin = new System.Windows.Forms.Padding(4);
			txtMa5.Name = "txtMa5" + s;
			//txtMa5.ReadOnly = true;
			txtMa5.Size = new System.Drawing.Size(45, 25);
			txtMa5.TabIndex = 55;
			txtMa5.Text = Convert.ToString(_Ma5) != "0" ? Convert.ToString(_Ma5) : this.txtFutureMa5.Text;
			// 
			// txtMa20
			// 
			TextBox txtMa20 = new TextBox();
			txtMa20.Location = new System.Drawing.Point(648, 9);
			txtMa20.Margin = new System.Windows.Forms.Padding(4);
			txtMa20.Name = "txtMa20" + s;
			//txtMa20.ReadOnly = true;
			txtMa20.Size = new System.Drawing.Size(45, 25);
			txtMa20.TabIndex = 57;
			txtMa20.Text = Convert.ToString(_Ma20) != "0" ? Convert.ToString(_Ma20) : this.txtFutureMa20.Text;
			// 
			// txtMa60
			// 
			TextBox txtMa60 = new TextBox();
			txtMa60.Location = new System.Drawing.Point(718, 9);
			txtMa60.Margin = new System.Windows.Forms.Padding(4);
			txtMa60.Name = "txtMa60" + s;
			//txtMa60.ReadOnly = true;
			txtMa60.Size = new System.Drawing.Size(45, 25);
			txtMa60.TabIndex = 56;
			txtMa60.Text = Convert.ToString(_Ma60) != "0" ? Convert.ToString(_Ma60) : this.txtFutureMa60.Text;
			// 
			// lbMa10
			// 
			Label lbMa10 = new Label();
			lbMa10.AutoSize = true;
			lbMa10.Location = new System.Drawing.Point(546, 14);
			lbMa10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lbMa10.Name = "lbMa10" + s;
			lbMa10.Size = new System.Drawing.Size(36, 15);
			lbMa10.TabIndex = 58;
			lbMa10.Text = "10日";
			// 
			// txtMa10
			// 
			TextBox txtMa10 = new TextBox();
			txtMa10.Location = new System.Drawing.Point(579, 9);
			txtMa10.Margin = new System.Windows.Forms.Padding(4);
			txtMa10.Name = "txtMa10" + s;
			//txtMa10.ReadOnly = true;
			txtMa10.Size = new System.Drawing.Size(45, 25);
			txtMa10.TabIndex = 59;
			txtMa10.Text = Convert.ToString(_Ma10) != "0" ? Convert.ToString(_Ma10) : this.txtFutureMa10.Text;


			// 
			// panel1
			// 
			// 
			// lbCost
			// 
			Label lbCost = new Label();
			lbCost.AutoSize = true;
			lbCost.Location = new System.Drawing.Point(236, 13);
			lbCost.Name = "lbCost" + s;
			lbCost.Size = new System.Drawing.Size(0, 12);
			lbCost.TabIndex = 64;
			// 
			// lbPersent
			// 
			Label lbPersent = new Label();
			lbPersent.AutoSize = true;
			lbPersent.Location = new System.Drawing.Point(280, 13);
			lbPersent.Name = "lbPersent" + s;
			lbPersent.Size = new System.Drawing.Size(41, 15);
			lbPersent.TabIndex = 65;
			//lbSplit

			Label lbSplit = new Label();
			lbSplit.AutoSize = true;
			lbSplit.Location = new System.Drawing.Point(230, 13);
			lbSplit.Name = "lbSplit" + s;
			lbSplit.Size = new System.Drawing.Size(8, 12);
			lbSplit.TabIndex = 63;
			lbSplit.Text = "/";

			// 
			// txtStopLost
			// 
			TextBox txtStopLost = new TextBox();
			txtStopLost.Location = new System.Drawing.Point(1356, 8);
			txtStopLost.Margin = new System.Windows.Forms.Padding(4);
			txtStopLost.Name = "txtStopLost" + s;
			txtStopLost.Size = new System.Drawing.Size(34, 25);
			txtStopLost.TabIndex = 63;
			// 
			// txtTarget
			// 
			TextBox txtTarget = new TextBox();
			txtTarget.Location = new System.Drawing.Point(1396, 9);
			txtTarget.Margin = new System.Windows.Forms.Padding(4);
			txtTarget.Name = "txtTarget" + s;
			txtTarget.Size = new System.Drawing.Size(34, 25);
			txtTarget.TabIndex = 64;
			// 
			// lbProfit
			// 
			Label lbProfit = new Label();
			lbProfit.AutoSize = true;
			lbProfit.Location = new System.Drawing.Point(1300, 12);
			lbProfit.Name = "lbProfit" + s;
			lbProfit.Size = new System.Drawing.Size(0, 15);
			lbProfit.TabIndex = 66;
			// 
			// chkAll
			// 
			CheckBox chkAll = new CheckBox();
			chkAll.AutoSize = true;
			chkAll.Location = new System.Drawing.Point(1433, 12);
			chkAll.Name = "chkAll" + s;
			chkAll.Size = new System.Drawing.Size(59, 19);
			chkAll.TabIndex = 68;
			chkAll.Text = "全砍";
			chkAll.UseVisualStyleBackColor = true;
			// 
			// chkCloseStopLost
			// 
			CheckBox chkCloseStopLost = new CheckBox();
			chkCloseStopLost.AutoSize = true;
			chkCloseStopLost.Location = new System.Drawing.Point(1583, 12);
			chkCloseStopLost.Name = "chkCloseStopLost" + s;
			chkCloseStopLost.Size = new System.Drawing.Size(59, 19);
			chkCloseStopLost.TabIndex = 80;
			chkCloseStopLost.Text = "AVG";
			chkCloseStopLost.UseVisualStyleBackColor = true;

			// 
			// chkCloseABSStopLost
			// 
			CheckBox chkCloseABSStopLost = new CheckBox();
			chkCloseABSStopLost.AutoSize = true;
			chkCloseABSStopLost.Location = new System.Drawing.Point(1533, 12);
			chkCloseABSStopLost.Name = "chkCloseABSStopLost" + s;
			chkCloseABSStopLost.Size = new System.Drawing.Size(59, 19);
			chkCloseABSStopLost.TabIndex = 80;
			chkCloseABSStopLost.Text = "ABS";
			chkCloseABSStopLost.UseVisualStyleBackColor = true;
			//  chkCloseABSStopLost.Checked = true;

			CheckBox chkClosePtStopLost = new CheckBox();
			chkClosePtStopLost.AutoSize = true;
			chkClosePtStopLost.Location = new System.Drawing.Point(1483, 12);
			chkClosePtStopLost.Name = "chkClosePtStopLost" + s;
			chkClosePtStopLost.Size = new System.Drawing.Size(59, 19);
			chkClosePtStopLost.TabIndex = 80;
			chkClosePtStopLost.Text = "Pt";
			chkClosePtStopLost.UseVisualStyleBackColor = true;
			chkClosePtStopLost.Checked = chkPt;
			Label lbDealQty = new Label();
			lbDealQty.Location = new System.Drawing.Point(1653, 12);
			lbDealQty.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lbDealQty.Name = "lbDealQty" + s;
			lbDealQty.Size = new System.Drawing.Size(50, 15);
			lbDealQty.TabIndex = 13;
			lbDealQty.Text = "";
			lbDealQty.ForeColor = Color.DodgerBlue;

			Label lbTotalDealQty = new Label();
			lbTotalDealQty.Location = new System.Drawing.Point(1705, 12);
			lbTotalDealQty.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lbTotalDealQty.Name = "lbTotalDealQty" + s;
			lbTotalDealQty.Size = new System.Drawing.Size(50, 15);
			lbTotalDealQty.TabIndex = 13;
			lbTotalDealQty.Text = "";
			lbTotalDealQty.ForeColor = Color.Blue;



			Panel panel1 = new Panel();

			lbCost.Text = value;
			panel1.Controls.Add(lbSplit);
			panel1.BackColor = System.Drawing.Color.PapayaWhip;
			btnDeal.Click += new System.EventHandler(this.btnDeal_Click);
			btnSell.Click += new System.EventHandler(this.btnSellFutureStock_Click);
			btnBuy.Click += new EventHandler(this.btnBuyFutureStock_Click);
			btnAdd.Click += new EventHandler(this.btnFutureAddStock_Click);
			btnMinus.Click += new EventHandler(this.btnFutureMinusStock_Click);
			cbOrderType.SelectedIndexChanged += new System.EventHandler(this.cbPriceTypeStock_SelectedIndexChanged);

			panel1.Controls.Add(txtStopLost);
			panel1.Controls.Add(txtTarget);
			panel1.Controls.Add(chkAll);
			//個股期開啟均價停損
			if (_EnCode != "MX") panel1.Controls.Add(chkCloseStopLost);
			panel1.Controls.Add(chkCloseABSStopLost);
			//  panel1.Controls.Add(chkCloseHighStopLost);
			panel1.Controls.Add(chkClosePtStopLost);

			panel1.Controls.Add(txtMa10);
			panel1.Controls.Add(lbMa10);
			panel1.Controls.Add(txtMa60);
			panel1.Controls.Add(txtMa20);
			panel1.Controls.Add(txtMa5);
			panel1.Controls.Add(label12);
			panel1.Controls.Add(lbMa20);
			panel1.Controls.Add(lbMa5);
			panel1.Controls.Add(cbOrderType);

			panel1.Controls.Add(btnDeal);
			panel1.Controls.Add(lbStockName);
			panel1.Controls.Add(txtStockId);
			panel1.Controls.Add(btnAdd);
			panel1.Controls.Add(btnMinus);
			panel1.Controls.Add(lbPrice);
			panel1.Controls.Add(txtPrice);
			panel1.Controls.Add(nupLots);
			panel1.Controls.Add(btnBuy);
			panel1.Controls.Add(btnSell);
			panel1.Controls.Add(lbCost);

			panel1.Controls.Add(lbPersent);
			panel1.Controls.Add(lbProfit);
			panel1.Controls.Add(lbStock);
			panel1.Controls.Add(lbDealQty);
			panel1.Controls.Add(lbTotalDealQty);
			panel1.Location = new System.Drawing.Point(23, 103);
			panel1.Margin = new System.Windows.Forms.Padding(4);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(2000, 44);
			panel1.TabIndex = 12;
			return panel1;
		}
		#endregion
		/// <summary>
		/// 彈性下單
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void txtStockId_TextChanged(object sender, EventArgs e)
		{
			nupQty.Value = 1;
			if (txtStockId.Text.Length >= 4)
			{
				btnGetPrice.PerformClick();
			}
		}
		/// <summary>
		/// 根據分號動態產生觀察名單
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnClick_Click(object sender, EventArgs e)
		{

			string _Log = "";
			this.flowLayoutPanel1.Controls.Clear();
			try
			{
				//觀察名單
				string[] _Oberservers = txtOberserver.Text.Split(';');
				foreach (string s in _Oberservers)
				{
					WebApiService.Models.StockInventory stockInventory = new WebApiService.Models.StockInventory();
					stockInventory.Code = s;
					stockInventory.Qty = 1;
					stockInventory.Cost = "";
					stockInventory.Status = "成交";
					stockInventory.CloseABSStopLost = false;
					stockInventory.CloseHighStopLost = false;
					stockInventory.ClosePtLost = false;
					stockInventory.CloseStopLost = false;
					stockInventory.SellAll = false;
					Panel p = dynamicOberserver(stockInventory, false, false);
					flowLayoutPanel1.Controls.Add(p);
				}
			}
			catch (Exception ex)
			{
				_Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " btnClick:" + "\r\n" + ex.Message;
				logger.Error(_Log);
			}
		}
		/// <summary>
		/// 按下狀態按鈕會顯示對沖的按鈕 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDealStock_Click(object sender, EventArgs e)
		{
			showButton(sender, true);
		}
		/// <summary>
		/// 成交按鈕壓上後,代筆該筆委託單已成交只會顯示可沖按鈕
		/// 券賣:顯示[券買][資買]
		/// 沖賣:顯示[現買]
		/// 資買:顯示[資賣]
		/// 現買:顯示[現賣]
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDeal_Click(object sender, EventArgs e)
		{
			showButton(sender, false);
		}
		/// <summary>
		/// 庫存的話從庫存容器上捉取相關按鈕
		/// 券賣狀態:顯示[券買][資買]
		/// 資買狀態:顯示[資賣]
		/// 沖賣狀態:顯示[現買]
		/// 現買狀態:顯示[現賣]
		/// </summary>
		/// <param name="btn"></param>
		/// <param name="mode"></param>
		private void showButton(object sender, bool? mode)
		{
			Button _Btn = sender as Button;
			Button _BtnSell = new Button();
			Button _BtnBuy = new Button();
			Button _BtnOverSell = new Button();
			Button _BtnMarginTrading = new Button();
			Button _BtnSellingShort = new Button();
			Button _BtnMarginTradingSell = new Button();
			Button _BtnLending = new Button();
			_Btn.BackColor = Color.Yellow;
			_Btn.Enabled = false;
			if (mode != null)
			{
				if (Convert.ToBoolean(mode))
				{
					_BtnSell = (Button)flowLayoutPanelStock.Controls.Find("btnSell" + _Btn.Tag, true)[0];
					_BtnBuy = (Button)flowLayoutPanelStock.Controls.Find("btnBuy" + _Btn.Tag, true)[0];
					_BtnOverSell = (Button)flowLayoutPanelStock.Controls.Find("btnOverSell" + _Btn.Tag, true)[0];
					_BtnMarginTrading = (Button)flowLayoutPanelStock.Controls.Find("btnMarginTrading" + _Btn.Tag, true)[0];
					_BtnSellingShort = (Button)flowLayoutPanelStock.Controls.Find("btnSellingShort" + _Btn.Tag, true)[0];
					_BtnMarginTradingSell = (Button)flowLayoutPanelStock.Controls.Find("btnMarginTradingSell" + _Btn.Tag, true)[0];
					_BtnLending = (Button)flowLayoutPanelStock.Controls.Find("btnLending" + _Btn.Tag, true)[0];
				}
				else
				{
					_BtnSell = (Button)flowLayoutPanel1.Controls.Find("btnSell" + _Btn.Tag, true)[0];
					_BtnBuy = (Button)flowLayoutPanel1.Controls.Find("btnBuy" + _Btn.Tag, true)[0];
					_BtnOverSell = (Button)flowLayoutPanel1.Controls.Find("btnOverSell" + _Btn.Tag, true)[0];
					_BtnMarginTrading = (Button)flowLayoutPanel1.Controls.Find("btnMarginTrading" + _Btn.Tag, true)[0];
					_BtnSellingShort = (Button)flowLayoutPanel1.Controls.Find("btnSellingShort" + _Btn.Tag, true)[0];
					_BtnMarginTradingSell = (Button)flowLayoutPanel1.Controls.Find("btnMarginTradingSell" + _Btn.Tag, true)[0];
					_BtnLending = (Button)flowLayoutPanel1.Controls.Find("btnLending" + _Btn.Tag, true)[0];
				}
			}
			else
			{
				_BtnSell = (Button)flowLayoutFutures.Controls.Find("btnSell" + _Btn.Tag, true)[0];
				_BtnBuy = (Button)flowLayoutFutures.Controls.Find("btnBuy" + _Btn.Tag, true)[0];

			}
			_BtnLending.Enabled = false;
			_BtnBuy.Enabled = false;
			_BtnSell.Enabled = false;
			_BtnMarginTrading.Enabled = false;
			_BtnOverSell.Enabled = false;
			_BtnSellingShort.Enabled = false;
			_BtnMarginTradingSell.Enabled = false;

			_BtnBuy.BackColor = Color.Gainsboro;
			_BtnSell.BackColor = Color.Gainsboro;
			_BtnMarginTrading.BackColor = Color.Gainsboro;
			_BtnOverSell.BackColor = Color.Gainsboro;
			_BtnSellingShort.BackColor = Color.Gainsboro;
			_BtnMarginTradingSell.BackColor = Color.Gainsboro;
			_BtnLending.BackColor = Color.Gainsboro;
			switch (_Btn.Text)
			{
				case "券賣":
					_BtnMarginTrading.Enabled = true;
					_BtnMarginTrading.BackColor = Color.IndianRed;
					_BtnLending.Enabled = true;
					_BtnLending.BackColor = Color.DeepPink;
					break;
				case "資買":
					_BtnMarginTradingSell.Enabled = true;
					_BtnMarginTradingSell.BackColor = Color.Aquamarine;
					break;
				case "沖賣":
				case "現賣":
					_BtnBuy.Enabled = true;
					_BtnBuy.BackColor = Color.Red;
					break;
				case "現買":
					_BtnSell.Enabled = true;
					_BtnSell.BackColor = Color.LightGreen;
					break;
			}
		}
		/// <summary>
		/// 資賣
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnMarginTradingSellStock_Click(object sender, EventArgs e)
		{
			sendParameter(sender, true, "S", "M", "資賣", Color.DarkGreen);
		}
		/// <summary>
		/// 資賣
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnMarginTradingSell_Click(object sender, EventArgs e)
		{
			sendParameter(sender, false, "S", "M", "資賣", Color.Aquamarine);

		}
		/// <summary>
		/// 券買
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnLending_Click(object sender, EventArgs e)
		{
			sendParameter(sender, false, "B", "S", "券買", Color.DeepPink);

		}
		private void btnLendingStock_Click(object sender, EventArgs e)
		{
			sendParameter(sender, true, "B", "S", "券買", Color.DeepPink);
		}
		/// <summary>
		/// 一鍵將所有庫存出清,用於黑天鵝降臨崩盤的時候
		/// 執行前須先填入密碼
		/// 現買狀態的股票會全部掛跌停出清
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAllStopLoss_Click(object sender, EventArgs e)
		{
			string _BuySell = "S";
			string _CashMarginShort = "C";
			string _Units = "0";
			string _Price = "0";
			string _StockId = "";
			string _OrderType = "F";//跌停出
			string _Status = "";
			string _Password = ConfigurationManager.AppSettings["Password"];
			Button _BtnDeal = new Button();
			NumericUpDown _NupQty = new NumericUpDown();
			Label _LbStock = new Label();
			if (txtPassword.Text == _Password)
			{
				MessageBox.Show("出清庫存");
				//庫存
				foreach (Panel p in flowLayoutPanelStock.Controls.Cast<Panel>())
				{
					foreach (Control contr in p.Controls.Cast<Control>())
					{
						if (contr is TextBox)
						{
							if (contr.Tag != null)
							{
								_BtnDeal = (Button)flowLayoutPanelStock.Controls.Find("btnDeal" + contr.Tag.ToString(), true)[0];
								_LbStock = (Label)flowLayoutPanelStock.Controls.Find("lbStock" + contr.Tag.ToString(), true)[0];
								_CashMarginShort = _BtnDeal.Text == "資買" ? "M" : "C";
								_Status = _BtnDeal.Text == "資買" ? "資賣" : "現賣";
								_Units = Convert.ToString(_LbStock.Text);
								string _Parameter = "Market=S,Account=" + this.txtStockAccount.Text + ",Symbol=" + contr.Tag.ToString() + ",BuySell=" + _BuySell + ",Units=" + _Units + ",OrderType=" + _OrderType + ",Price=" + _Price + ",CashMarginShort=" + _CashMarginShort;
								StringBuilder sb = new StringBuilder(_Parameter);
								if (_BtnDeal.Text == "現買" || _BtnDeal.Text == "資買") HTSOrder(sb);
								string _Msg = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " 你" + _Status + _StockId + "共" + _Units + "張,成本" + _Price + "元";
								MessageBox.Show(_Parameter);
								Color color = _BtnDeal.Text == "資買" ? Color.Aquamarine : Color.Green;
								richTxtInfo.SelectionColor = color;
								richTxtInfo.AppendText(_Msg + Environment.NewLine);
							}
						}
					}
				}
			}
			else
			{
				MessageBox.Show("Enter Your Password", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
			}
		}
		/// <summary>
		/// 更改該委託編號的價格
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnChangePrice_Click(object sender, EventArgs e)
		{
			string _OrderID = "";
			string _Units = "0";
			string _Price = "0";
			string _StockId = "";
			Button _Btn = sender as Button;
			if (Convert.ToString(_Btn.Tag) == "")
			{
				_Units = Convert.ToString(nupQty.Value);
				_Price = txtPrice.Text != "" ? txtPrice.Text : lbPrice.Text;
				btnDeal.Text = "改價";
				_StockId = txtStockId.Text;
			}
			else
			{
				NumericUpDown _NupQty = (NumericUpDown)flowLayoutPanel1.Controls.Find("nupQty" + _Btn.Tag, true)[0];
				_Units = Convert.ToString(_NupQty.Value);
				TextBox _TxtPrice = (TextBox)flowLayoutPanel1.Controls.Find("txtPrice" + _Btn.Tag, true)[0];
				Label _LbPrice = (Label)flowLayoutPanel1.Controls.Find("lbPrice" + _Btn.Tag, true)[0];
				_Price = _TxtPrice.Text != "" ? _TxtPrice.Text : _LbPrice.Text;
				Button _BtnDeal = (Button)flowLayoutPanel1.Controls.Find("btnDeal" + _Btn.Tag, true)[0];
				_BtnDeal.Text = "改價";
				_StockId = _Btn.Tag.ToString();
			}
			string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _StockId + ",OrderID=" + _OrderID + ",Price=" + _Price + ",Units=" + _Units;
			StringBuilder sb = new StringBuilder(_Parameter);
			HTSOrderChgPrice(sb);
			MessageBox.Show(_Parameter);
			string _Msg = "你委託改價" + _StockId + "共" + _Units + "張,價格" + _Price + "元,委託編號:" + _OrderID;
			Color color = Color.PaleGoldenrod;
			richTxtInfo.SelectionColor = color;
			richTxtInfo.AppendText(_Msg + Environment.NewLine);
			richTxtInfo.SelectionStart = richTxtInfo.TextLength;
			richTxtInfo.ScrollToCaret();
		}
		/// <summary>
		/// 價格類型非限價的價格都設定為0
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cbPriceType_SelectedIndexChanged(object sender, EventArgs e)
		{
			priceTypeSelectedIndexChanged(sender, false);

		}
		private void cbPriceTypeStock_SelectedIndexChanged(object sender, EventArgs e)
		{
			priceTypeSelectedIndexChanged(sender, true);

		}
		private void priceTypeSelectedIndexChanged(object sender, bool mode)
		{
			ComboBox _CbPriceType = sender as ComboBox;
			if (Convert.ToString(_CbPriceType.Tag) == "")
			{
				if (this.cbPriceType.SelectedItem.ToString() != "限價")
				{
					this.txtPrice.Text = "0";
					this.txtPrice.Enabled = false;
					this.btnAdd.Enabled = false;
					this.btnMinus.Enabled = false;
				}
				else
				{
					this.txtPrice.Text = "";
					this.txtPrice.Enabled = true;
					this.btnAdd.Enabled = true;
					this.btnMinus.Enabled = true;

				}
			}
			else
			{
				TextBox _TxtPrice = new TextBox();
				Button _BtnAdd = new Button();
				Button _BtnMinus = new Button();
				if (mode)
				{
					_TxtPrice = (TextBox)flowLayoutPanelStock.Controls.Find("txtPrice" + _CbPriceType.Tag, true)[0];
					_BtnAdd = (Button)flowLayoutPanelStock.Controls.Find("btnAdd" + _CbPriceType.Tag, true)[0];
					_BtnMinus = (Button)flowLayoutPanelStock.Controls.Find("btnMinus" + _CbPriceType.Tag, true)[0];
				}
				else
				{
					_TxtPrice = (TextBox)flowLayoutPanel1.Controls.Find("txtPrice" + _CbPriceType.Tag, true)[0];
					_BtnAdd = (Button)flowLayoutPanel1.Controls.Find("btnAdd" + _CbPriceType.Tag, true)[0];
					_BtnMinus = (Button)flowLayoutPanel1.Controls.Find("btnMinus" + _CbPriceType.Tag, true)[0];
				}
				if (_CbPriceType.SelectedItem.ToString() != "限價")
				{
					_TxtPrice.Text = "0";
					_TxtPrice.Enabled = false;
					_BtnAdd.Enabled = false;
					_BtnMinus.Enabled = false;
				}
				else
				{
					_TxtPrice.Enabled = true;
					_BtnAdd.Enabled = true;
					_BtnMinus.Enabled = true;
				}
			}
		}
		/// <summary>
		/// 重置按鈕
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnReset_Click(object sender, EventArgs e)
		{
			resetButton(sender, false);

		}
		private void btnResetStock_Click(object sender, EventArgs e)
		{
			resetButton(sender, true);

		}
		private void resetButton(object sender, bool mode)
		{
			Button _Btn = sender as Button;
			this.cbPriceType.SelectedItem = "限價";
			this.nupQty.Value = 1;
			Button _BtnSell = new Button();
			Button _BtnBuy = new Button();
			Button _BtnOverSell = new Button();
			Button _BtnMarginTrading = new Button();
			Button _BtnSellingShort = new Button();
			Button _BtnMarginTradingSell = new Button();
			Button _BtnLending = new Button();
			Button _BtnDeal = new Button();
			if (mode)
			{
				_BtnSell = (Button)flowLayoutPanelStock.Controls.Find("btnSell" + _Btn.Tag, true)[0];
				_BtnBuy = (Button)flowLayoutPanelStock.Controls.Find("btnBuy" + _Btn.Tag, true)[0];
				_BtnOverSell = (Button)flowLayoutPanelStock.Controls.Find("btnOverSell" + _Btn.Tag, true)[0];
				_BtnMarginTrading = (Button)flowLayoutPanelStock.Controls.Find("btnMarginTrading" + _Btn.Tag, true)[0];
				_BtnSellingShort = (Button)flowLayoutPanelStock.Controls.Find("btnSellingShort" + _Btn.Tag, true)[0];
				_BtnMarginTradingSell = (Button)flowLayoutPanelStock.Controls.Find("btnMarginTradingSell" + _Btn.Tag, true)[0];
				_BtnLending = (Button)flowLayoutPanelStock.Controls.Find("btnLending" + _Btn.Tag, true)[0];
				_BtnDeal = (Button)flowLayoutPanelStock.Controls.Find("btnDeal" + _Btn.Tag, true)[0];
			}
			else
			{
				_BtnSell = (Button)flowLayoutPanel1.Controls.Find("btnSell" + _Btn.Tag, true)[0];
				_BtnBuy = (Button)flowLayoutPanel1.Controls.Find("btnBuy" + _Btn.Tag, true)[0];
				_BtnOverSell = (Button)flowLayoutPanel1.Controls.Find("btnOverSell" + _Btn.Tag, true)[0];
				_BtnMarginTrading = (Button)flowLayoutPanel1.Controls.Find("btnMarginTrading" + _Btn.Tag, true)[0];
				_BtnSellingShort = (Button)flowLayoutPanel1.Controls.Find("btnSellingShort" + _Btn.Tag, true)[0];
				_BtnMarginTradingSell = (Button)flowLayoutPanel1.Controls.Find("btnMarginTradingSell" + _Btn.Tag, true)[0];
				_BtnLending = (Button)flowLayoutPanel1.Controls.Find("btnLending" + _Btn.Tag, true)[0];
				_BtnDeal = (Button)flowLayoutPanel1.Controls.Find("btnDeal" + _Btn.Tag, true)[0];
			}
			_BtnBuy.Enabled = true;
			_BtnMarginTrading.Enabled = true;
			_BtnOverSell.Enabled = true;
			_BtnSellingShort.Enabled = true;
			_BtnDeal.Enabled = true;
			_BtnBuy.BackColor = Color.Red;
			_BtnSell.BackColor = Color.LightGreen;
			_BtnMarginTrading.BackColor = Color.IndianRed;
			_BtnOverSell.BackColor = Color.Salmon;
			_BtnSellingShort.BackColor = Color.DarkGreen;
			_BtnMarginTradingSell.BackColor = Color.Aquamarine;
			_BtnLending.BackColor = Color.DeepPink;
			_BtnDeal.Text = "成交";
			_BtnDeal.BackColor = SystemColors.Control;

		}
		private void btnGetPrice_Click(object sender, EventArgs e)
		{
			string _Log = "";
			string _Id = "Price1_lbTPrice";
			string _StockFuture_URL = ConfigurationManager.AppSettings["HiStock_URL"];
			try
			{
				Button _Btn = sender as Button;
				Stock.GetRealtimePriceIn _RealtimePriceIn = new Stock.GetRealtimePriceIn();
				if (Convert.ToString(_Btn.Tag) == "") { _RealtimePriceIn.Sample1_Symbol = this.txtStockId.Text; }
				List<Stock.GetRealtimeStockPrice> _PriceList = StockPrice.GetRealtimePrice(_RealtimePriceIn);
				if (_PriceList.Count() == 1)
				{
					this.lbPrice.Text = _PriceList.First().Price;
					this.lbStockName.Text = _PriceList.First().StockName.Length > 4 ? _PriceList.First().StockName.Substring(0, 5) : _PriceList.First().StockName;
				}
				else
				{
					this.lbPrice.Text = Common.Job.GetTaiwanFutures(_StockFuture_URL + this.txtStockId.Text, _Id, true);
					if (_StockGroupList.Where(x => x.Code == this.txtStockId.Text).ToList().Count > 0)
						this.lbStockName.Text = _StockGroupList.Where(x => x.Code == this.txtStockId.Text).First().Name;

				}
				//Stock.GetMonthPriceOut _GetMonthPriceOutList = getQuaterMonthPriceOutList(txtStockId.Text);
				//double _Ma5 = Math.Round(_GetMonthPriceOutList.gridList.OrderByDescending(x => x.date).ToList().Take(5).Sum(x => Convert.ToDouble(x.close)) / 5, 2);
				//double _Ma10 = Math.Round(_GetMonthPriceOutList.gridList.OrderByDescending(x => x.date).ToList().Take(10).Sum(x => Convert.ToDouble(x.close)) / 10, 2);
				//double _Ma20 = Math.Round(_GetMonthPriceOutList.gridList.OrderByDescending(x => x.date).ToList().Take(20).Sum(x => Convert.ToDouble(x.close)) / 20, 2);
				//double _Ma60 = Math.Round(_GetMonthPriceOutList.gridList.OrderByDescending(x => x.date).ToList().Take(60).Sum(x => Convert.ToDouble(x.close)) / 60, 2);
				double _Ma5 = 0, _Ma10 = 0, _Ma20 = 0, _Ma60 = 0;
				List<DataModel.Stock.Stock> _StockList = _DataAccess.getStockBySqlList(txtStockId.Text, "Code");
				if (_StockList.Count > 5) _Ma5 = Math.Round(_StockList.OrderByDescending(x => x.Date).ToList().Take(5).Sum(x => Convert.ToDouble(x.ClosingPrice)) / 5, 2);
				if (_StockList.Count > 10) _Ma10 = Math.Round(_StockList.OrderByDescending(x => x.Date).ToList().Take(10).Sum(x => Convert.ToDouble(x.ClosingPrice)) / 10, 2);
				if (_StockList.Count > 20) _Ma20 = Math.Round(_StockList.OrderByDescending(x => x.Date).ToList().Take(20).Sum(x => Convert.ToDouble(x.ClosingPrice)) / 20, 2);
				if (_StockList.Count > 60) _Ma60 = Math.Round(_StockList.OrderByDescending(x => x.Date).ToList().Take(60).Sum(x => Convert.ToDouble(x.ClosingPrice)) / 60, 2);
				this.txtMa5.Text = _Ma5.ToString();
				this.txtMa10.Text = _Ma10.ToString();
				this.txtMa20.Text = _Ma20.ToString();
				this.txtMa60.Text = _Ma60.ToString();

			}
			catch (Exception ex)
			{
				_Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " GetPrice:" + "\r\n" + ex.Message;
				logger.Error(_Log);
			}
		}
		private void getFuturePrice()
		{
			string _Log = "";
			try
			{
				Stock.GetRealtimePriceIn _RealtimePriceIn = new Stock.GetRealtimePriceIn();
				_RealtimePriceIn.Sample1_Symbol = this.txtFutureName.Text;
				List<Stock.GetRealtimeStockPrice> _PriceList = StockPrice.GetRealtimePrice(_RealtimePriceIn);
				if (_PriceList.Count() == 1)
				{
					this.lbFutureStock.Text = _PriceList.First().Price;
				}

				double _Ma5 = 0, _Ma10 = 0, _Ma20 = 0, _Ma60 = 0;
				DataAccess _DataAccess = new DataAccess();
				List<DataModel.Stock.Stock> _StockList = _DataAccess.getStockBySqlList(this.txtFutureName.Text, "Code");
				if (_StockList.Count > 5) _Ma5 = Math.Round(_StockList.OrderByDescending(x => x.Date).ToList().Take(5).Sum(x => Convert.ToDouble(x.ClosingPrice)) / 5, 2);
				if (_StockList.Count > 10) _Ma10 = Math.Round(_StockList.OrderByDescending(x => x.Date).ToList().Take(10).Sum(x => Convert.ToDouble(x.ClosingPrice)) / 10, 2);
				if (_StockList.Count > 20) _Ma20 = Math.Round(_StockList.OrderByDescending(x => x.Date).ToList().Take(20).Sum(x => Convert.ToDouble(x.ClosingPrice)) / 20, 2);
				if (_StockList.Count > 60) _Ma60 = Math.Round(_StockList.OrderByDescending(x => x.Date).ToList().Take(60).Sum(x => Convert.ToDouble(x.ClosingPrice)) / 60, 2);
				this.txtFutureMa5.Text = _Ma5.ToString();
				this.txtFutureMa10.Text = _Ma10.ToString();
				this.txtFutureMa20.Text = _Ma20.ToString();
				this.txtFutureMa60.Text = _Ma60.ToString();

			}
			catch (Exception ex)
			{
				_Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " GetFuturePrice:" + "\r\n" + ex.Message;
				logger.Error(_Log);
			}
		}

		/// <summary>
		/// 每5秒更新股價
		/// 太頻繁跟證交所呼叫API會被封鎖IP
		/// 取得大盤指數
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerRealPrice_Tick(object sender, EventArgs e)
		{
			string _Log = "";
			Stock.GetRealtimePriceIn _RealtimePriceIn = new Stock.GetRealtimePriceIn();
			_RealtimePriceIn.Sample1_Symbol = "t00;";
			double _Exposure = 0.0;
			double _SubTotal = 0.0;
			List<Stock.GetRealtimeStockPrice> _PriceList = new List<Stock.GetRealtimeStockPrice>();
			List<string> _StockList = new List<string>();

			try
			{
				//觀察名單
				foreach (Panel p in flowLayoutPanel1.Controls.Cast<Panel>())
				{
					foreach (Control contr in p.Controls.Cast<Control>())
					{
						if (contr is TextBox)
						{
							if (contr.Tag != null)
								_RealtimePriceIn.Sample1_Symbol = _RealtimePriceIn.Sample1_Symbol + contr.Tag.ToString() + ";";
						}
					}
				}
				//庫存
				foreach (Panel p in flowLayoutPanelStock.Controls.Cast<Panel>())
				{
					foreach (Control contr in p.Controls.Cast<Control>())
					{
						if (contr is TextBox)
						{
							if (contr.Tag != null)
							{
								_RealtimePriceIn.Sample1_Symbol = _RealtimePriceIn.Sample1_Symbol + contr.Tag.ToString() + ";";
								_StockList.Add(contr.Tag.ToString());
							}
						}
					}
				}
				if (_RealtimePriceIn.Sample1_Symbol != null)
				{
					_RealtimePriceIn.Sample1_Symbol = _RealtimePriceIn.Sample1_Symbol.Remove(_RealtimePriceIn.Sample1_Symbol.Length - 1);
					_PriceList = StockPrice.GetRealtimePrice(_RealtimePriceIn);
				}
				List<string> _CodeList = new List<string>();
				List<string> _CodeObserverList = new List<string>();
				_CodeObserverList = _RealtimePriceIn.Sample1_Symbol.Split(';').ToList();
				foreach (Stock.GetRealtimeStockPrice s in _PriceList)
				{
					if (s.StockId == "t00")
					{
						this.lbTaiwanStock.Text = s.Price;
						this.lbGain.Text = Convert.ToString(Math.Round(Convert.ToDouble(s.Price) - Convert.ToDouble(TAIEX), 2));
						if ((Convert.ToDouble(s.Price) - Convert.ToDouble(TAIEX)) > 0) this.lbGain.ForeColor = Color.Red;
						else this.lbGain.ForeColor = Color.Green;
						if (Convert.ToDouble(s.Price) < Convert.ToDouble(this.lbTAIEXMA60.Text))
							this.lbTaiwanStock.ForeColor = Color.Green;
						else if (Convert.ToDouble(s.Price) < Convert.ToDouble(this.lbTAIEXMA20.Text))
							this.lbTaiwanStock.ForeColor = Color.Gold;
						else if (Convert.ToDouble(s.Price) < Convert.ToDouble(this.lbTAIEXMA5.Text))
							this.lbTaiwanStock.ForeColor = Color.Blue;
					}
					Label _LbPrice = new Label();
					Label _LbStockName = new Label();
					Label _LbDealQty = new Label();
					Label _LbTotalDealQty = new Label();
					if (flowLayoutPanel1.Controls.Find("lbPrice" + s.StockId, true).Count() > 0)
					{
						_StockLackOffList.Add(s.StockId);
						_LbPrice = (Label)flowLayoutPanel1.Controls.Find("lbPrice" + s.StockId, true)[0];
						_LbPrice.Text = s.Price;
						_LbStockName = (Label)flowLayoutPanel1.Controls.Find("lbStockName" + s.StockId, true)[0];
						_LbStockName.Text = s.StockName;
						_LbDealQty = (Label)flowLayoutPanel1.Controls.Find("lbDealQty" + s.StockId, true)[0];
						_LbDealQty.Text = s.DealQty;
						_LbTotalDealQty = (Label)flowLayoutPanel1.Controls.Find("lbTotalDealQty" + s.StockId, true)[0];
						_LbTotalDealQty.Text = s.TotalDealQty;
					}
					_CodeList.Add(s.StockId);
				}
				//string _Id = "Price1_lbTPrice";
				//string _Volume = "Price1_lbTVolume";
				string _StockFuture_URL = ConfigurationManager.AppSettings["HiStock_URL"];
				var _LackOffList = _CodeObserverList.Except(_CodeList);
				foreach (string s in _LackOffList)
				{
					Label _LbPrice = new Label();
					Label _LbStockName = new Label();
					Label _LbDealQty = new Label();
					Label _LbTotalDealQty = new Label();
					if (flowLayoutPanel1.Controls.Find("lbPrice" + s, true).Count() > 0)
					{
						_StockLackOffList.Add(s);
						//_LbPrice = (Label)flowLayoutPanel1.Controls.Find("lbPrice" + s, true)[0];
						//_LbPrice.Text = Common.Job.GetTaiwanFutures(_StockFuture_URL + s, _Id, true); 
						_LbStockName = (Label)flowLayoutPanel1.Controls.Find("lbStockName" + s, true)[0];
						if (_StockGroupList.Where(x => x.Code == s).ToList().Count > 0)
							_LbStockName.Text = _StockGroupList.Where(x => x.Code == s).First().Name;
						//_LbTotalDealQty = (Label)flowLayoutPanel1.Controls.Find("lbTotalDealQty" + s, true)[0];
						//_LbTotalDealQty.Text = Common.Job.GetTaiwanFutures(_StockFuture_URL + s, _Volume, true);
					}
				}
				foreach (string sl in _StockList)
				//foreach (Stock.GetRealtimeStockPrice s in _PriceList)
				{
					Label _LbPrice = new Label();
					Label _LbCost = new Label();
					Label _LbStockName = new Label();
					Label _LbPersent = new Label();
					Label _LbProfit = new Label();
					Label _LbStock = new Label();
					Label _LbHigh = new Label();
					Button _BtnDeal = new Button();
					Label _LbDealQty = new Label();
					Label _LbTotalDealQty = new Label();
					Stock.GetRealtimeStockPrice s = _PriceList.Where(x => x.StockId == sl).First();
					{

						if (flowLayoutPanelStock.Controls.Find("lbPrice" + s.StockId, true).Count() > 0)
						{
							_LbPrice = (Label)flowLayoutPanelStock.Controls.Find("lbPrice" + s.StockId, true)[0];
							_LbCost = (Label)flowLayoutPanelStock.Controls.Find("lbCost" + s.StockId, true)[0];
							_LbPersent = (Label)flowLayoutPanelStock.Controls.Find("lbPersent" + s.StockId, true)[0];
							_LbProfit = (Label)flowLayoutPanelStock.Controls.Find("lbProfit" + s.StockId, true)[0];
							_LbStock = (Label)flowLayoutPanelStock.Controls.Find("lbStock" + s.StockId, true)[0];
							_LbHigh = (Label)flowLayoutPanelStock.Controls.Find("lbHigh" + s.StockId, true)[0];
							_BtnDeal = (Button)flowLayoutPanelStock.Controls.Find("btnDeal" + s.StockId, true)[0];
							if (Convert.ToDecimal(s.Price) > Convert.ToDecimal(_LbCost.Text)) _LbPrice.ForeColor = Color.Red;
							else if (Convert.ToDecimal(s.Price) < Convert.ToDecimal(_LbCost.Text)) _LbPrice.ForeColor = Color.Green;
							double _Persent = 0;
							bool _BuySell = false;
							if (_BtnDeal.Text == "現買" || _BtnDeal.Text == "資買") _BuySell = true;
							_Persent = profitMargin(s.Price, _LbCost.Text, _BuySell);
							double _Profit = Convert.ToDouble(_LbCost.Text) * _Persent * 10 * Convert.ToDouble(_LbStock.Text);
							_LbPersent.Text = Convert.ToString(_Persent) + "%";
							_LbPersent.ForeColor = _Persent > 0 ? Color.Red : Color.Green;
							_LbProfit.Text = Convert.ToString(Math.Round(_Profit, 2));
							_LbProfit.ForeColor = _Profit > 0 ? Color.Red : Color.Green;
							_LbPrice.Text = s.Price;
							_LbStockName = (Label)flowLayoutPanelStock.Controls.Find("lbStockName" + s.StockId, true)[0];
							_LbStockName.Text = s.StockName;
							_SubTotal = Convert.ToDouble(_LbPrice.Text) * Convert.ToDouble(_LbStock.Text) * 1000;
							_Exposure = _Exposure + _SubTotal;
							_LbHigh.Text = s.HighPrice != "" ? Math.Round(Convert.ToDouble(s.HighPrice), 2).ToString() : s.HighPrice;

							_LbDealQty = (Label)flowLayoutPanelStock.Controls.Find("lbDealQty" + s.StockId, true)[0];
							_LbDealQty.Text = s.DealQty;
							_LbTotalDealQty = (Label)flowLayoutPanelStock.Controls.Find("lbTotalDealQty" + s.StockId, true)[0];
							_LbTotalDealQty.Text = s.TotalDealQty;
						}
					}



				}

				this.lbExposure.Text = "水位:" + _Exposure.ToString("#,#", CultureInfo.InvariantCulture);
			}
			catch (Exception ex)
			{
				_Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " timerRealPrice:" + "\r\n" + ex.Message;
				logger.Error(_Log);
			}
		}
		private double realTimeFuture(string s, string futurePrice)
		{

			double _Fee = Convert.ToDouble(ConfigurationManager.AppSettings["Fee"]);
			double _Exposure = 0.0;
			double _SubTotal = 0.0;
			Label _LbPrice = new Label();
			Label _LbCost = new Label();
			Label _LbStockName = new Label();
			Label _LbPersent = new Label();
			Label _LbProfit = new Label();
			Label _LbStock = new Label();
			Label _LbHigh = new Label();
			Button _BtnDeal = new Button();
			Label _LbDealQty = new Label();
			Label _LbTotalDealQty = new Label();
			string _EnCode = s.Substring(0, 2);
			double _UnitProfit = _EnCode == "MX" ? 50.0 : 2000.0;

			if (flowLayoutFutures.Controls.Find("lbPrice" + s, true).Count() > 0)
			{
				_LbPrice = (Label)flowLayoutFutures.Controls.Find("lbPrice" + s, true)[0];
				_LbCost = (Label)flowLayoutFutures.Controls.Find("lbCost" + s, true)[0];
				_LbPersent = (Label)flowLayoutFutures.Controls.Find("lbPersent" + s, true)[0];
				_LbProfit = (Label)flowLayoutFutures.Controls.Find("lbProfit" + s, true)[0];
				_LbStock = (Label)flowLayoutFutures.Controls.Find("lbStock" + s, true)[0];
				_BtnDeal = (Button)flowLayoutFutures.Controls.Find("btnDeal" + s, true)[0];
				if (Convert.ToDecimal(futurePrice) > Convert.ToDecimal(_LbCost.Text)) _LbPrice.ForeColor = Color.Red;
				else if (Convert.ToDecimal(futurePrice) < Convert.ToDecimal(_LbCost.Text)) _LbPrice.ForeColor = Color.Green;
				double _Persent = 0;
				bool _BuySell = false;
				if (_BtnDeal.Text == "現買" || _BtnDeal.Text == "資買") _BuySell = true;

				_LbPrice.Text = futurePrice;
				double _Profit = 0.0;
				if (_BuySell)
				{
					_Profit = (Convert.ToDouble(_LbPrice.Text) - Convert.ToDouble(_LbCost.Text)) * _UnitProfit * Convert.ToDouble(_LbStock.Text) - _Fee;
				}
				else
				{
					_Profit = (Convert.ToDouble(_LbCost.Text) - Convert.ToDouble(_LbPrice.Text)) * _UnitProfit * Convert.ToDouble(_LbStock.Text) - _Fee;
				}
				if (_EnCode == "MX")
					_SubTotal = Convert.ToDouble(Margin.MTX) * Convert.ToDouble(_LbStock.Text);
				else
					_SubTotal = Convert.ToDouble(futurePrice) * 270 * Convert.ToDouble(_LbStock.Text);
				_Exposure = _Exposure + _SubTotal;

				_Persent = Math.Round(_Profit / _Exposure, 2);
				_LbPersent.Text = Convert.ToString(_Persent * 100) + "%";
				_LbPersent.ForeColor = _Persent > 0 ? Color.Red : Color.Green;
				_LbProfit.Text = Convert.ToString(Math.Round(_Profit, 2));
				_LbProfit.ForeColor = _Profit > 0 ? Color.Red : Color.Green;
			}
			return _Exposure;
		}
		/// <summary>
		/// 手續:0.001425
		/// 稅金:0.003
		/// </summary>
		/// <param name="price">現價</param>
		/// <param name="cost">成本</param>
		/// <param name="buySell">1:buy /0:sell</param>
		/// <param name="mode">1:stock /0:future</param>
		/// <returns></returns>
		private double profitMargin(string price, string cost, bool buySell)
		{
			double _Persent = 0;
			if (buySell)
			{
				_Persent = Convert.ToDouble(price) / Convert.ToDouble(cost);
			}
			else
			{
				_Persent = Convert.ToDouble(cost) / Convert.ToDouble(price);
			}
			_Persent = (_Persent - 1 - 0.001425 - 0.003) * 100;
			_Persent = Math.Round(_Persent, 2);
			return _Persent;
		}
		/// <summary>
		/// 防守條件單
		/// </summary>
		/// <param name="stockId">股票代號</param>
		/// <param name="stopLostPrice">防守價位</param>
		///  <param name="ladderPiece">分批張數</param>
		///   <param name="ma">MA5/MA10</param>
		private void stopLostOrder(string stockId, string stopLostPrice, string ladderPiece, string ma)
		{
			string _BuySell = "S";
			string _CashMarginShort = "C";
			string _Units = "0";
			string _Price = "0";
			string _StockId = "";
			string _OverSell = "";
			string _OrderType = dicOrderType[this.cbPriceType.SelectedItem.ToString()];
			string _MarketLimit = this.cbPriceType.SelectedItem.ToString() == "市價" ? ",MarketLimit=M" : "";
			string _PriceType = this.cbPriceType.SelectedItem.ToString();
			string _Profit = "";
			ComboBox _CbPriceType = new ComboBox();
			NumericUpDown _NupQty = new NumericUpDown();
			Button _BtnDeal = new Button();
			TextBox _TxtPrice = new TextBox();
			Label _LbPrice = new Label();
			Label _LbProfit = new Label();
			Label _LbStock = new Label();
			_CbPriceType = (ComboBox)flowLayoutPanelStock.Controls.Find("cbPriceType" + stockId, true)[0];
			// _NupQty = (NumericUpDown)flowLayoutPanelStock.Controls.Find("nupQty" + stockId, true)[0];
			_BtnDeal = (Button)flowLayoutPanelStock.Controls.Find("btnDeal" + stockId, true)[0];
			//_TxtPrice = (TextBox)flowLayoutPanelStock.Controls.Find("txtPrice" + stockId, true)[0];
			_LbProfit = (Label)flowLayoutPanelStock.Controls.Find("lbProfit" + stockId, true)[0];
			_LbStock = (Label)flowLayoutPanelStock.Controls.Find("lbStock" + stockId, true)[0];
			_Profit = _LbProfit.Text;
			if (_Profit != "")
			{
				double _Part = Convert.ToDouble(ladderPiece) / Convert.ToDouble(_LbStock.Text);
				_Profit = Convert.ToString(Convert.ToDouble(_Profit) * _Part);
				if (Convert.ToDouble(_Profit) > 0)
				{
					_Profit = ",賺" + _Profit + "元";
				}
				else
				{
					_Profit = ",賠" + _Profit + "元";
				}
			}
			// _Units = Convert.ToString(_NupQty.Value);
			_Units = Convert.ToString(ladderPiece);
			_Price = _CbPriceType.SelectedItem.ToString() == "市價" ? "" : stopLostPrice;
			_BtnDeal.Text = "現賣";
			_OrderType = dicOrderType[_CbPriceType.SelectedItem.ToString()];
			_MarketLimit = _CbPriceType.SelectedItem.ToString() == "市價" ? ",MarketLimit=M" : "";
			_PriceType = _CbPriceType.SelectedItem.ToString();
			_StockId = stockId;
			string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + _StockId + ",BuySell=" + _BuySell + ",Units=" + _Units + ",OrderType=" + _OrderType + ",Price=" + _Price + ",CashMarginShort=" + _CashMarginShort + _OverSell + _MarketLimit;
			StringBuilder sb = new StringBuilder(_Parameter);
			HTSOrder(sb);
			MessageBox.Show(_Parameter);
			string _Msg = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " 你" + "現賣" + _StockId + "共" + _Units + "張," + _PriceType + _Price + "元," + ma + _Profit;
			richTxtInfo.SelectionColor = Color.Green;
			richTxtInfo.AppendText(_Msg + Environment.NewLine);
			richTxtInfo.SelectionStart = richTxtInfo.TextLength;
			richTxtInfo.ScrollToCaret();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="buySell">B/S</param>
		/// <param name="futureId"></param>
		/// <param name="stopLostPrice"></param>
		/// <param name="ladderPiece">分批</param> 
		/// <param name="ma">Pt/AVG/ABS/HIGH</param>
		private void stopLostFuture(string buySell, string status, string futureId, string stopLostPrice, string ladderPiece, string ma)
		{
			string _BuySell = buySell;
			string _Price = "0";
			string _PriceParameter = "0";
			string _FutureOrderType = "M";//停損市價賣出
			string _FokIocRod = _FutureOrderType == "M" ? "F" : "R";
			string _PriceType = this.cbOrderType.SelectedItem.ToString();
			string _OpenCloseAuto = "A";//自動平倉
			string _Profit = "";
			string _StockStatus = "";
			string _Log = "";

			DateTime _DtContractDate = DateTime.Now;
			string _ContractDate = futureId.Length > 8 ? futureId.Substring(3, 6) : futureId.Substring(2, 6);
			string _FutureName = futureId.Length > 8 ? futureId.Substring(0, 3) : futureId.Substring(0, 2); ;
			string _Name = _FutureName;
			if (this._StockFutureCodeList.Where(x => x.EnCode == _FutureName).ToList().Count > 0)
				_Name = this._StockFutureCodeList.Where(x => x.EnCode == _FutureName).First().Name + "期" + _ContractDate.Substring(4, 2);
			ComboBox _CbOrderType = new ComboBox();
			NumericUpDown _NupLots = new NumericUpDown();
			Button _BtnFutureDeal = new Button();
			TextBox _TxtFuturePrice = new TextBox();
			Label _LbPrice = new Label();
			Label _LbProfit = new Label();
			_CbOrderType = (ComboBox)flowLayoutFutures.Controls.Find("cbOrderType" + futureId, true)[0];
			_NupLots = (NumericUpDown)flowLayoutFutures.Controls.Find("nupLots" + futureId, true)[0];
			_BtnFutureDeal = (Button)flowLayoutFutures.Controls.Find("btnDeal" + futureId, true)[0];
			_TxtFuturePrice = (TextBox)flowLayoutFutures.Controls.Find("txtPrice" + futureId, true)[0];
			_LbPrice = (Label)flowLayoutFutures.Controls.Find("lbPrice" + futureId, true)[0];
			_LbProfit = (Label)flowLayoutFutures.Controls.Find("lbProfit" + futureId, true)[0];
			_Price = _TxtFuturePrice.Text != "" ? _TxtFuturePrice.Text : _LbPrice.Text;
			_FutureOrderType = dicFutureOrderType[_CbOrderType.SelectedItem.ToString()];
			_StockStatus = _BtnFutureDeal.Text;
			_BtnFutureDeal.Text = status;
			try
			{
				_Profit = Convert.ToString(Math.Round(Convert.ToDouble(_LbProfit.Text) / Convert.ToDouble(ladderPiece), 2));
				if (_Profit != "")
				{
					if (Convert.ToDouble(_Profit) > 0)
					{
						_Profit = ",賺" + _Profit + "元";
					}
					else
					{
						_Profit = ",賠" + _Profit + "元";
					}
				}


				_PriceParameter = _FutureOrderType == "M" ? "" : _Price;
				string _Parameter = "Market=F,Account=" + txtFutureAccount.Text + ",ContractName=" + _FutureName + ",ContractDate=" + _ContractDate + ",OpenCloseAuto=" + _OpenCloseAuto + ",BuySell=" + _BuySell + ",Lots=" + ladderPiece + ",OrderType=" + _FutureOrderType + ",Price=" + _PriceParameter + ",FokIocRod=" + _FokIocRod + ",DayTrade=N";
				StringBuilder sb = new StringBuilder(_Parameter);

				if ((_Price != "" && _FutureOrderType == "L") || _FutureOrderType == "M")
				{
					HTSOrder(sb);
					// MessageBox.Show(_Parameter);
					string _Msg = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " 你" + status + _Name + "共" + ladderPiece + "口," + _PriceType + _Price + "元" + _Profit + ",停損條件:" + ma + ",價格:" + stopLostPrice;
					richTxtInfo.SelectionColor = status == "現買" ? Color.Red : Color.Green;
					richTxtInfo.AppendText(_Msg + Environment.NewLine);
					richTxtInfo.SelectionStart = richTxtInfo.TextLength;
					richTxtInfo.ScrollToCaret();
					string _Code = _FutureName + _ContractDate;
					bool _ChkPt = _FutureName == "MXF" ? false : true;
					insertUpdateStockInventory(_Code, _Name, status, ladderPiece, _Price, true, _ChkPt);
					this.lbTimes.Text = Convert.ToString(Convert.ToInt32(this.lbTimes.Text) + 1);
					this.btnFutureRefrash.PerformClick();
				}
			}
			catch (Exception ex)
			{
				_Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " stopLostFuture:" + "\r\n" + ex.Message;
				logger.Error(_Log);
			}
		}
		/// <summary>
		/// 將均線停損儲存在設定檔
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSetting_Click(object sender, EventArgs e)
		{
			setStopLostConfig();
			updateStockStopLost();
			Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
			config.AppSettings.Settings["StockAccount"].Value = Tool.Encrypt(this.txtStockAccount.Text, "20220801", "B1050520");
			config.AppSettings.Settings["FutureAccount"].Value = Tool.Encrypt(this.txtFutureAccount.Text, "20220801", "B1050520");
			config.Save(ConfigurationSaveMode.Modified);
			//儲存股票和期貨的停損設定
			this.lbSetting.Text = "設定完成";
		}
		private void updateStockStopLost()
		{
			string _Log = "";
			CheckBox _ChkAll = new CheckBox();
			TextBox _TxtStopLost = new TextBox();
			TextBox _TxtTarget = new TextBox();
			CheckBox _ChkCloseStopLost = new CheckBox();
			CheckBox _ChkCloseABSStopLost = new CheckBox();
			CheckBox _ChkCloseHighStopLost = new CheckBox();
			CheckBox _ChkClosePtLost = new CheckBox();
			try
			{
				foreach (Panel p in flowLayoutPanelStock.Controls.Cast<Panel>())
				{
					foreach (Control contr in p.Controls.Cast<Control>())
					{
						if (contr is TextBox)
						{
							if (contr.Tag != null)
							{
								WebApiService.Models.StockInventory s = new WebApiService.Models.StockInventory();
								s = getStockInventory(Convert.ToString(contr.Tag));
								s.UpdateTime = DateTime.Now.ToString("yyyyMMdd hh:mm:ss");
								_ChkCloseStopLost = (CheckBox)flowLayoutPanelStock.Controls.Find("chkCloseStopLost" + contr.Tag, true)[0];
								_ChkCloseABSStopLost = (CheckBox)flowLayoutPanelStock.Controls.Find("chkCloseABSStopLost" + contr.Tag, true)[0];
								_ChkCloseHighStopLost = (CheckBox)flowLayoutPanelStock.Controls.Find("chkCloseHighStopLost" + contr.Tag, true)[0];
								_ChkAll = (CheckBox)flowLayoutPanelStock.Controls.Find("chkAll" + contr.Tag, true)[0];
								_TxtStopLost = (TextBox)flowLayoutPanelStock.Controls.Find("txtStopLost" + contr.Tag, true)[0];
								_TxtTarget = (TextBox)flowLayoutPanelStock.Controls.Find("txtTarget" + contr.Tag, true)[0];
								s.CloseStopLost = _ChkCloseStopLost.Checked;
								s.CloseABSStopLost = _ChkCloseABSStopLost.Checked;
								s.CloseHighStopLost = _ChkCloseHighStopLost.Checked;
								s.ClosePtLost = false;
								s.SellAll = _ChkAll.Checked;
								s.Defence = _TxtStopLost.Text;
								s.Target = _TxtTarget.Text;
								updateStockInventory(s);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				_Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " updateStockStopLost:" + "\r\n" + ex.Message;
				logger.Error(_Log);
			}
		}
		private void setStopLostConfig()
		{
			string _BullStop = "", _BearStop = "", _PercentStop = "";
			foreach (Control contr in gbBullStop.Controls.Cast<Control>())
			{
				if (contr is RadioButton)
				{
					RadioButton _RaBtn = contr as RadioButton;
					if (_RaBtn.Checked)
						_BullStop = _RaBtn.Text;
				}
			}
			foreach (Control contr in gbBearStop.Controls.Cast<Control>())
			{
				if (contr is RadioButton)
				{
					RadioButton _RaBtn = contr as RadioButton;
					if (_RaBtn.Checked)
						_BearStop = _RaBtn.Text;
				}
			}
			_PercentStop = this.nupPercentStop.Value.ToString();
			Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
			config.AppSettings.Settings["BullStop"].Value = _BullStop;
			config.AppSettings.Settings["BearStop"].Value = _BearStop;
			config.AppSettings.Settings["PercentStop"].Value = _PercentStop;
			config.Save(ConfigurationSaveMode.Modified);


		}
		/// <summary>
		/// 根據設定檔顯示停損均線
		/// </summary>
		private void initialRadioButton()
		{
			this.nupPercentStop.Value = Convert.ToInt32(ConfigurationManager.AppSettings["PercentStop"]);
			string _BullStop = ConfigurationManager.AppSettings["BullStop"].ToString();
			foreach (Control contr in gbBullStop.Controls.Cast<Control>())
			{
				if (contr is RadioButton)
				{
					RadioButton _RaBtn = contr as RadioButton;
					if (contr.Text == _BullStop)
						_RaBtn.Checked = true;
				}
			}
			string _BearStop = ConfigurationManager.AppSettings["BearStop"].ToString();
			foreach (Control contr in gbBearStop.Controls.Cast<Control>())
			{
				if (contr is RadioButton)
				{
					RadioButton _RaBtn = contr as RadioButton;
					if (contr.Text == _BearStop)
						_RaBtn.Checked = true;
				}
			}
		}
		/// <summary>
		/// 根據設定檔動態產生庫存名單 
		/// </summary>

		private void btnReflash_Click(object sender, EventArgs e)
		{
			List<WebApiService.Models.StockInventory> _StockInventoryList = getStockInventoryList();
			string _Log = "";
			int i = 0;
			this.flowLayoutPanelStock.Controls.Clear();
			try
			{
				Button _Btn = new Button();
				//庫存
				foreach (WebApiService.Models.StockInventory s in _StockInventoryList.Where(x => x.Type == false))
				{
					//Panel p = dynamicOberserver(s.Code, Convert.ToDecimal(s.Qty), s.Cost, true, s.Status, false, Convert.ToString(Convert.ToInt32(s.CloseStopLost)), Convert.ToString(Convert.ToInt32(s.CloseABSStopLost)), Convert.ToString(Convert.ToInt32(s.CloseHighStopLost)));
					Panel p = dynamicOberserver(s, true, false);
					flowLayoutPanelStock.Controls.Add(p);
					_Btn = (Button)flowLayoutPanelStock.Controls.Find("btnDeal" + s.Code, true)[0];
					showButton(_Btn, true);
					i++;
				}
			}
			catch (Exception ex)
			{
				_Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " btnReflash:" + "\r\n" + ex.Message;
				logger.Error(_Log);
			}
		}

		private void timerAvgStopLost_Tick(object sender, EventArgs e)
		{
			if (!chkCloseAllStopLost.Checked)
			{
				stopLostStrategy(2, "AVG", false);
			}
		}
		/// <summary>
		/// 停損策略
		/// AVG:日/10日均線停損一半
		/// DEF:跌破防守價停損,漲過目標價賣出
		/// ABS:絕對金額停損,如果部位小於150萬,損失超過兩萬就砍一半
		/// </summary>
		/// <param name="batch">2/3</param>
		/// <param name="mode">AVG/DEF</param>
		/// <param name="allStopLost">1:可全砍</param>
		private void stopLostStrategy(int batch, string mode, bool allStopLost)
		{
			Label _LbPrice = new Label();
			Label _LbStock = new Label();
			Label _LbMa5 = new Label();
			Label _LbMa10 = new Label();
			Label _LbMa20 = new Label(); ;
			TextBox _TxtMa5 = new TextBox();
			TextBox _TxtMa10 = new TextBox();
			TextBox _TxtMa20 = new TextBox();
			Button _BtnDeal = new Button();
			NumericUpDown _NupQty = new NumericUpDown();
			CheckBox _ChkAll = new CheckBox();
			TextBox _TxtStopLost = new TextBox();
			TextBox _TxtTarget = new TextBox();
			CheckBox _ChkCloseStopLost = new CheckBox();
			CheckBox _ChkCloseABSStopLost = new CheckBox();
			CheckBox _ChkCloseHighStopLost = new CheckBox();
			Label _LbProfit = new Label();
			Label _LbCost = new Label();
			Label _LbHigh = new Label();
			string _Log = "";
			try
			{
				foreach (Panel p in flowLayoutPanelStock.Controls.Cast<Panel>())
				{
					foreach (Control contr in p.Controls.Cast<Control>())
					{
						if (contr is TextBox)
						{
							if (contr.Tag != null)
							{
								_TxtMa5 = (TextBox)flowLayoutPanelStock.Controls.Find("txtMa5" + contr.Tag, true)[0];
								_LbPrice = (Label)flowLayoutPanelStock.Controls.Find("lbPrice" + contr.Tag, true)[0];
								if (_TxtMa5.Text != "" && _LbPrice.Text != "")
								{

									_TxtMa10 = (TextBox)flowLayoutPanelStock.Controls.Find("txtMa10" + contr.Tag, true)[0];
									_TxtMa20 = (TextBox)flowLayoutPanelStock.Controls.Find("txtMa20" + contr.Tag, true)[0];
									_NupQty = (NumericUpDown)flowLayoutPanelStock.Controls.Find("nupQty" + contr.Tag, true)[0];
									_LbStock = (Label)flowLayoutPanelStock.Controls.Find("lbStock" + contr.Tag, true)[0];
									_LbMa5 = (Label)flowLayoutPanelStock.Controls.Find("lbMa5" + contr.Tag, true)[0];
									_LbMa10 = (Label)flowLayoutPanelStock.Controls.Find("lbMa10" + contr.Tag, true)[0];
									_LbMa20 = (Label)flowLayoutPanelStock.Controls.Find("lbMa20" + contr.Tag, true)[0];
									_BtnDeal = (Button)flowLayoutPanelStock.Controls.Find("btnDeal" + contr.Tag, true)[0];
									_ChkAll = (CheckBox)flowLayoutPanelStock.Controls.Find("chkAll" + contr.Tag, true)[0];
									_TxtStopLost = (TextBox)flowLayoutPanelStock.Controls.Find("txtStopLost" + contr.Tag, true)[0];
									_TxtTarget = (TextBox)flowLayoutPanelStock.Controls.Find("txtTarget" + contr.Tag, true)[0];
									_ChkCloseStopLost = (CheckBox)flowLayoutPanelStock.Controls.Find("chkCloseStopLost" + contr.Tag, true)[0];
									_ChkCloseABSStopLost = (CheckBox)flowLayoutPanelStock.Controls.Find("chkCloseABSStopLost" + contr.Tag, true)[0];
									_ChkCloseHighStopLost = (CheckBox)flowLayoutPanelStock.Controls.Find("chkCloseHighStopLost" + contr.Tag, true)[0];
									_LbProfit = (Label)flowLayoutPanelStock.Controls.Find("lbProfit" + contr.Tag, true)[0];
									_LbCost = (Label)flowLayoutPanelStock.Controls.Find("lbCost" + contr.Tag, true)[0];
									_LbHigh = (Label)flowLayoutPanelStock.Controls.Find("lbHigh" + contr.Tag, true)[0];
									double _Price = Convert.ToDouble(_LbPrice.Text);
									double _Value = tick(Convert.ToDouble(_LbPrice.Text));
									_Price = Math.Round(_Price - _Value, 2);
									double _LadderPiece = 0;
									if (allStopLost && _ChkAll.Checked) _LadderPiece = Convert.ToDouble(_NupQty.Value);
									else _LadderPiece = Convert.ToDouble(_NupQty.Value) >= batch ? Math.Ceiling(Convert.ToDouble(_NupQty.Value) / batch) : Convert.ToDouble(_NupQty.Value);
									double _Piece = 0;
									string _Status = _BtnDeal.Text;
									double _Exposure = Convert.ToDouble(_LbCost.Text) * Convert.ToDouble(_LbStock.Text) * 1000;
									if (Convert.ToDouble(_LbStock.Text) > 0)
									{
										switch (mode)
										{
											case "AVG":
												if (!_ChkCloseStopLost.Checked)
												{
													if (_Status == "現買" || _Status == "資買")
													{
														_Piece = Convert.ToDouble(_LbStock.Text) > _LadderPiece ? _LadderPiece : Convert.ToDouble(_LbStock.Text);
														if ((Convert.ToDouble(_TxtMa5.Text) > Convert.ToDouble(_LbPrice.Text)) && _LbMa5.ForeColor != Color.Yellow)
														{
															_LbMa5.ForeColor = Color.Yellow;
															stopLostOrder(contr.Tag.ToString(), _Price.ToString(), Convert.ToString(_Piece), _LbMa5.Text);
															_BtnDeal.Text = _Status;
															_LbStock.Text = Convert.ToDouble(_LbStock.Text) - _LadderPiece > 0 ? Convert.ToString(Convert.ToDouble(_LbStock.Text) - _LadderPiece) : "0";
														}

														_Piece = Convert.ToDouble(_LbStock.Text) > _LadderPiece ? _LadderPiece : Convert.ToDouble(_LbStock.Text);
														if ((Convert.ToDouble(_TxtMa10.Text) > Convert.ToDouble(_LbPrice.Text)) && _LbMa10.ForeColor != Color.Yellow)
														{
															_LbMa10.ForeColor = Color.Yellow;
															if (Convert.ToDouble(_LbStock.Text) > 0)
															{
																stopLostOrder(contr.Tag.ToString(), _Price.ToString(), Convert.ToString(_Piece), _LbMa10.Text);
															}
															_BtnDeal.Text = _Status;
															_LbStock.Text = Convert.ToDouble(_LbStock.Text) - _LadderPiece > 0 ? Convert.ToString(Convert.ToDouble(_LbStock.Text) - _LadderPiece) : "0";
														}

													}
													//else if (_Status == "券賣" || _Status == "沖賣")
													//{
													//    _Piece = Convert.ToDouble(_LbStock.Text) > _LadderPiece ? _LadderPiece : Convert.ToDouble(_LbStock.Text);
													//    if ((Convert.ToDouble(_TxtMa5.Text) < Convert.ToDouble(_LbPrice.Text)) && _LbMa5.ForeColor != Color.Blue)
													//    {
													//        _LbMa5.ForeColor = Color.Blue;
													//        stopLostOrder(contr.Tag.ToString(), _Price.ToString(), Convert.ToString(_Piece));
													//        _BtnDeal.Text = _Status;
													//        _LbStock.Text = Convert.ToDouble(_LbStock.Text) - _LadderPiece > 0 ? Convert.ToString(Convert.ToDouble(_LbStock.Text) - _LadderPiece) : "0";
													//    }

													//    _Piece = Convert.ToDouble(_LbStock.Text) > _LadderPiece ? _LadderPiece : Convert.ToDouble(_LbStock.Text);
													//    if ((Convert.ToDouble(_TxtMa10.Text) < Convert.ToDouble(_LbPrice.Text)) && _LbMa10.ForeColor != Color.Blue)
													//    {
													//        _LbMa10.ForeColor = Color.Blue;
													//        if (Convert.ToDouble(_LbStock.Text) > 0)
													//        {
													//            stopLostOrder(contr.Tag.ToString(), _Price.ToString(), Convert.ToString(_Piece));
													//        }
													//        _BtnDeal.Text = _Status;
													//        _LbStock.Text = Convert.ToDouble(_LbStock.Text) - _LadderPiece > 0 ? Convert.ToString(Convert.ToDouble(_LbStock.Text) - _LadderPiece) : "0";
													//    } 
													//}
												}
												if (_Status == "現買" || _Status == "資買")
												{
													System.Media.SoundPlayer sndPlayer = new System.Media.SoundPlayer(Application.StartupPath + @"/Media/WindowsDing.wav");    //wav格式的鈴聲 

													if ((Convert.ToDouble(_TxtMa5.Text) > Convert.ToDouble(_LbPrice.Text)) && _LbMa10.ForeColor != Color.Red)
													{
														if (btnCloseSound.BackColor != Color.Yellow) sndPlayer.PlayLooping();
														_LbMa5.ForeColor = Color.Red;
													}
													if ((Convert.ToDouble(_TxtMa10.Text) > Convert.ToDouble(_LbPrice.Text)) && _LbMa10.ForeColor != Color.Red)
													{
														if (btnCloseSound.BackColor != Color.Yellow) sndPlayer.PlayLooping();
														_LbMa10.ForeColor = Color.Red;
													}
												}
												break;
											case "DEF":

												_Piece = Convert.ToDouble(_LbStock.Text) > _LadderPiece ? _LadderPiece : Convert.ToDouble(_LbStock.Text);
												//現價小於防守價位
												if (_TxtStopLost.Text != "" && _BtnDeal.Text != "現賣")
												{
													if (Convert.ToDecimal(_LbPrice.Text) <= Convert.ToDecimal(_TxtStopLost.Text))
													{
														stopLostOrder(contr.Tag.ToString(), _Price.ToString(), Convert.ToString(_Piece), "防守");
														if (_TxtStopLost.Text != "")
														{
															double _Ladder = Convert.ToDouble(_TxtStopLost.Text) - Convert.ToDouble(_TxtStopLost.Text) * 0.02;
															_Ladder = Math.Round(_Ladder, 2);
															_BtnDeal.Text = "現買";
															_LbStock.Text = Convert.ToDouble(_LbStock.Text) - _LadderPiece > 0 ? Convert.ToString(Convert.ToDouble(_LbStock.Text) - _LadderPiece) : "0";
															_TxtStopLost.Text = _LbStock.Text != "0" ? Convert.ToString(_Ladder) : "";
														}
													}
												}
												//現價大於目標價
												if (_TxtTarget.Text != "" && _TxtTarget.Text.Length >= 2 && _BtnDeal.Text != "現賣")
												{
													if (Convert.ToDecimal(_LbPrice.Text) > Convert.ToDecimal(_TxtTarget.Text))
													{
														stopLostOrder(contr.Tag.ToString(), _Price.ToString(), Convert.ToString(_Piece), "目標");
														if (_TxtTarget.Text != "")
														{
															double _Ladder = Convert.ToDouble(_TxtTarget.Text) + Convert.ToDouble(_TxtTarget.Text) * 0.02;
															_Ladder = Math.Round(_Ladder, 2);
															_BtnDeal.Text = "現買";
															_LbStock.Text = Convert.ToDouble(_LbStock.Text) - _LadderPiece > 0 ? Convert.ToString(Convert.ToDouble(_LbStock.Text) - _LadderPiece) : "0";
															_TxtTarget.Text = _LbStock.Text != "0" ? Convert.ToString(_Ladder) : "";
														}
													}
												}
												break;
											case "ABS":
												if (!_ChkCloseABSStopLost.Checked)
												{
													_Piece = Convert.ToDouble(_LbStock.Text) > _LadderPiece ? _LadderPiece : Convert.ToDouble(_LbStock.Text);
													if (_Status == "現買" || _Status == "資買")
													{
														if (_Exposure < 1500000)
														{
															if (Convert.ToDouble(_LbProfit.Text) < -20000)
															{
																if (Convert.ToDouble(_LbStock.Text) > 0)
																{
																	stopLostOrder(contr.Tag.ToString(), _Price.ToString(), Convert.ToString(_Piece), "絕對金額");
																}
																_BtnDeal.Text = _Status;
																_LbStock.Text = Convert.ToDouble(_LbStock.Text) - _LadderPiece > 0 ? Convert.ToString(Convert.ToDouble(_LbStock.Text) - _LadderPiece) : "0";
																_NupQty.Value = _LbStock.Text != "" ? Convert.ToInt32(_LbStock.Text) : 0;
															}
														}
													}
												}
												break;
											case "HIGH":
												if (!_ChkCloseHighStopLost.Checked)
												{
													_Piece = Convert.ToDouble(_LbStock.Text) > _LadderPiece ? _LadderPiece : Convert.ToDouble(_LbStock.Text);
													if (_Status == "現買" || _Status == "資買")
													{
														double _HighPrice = _LbHigh.Text != "" ? Convert.ToDouble(_LbHigh.Text) : 0;
														double _Persent = Math.Round((_HighPrice / Convert.ToDouble(_LbPrice.Text)) - 1, 4);
														if (_Persent > 0.02)
														{
															if (Convert.ToDouble(_LbStock.Text) > 0)
															{
																stopLostOrder(contr.Tag.ToString(), _Price.ToString(), Convert.ToString(_Piece), "高檔回測超過2%");
																_ChkCloseHighStopLost.Checked = true;
															}
															_BtnDeal.Text = _Status;
															_LbStock.Text = Convert.ToDouble(_LbStock.Text) - _LadderPiece > 0 ? Convert.ToString(Convert.ToDouble(_LbStock.Text) - _LadderPiece) : "0";
															_NupQty.Value = _LbStock.Text != "" ? Convert.ToInt32(_LbStock.Text) : 0;

														}
													}
												}
												break;
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + "stopLostStrategy:" + "\r\n" + ex.Message;
				logger.Error(_Log);

			}
		}
		/// <summary>
		/// 期貨停損策略
		/// </summary>
		/// <param name="batch"></param>
		/// <param name="mode"></param>
		/// <param name="allStopLost"></param>
		private void stopFutureLostStrategy(int batch, string mode, bool allStopLost)
		{
			Label _LbFuturePrice = new Label();
			Label _LbStock = new Label();
			Label _LbMa5 = new Label();
			Label _LbMa10 = new Label();
			Label _LbMa20 = new Label(); ;
			TextBox _TxtMa5 = new TextBox();
			TextBox _TxtMa10 = new TextBox();
			TextBox _TxtMa20 = new TextBox();
			Button _BtnDeal = new Button();
			NumericUpDown _NupLots = new NumericUpDown();
			CheckBox _ChkAll = new CheckBox();
			TextBox _TxtStopLost = new TextBox();
			TextBox _TxtTarget = new TextBox();
			CheckBox _ChkCloseStopLost = new CheckBox();
			CheckBox _ChkCloseABSStopLost = new CheckBox();
			CheckBox _ChkCloseHighStopLost = new CheckBox();
			CheckBox _ChkClosePtStopLost = new CheckBox();
			Label _LbProfit = new Label();
			Label _LbCost = new Label();
			Label _LbHigh = new Label();
			foreach (Panel p in flowLayoutFutures.Controls.Cast<Panel>())
			{
				foreach (Control contr in p.Controls.Cast<Control>())
				{
					if (contr is TextBox)
					{
						if (contr.Tag != null)
						{
							_TxtMa5 = (TextBox)flowLayoutFutures.Controls.Find("txtMa5" + contr.Tag, true)[0];
							_LbFuturePrice = (Label)flowLayoutFutures.Controls.Find("lbPrice" + contr.Tag, true)[0];
							if (_TxtMa5.Text != "" && _LbFuturePrice.Text != "")
							{

								_TxtMa10 = (TextBox)flowLayoutFutures.Controls.Find("txtMa10" + contr.Tag, true)[0];
								_TxtMa20 = (TextBox)flowLayoutFutures.Controls.Find("txtMa20" + contr.Tag, true)[0];
								_NupLots = (NumericUpDown)flowLayoutFutures.Controls.Find("nupLots" + contr.Tag, true)[0];
								_LbStock = (Label)flowLayoutFutures.Controls.Find("lbStock" + contr.Tag, true)[0];
								_LbMa5 = (Label)flowLayoutFutures.Controls.Find("lbMa5" + contr.Tag, true)[0];
								_LbMa10 = (Label)flowLayoutFutures.Controls.Find("lbMa10" + contr.Tag, true)[0];
								_LbMa20 = (Label)flowLayoutFutures.Controls.Find("lbMa20" + contr.Tag, true)[0];
								_BtnDeal = (Button)flowLayoutFutures.Controls.Find("btnDeal" + contr.Tag, true)[0];
								_ChkAll = (CheckBox)flowLayoutFutures.Controls.Find("chkAll" + contr.Tag, true)[0];
								_TxtStopLost = (TextBox)flowLayoutFutures.Controls.Find("txtStopLost" + contr.Tag, true)[0];
								_TxtTarget = (TextBox)flowLayoutFutures.Controls.Find("txtTarget" + contr.Tag, true)[0];
								_ChkCloseABSStopLost = (CheckBox)flowLayoutFutures.Controls.Find("chkCloseABSStopLost" + contr.Tag, true)[0];
								_ChkClosePtStopLost = (CheckBox)flowLayoutFutures.Controls.Find("chkClosePtStopLost" + contr.Tag, true)[0];
								_LbProfit = (Label)flowLayoutFutures.Controls.Find("lbProfit" + contr.Tag, true)[0];
								_LbCost = (Label)flowLayoutFutures.Controls.Find("lbCost" + contr.Tag, true)[0];
								double _Price = Convert.ToDouble(_LbFuturePrice.Text);
								double _Value = tick(Convert.ToDouble(_LbFuturePrice.Text));
								_Price = Math.Round(_Price - _Value, 2);
								double _LadderPiece = 0;
								if (allStopLost && _ChkAll.Checked) _LadderPiece = Convert.ToDouble(_NupLots.Value);
								else _LadderPiece = Convert.ToDouble(_NupLots.Value) >= batch ? Math.Ceiling(Convert.ToDouble(_NupLots.Value) / batch) : Convert.ToDouble(_NupLots.Value);
								double _Piece = 0;
								string _Status = _BtnDeal.Text;
								double _SubTotal = Convert.ToDouble(Margin.MTX) * Convert.ToDouble(_LbStock.Text);
								double _Exposure = _SubTotal;
								string _EnCode = Convert.ToString(contr.Tag).Substring(0, 2);
								int _Length = _EnCode == "MX" ? 5 : 2;
								string _Point = ConfigurationManager.AppSettings["Point"];
								if (Convert.ToDouble(_LbStock.Text) > 0)
								{
									switch (mode)
									{

										case "DEF":

											_Piece = Convert.ToDouble(_LbStock.Text) > _LadderPiece ? _LadderPiece : Convert.ToDouble(_LbStock.Text);
											//放空
											//現價大於防守價位
											double _Ladder = 0.0;
											if (_TxtStopLost.Text != "" && _BtnDeal.Text != "現買" && _TxtStopLost.Text.Length >= _Length)
											{
												if (Convert.ToDecimal(_LbFuturePrice.Text) > Convert.ToDecimal(_TxtStopLost.Text))
												{
													stopLostFuture("B", "現買", contr.Tag.ToString(), _TxtStopLost.Text, Convert.ToString(_Piece), mode);
													if (_EnCode == "MX")
														_Ladder = Convert.ToDouble(_TxtStopLost.Text) + Convert.ToDouble(_Point);
													else
														_Ladder = Convert.ToDouble(_TxtStopLost.Text) + Convert.ToDouble(_TxtStopLost.Text) * 0.02;
													_Ladder = Math.Round(_Ladder, 2);
													_BtnDeal.Text = "現買";
													_LbStock.Text = Convert.ToDouble(_LbStock.Text) - _LadderPiece > 0 ? Convert.ToString(Convert.ToDouble(_LbStock.Text) - _LadderPiece) : "0";
													_TxtStopLost.Text = _LbStock.Text != "0" ? Convert.ToString(_Ladder) : "";
												}
											}
											if (_TxtTarget.Text != "" && _BtnDeal.Text != "現買" && _TxtTarget.Text.Length >= _Length)
											{
												if (Convert.ToDecimal(_LbFuturePrice.Text) < Convert.ToDecimal(_TxtTarget.Text))
												{
													stopLostFuture("B", "現買", contr.Tag.ToString(), _TxtTarget.Text, Convert.ToString(_Piece), mode);
													if (_EnCode == "MX")
														_Ladder = Convert.ToDouble(_TxtTarget.Text) - Convert.ToDouble(_Point);
													else
														_Ladder = Convert.ToDouble(_TxtTarget.Text) - Convert.ToDouble(_TxtTarget.Text) * 0.02;
													_Ladder = Math.Round(_Ladder, 2);
													_BtnDeal.Text = "現買";
													_LbStock.Text = Convert.ToDouble(_LbStock.Text) - _LadderPiece > 0 ? Convert.ToString(Convert.ToDouble(_LbStock.Text) - _LadderPiece) : "0";
													_TxtTarget.Text = _LbStock.Text != "0" ? Convert.ToString(_Ladder) : "";
												}
											}
											if (_TxtStopLost.Text != "" && _BtnDeal.Text != "現賣" && _TxtStopLost.Text.Length >= _Length)
											{
												if (Convert.ToDecimal(_LbFuturePrice.Text) < Convert.ToDecimal(_TxtStopLost.Text))
												{
													stopLostFuture("S", "現賣", contr.Tag.ToString(), _TxtStopLost.Text, Convert.ToString(_Piece), mode);
													if (_EnCode == "MX")
														_Ladder = Convert.ToDouble(_TxtStopLost.Text) - Convert.ToDouble(_Point);
													else
														_Ladder = Convert.ToDouble(_TxtStopLost.Text) - Convert.ToDouble(_TxtStopLost.Text) * 0.02;
													_Ladder = Math.Round(_Ladder, 2);
													_BtnDeal.Text = "現賣";
													_LbStock.Text = Convert.ToDouble(_LbStock.Text) - _LadderPiece > 0 ? Convert.ToString(Convert.ToDouble(_LbStock.Text) - _LadderPiece) : "0";
													_TxtStopLost.Text = _LbStock.Text != "0" ? Convert.ToString(_Ladder) : "";
												}

											}
											if (_TxtTarget.Text != "" && _BtnDeal.Text != "現賣" && _TxtTarget.Text.Length >= _Length)
											{
												if (Convert.ToDecimal(_LbFuturePrice.Text) > Convert.ToDecimal(_TxtTarget.Text))
												{
													stopLostFuture("S", "現賣", contr.Tag.ToString(), _TxtTarget.Text, Convert.ToString(_Piece), mode);
													if (_EnCode == "MX")
														_Ladder = Convert.ToDouble(_TxtTarget.Text) + Convert.ToDouble(_Point);
													else
														_Ladder = Convert.ToDouble(_TxtTarget.Text) + Convert.ToDouble(_TxtTarget.Text) * 0.02;
													_Ladder = Math.Round(_Ladder, 2);
													_BtnDeal.Text = "現賣";
													_LbStock.Text = Convert.ToDouble(_LbStock.Text) - _LadderPiece > 0 ? Convert.ToString(Convert.ToDouble(_LbStock.Text) - _LadderPiece) : "0";
													_TxtTarget.Text = _LbStock.Text != "0" ? Convert.ToString(_Ladder) : "";
												}
											}

											break;
										case "ABS":
											if (!_ChkCloseABSStopLost.Checked)
											{
												_Piece = Convert.ToDouble(_LbStock.Text) > _LadderPiece ? _LadderPiece : Convert.ToDouble(_LbStock.Text);
												string _BuySell = _Status == "現買" ? "S" : "B";
												string status = _Status == "現買" ? "現賣" : "現買";
												if (Convert.ToDouble(_LbProfit.Text) < -20000)
												{
													if (Convert.ToDouble(_LbStock.Text) > 0)
													{
														stopLostFuture(_BuySell, status, contr.Tag.ToString(), _TxtTarget.Text, Convert.ToString(_Piece), mode);
													}
													_BtnDeal.Text = status;
													_LbStock.Text = Convert.ToDouble(_LbStock.Text) - _LadderPiece > 0 ? Convert.ToString(Convert.ToDouble(_LbStock.Text) - _LadderPiece) : "0";
												}
											}
											break;

										case "Pt":
											if (!_ChkClosePtStopLost.Checked)
											{
												_Piece = Convert.ToDouble(_LbStock.Text) > _LadderPiece ? _LadderPiece : Convert.ToDouble(_LbStock.Text);
												if (_Status == "現買")
												{
													if ((Convert.ToDouble(_LbCost.Text) - (Convert.ToDouble(_LbFuturePrice.Text)) > Convert.ToInt32(_Point)))
													{
														stopLostFuture("S", "現賣", contr.Tag.ToString(), "", Convert.ToString(_Piece), mode);
														_LbStock.Text = Convert.ToDouble(_LbStock.Text) - _LadderPiece > 0 ? Convert.ToString(Convert.ToDouble(_LbStock.Text) - _LadderPiece) : "0";
													}
												}
												else if (_Status == "現賣")
												{
													if ((Convert.ToDouble(_LbCost.Text) - (Convert.ToDouble(_LbFuturePrice.Text)) < -Convert.ToInt32(_Point)))
													{
														stopLostFuture("B", "現買", contr.Tag.ToString(), "", Convert.ToString(_Piece), mode);
														_LbStock.Text = Convert.ToDouble(_LbStock.Text) - _LadderPiece > 0 ? Convert.ToString(Convert.ToDouble(_LbStock.Text) - _LadderPiece) : "0";
													}
												}
											}
											break;
									}
								}
							}
						}
					}
				}
			}
		}
		private void timerDefenseStopLost_Tick(object sender, EventArgs e)
		{
			stopLostStrategy(3, "DEF", true);
			stopFutureLostStrategy(2, "DEF", true);
		}
		/// <summary>
		/// 帳戶輸入存於config檔
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void txtStockAccount_TextChanged(object sender, EventArgs e)
		{
			if (this.txtStockAccount.Text.Length > 10)
			{
				Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
				config.AppSettings.Settings["StockAccount"].Value = this.txtStockAccount.Text;
				config.Save(ConfigurationSaveMode.Modified);
			}
		}
		private void getHTSStkInfoQuery(string stockId)
		{

			Label _LbCoupon = new Label();
			//foreach (Panel p in flowLayoutPanel1.Controls.Cast<Panel>())
			//{
			//    foreach (Control contr in p.Controls.Cast<Control>())
			//    {
			//        if (contr is TextBox)
			//        {
			//            if (contr.Tag != null)
			//            {
			string _Parameter = "Market=S,Account=" + txtStockAccount.Text + ",Symbol=" + stockId;
			_LbCoupon = (Label)flowLayoutPanel1.Controls.Find("lbCoupon" + stockId, true)[0];
			StringBuilder sb = new StringBuilder(_Parameter);
			int i = HTSStkInfoQuery(sb, Handle);
			//   MessageBox.Show(contr.Tag.ToString());
			//            }
			//        }
			//    }
			//}            

		}
		protected override void DefWndProc(ref Message m)
		{
			Label _LbStockName = new Label();
			Label _LbCoupon = new Label();
			int WM_COPYDATA = 0x004A;

			if (m.Msg == WM_COPYDATA)
			{
				CopyDataStruct CopyData = new CopyDataStruct();

				CopyData = (CopyDataStruct)m.GetLParam(CopyData.GetType());
				string[] _Coupon = CopyData.lpData.Split(';');
				string[] _Peice = new string[2];
				string[] _StockName = new string[2];
				foreach (string s in _Coupon)
				{
					if (s.IndexOf("融券可用張數") == 1)
					{
						_Peice = s.Split(',');
					}
				}
				foreach (string s in _Coupon)
				{
					if (s.IndexOf("股票名稱") == 1)
					{
						_StockName = s.Split(',');
						foreach (Panel p in flowLayoutPanel1.Controls.Cast<Panel>())
						{
							foreach (Control contr in p.Controls.Cast<Control>())
							{
								if (contr is TextBox)
								{
									if (contr.Tag != null)
									{
										_LbStockName = (Label)flowLayoutPanel1.Controls.Find("lbStockName" + contr.Tag, true)[0];
										_LbCoupon = (Label)flowLayoutPanel1.Controls.Find("lbCoupon" + contr.Tag, true)[0];

										if (_LbStockName.Text == _StockName[1])
										{
											_LbCoupon.Text = _Peice[1];
										}
									}
								}
							}
						}
					}

				}
				int iCopyDataType = CopyData.dwData;
			}

			base.DefWndProc(ref m);
		}
		/// <summary>
		/// 點擊券餘label呼叫日盛API取得數量
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lbCoupon_Click(object sender, EventArgs e)
		{
			Label _lb = sender as Label;
			getHTSStkInfoQuery(_lb.Tag.ToString());
		}

		private void timerAbsStopLost_Tick(object sender, EventArgs e)
		{
			stopLostStrategy(2, "ABS", true);
			stopFutureLostStrategy(2, "ABS", true);
		}

		private void timerHighStopList_Tick(object sender, EventArgs e)
		{
			stopLostStrategy(2, "HIGH", true);
		}
		private bool prohibitOrder()
		{
			bool _Yes = false;
			string _minute = DateTime.Now.Minute.ToString();
			if (DateTime.Now.Minute < 10)
				_minute = "0" + _minute;
			string _time = DateTime.Now.Hour.ToString() + _minute;
			int _time_num = Convert.ToInt32(_time);
			if (_time_num > 0920 || _time_num < 0900)
			{
				_Yes = true;
			}
			return _Yes;

		}

		private void btnBuyFuture_Click(object sender, EventArgs e)
		{
			sendFuturesParameter(sender, false, "B", "現買", Color.Red);
		}
		private void btnBuyFutureStock_Click(object sender, EventArgs e)
		{
			sendFuturesParameter(sender, true, "B", "現買", Color.Red);
		}
		private void btnFutureAdd_Click(object sender, EventArgs e)
		{
			addMinusFuture(sender, false, true);
		}
		private void btnFutureMinus_Click(object sender, EventArgs e)
		{
			addMinusFuture(sender, false, false);
		}
		private void btnSellFuture_Click(object sender, EventArgs e)
		{
			sendFuturesParameter(sender, false, "S", "現賣", Color.Green);
		}
		private void btnSellFutureStock_Click(object sender, EventArgs e)
		{
			sendFuturesParameter(sender, true, "S", "現賣", Color.Green);
		}
		/// <summary>
		/// 取得台指
		/// </summary>
		/// <returns></returns>
		private string getTaiwanFutures()
		{
			string _FuturePrice = "";
			string _Log = "";
			try
			{
				string url = "https://histock.tw/index-tw/FIMTX";
				HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
				//req.Timeout = 30000;    //Timeout 30秒
				WebResponse resp = req.GetResponse();
				string strHtml = string.Empty;
				using (StreamReader reader = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding(65001)))
				{
					strHtml = reader.ReadToEnd();//讀取指定url的HTML
				}
				HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
				htmlDoc.LoadHtml(strHtml);
				_FuturePrice = Math.Floor(Convert.ToDouble(htmlDoc.GetElementbyId("Price1_lbTPrice").InnerText)).ToString();
				//this.txtFuturePrice.Text = htmlDoc.GetElementbyId("Price1_lbTPrice").InnerText; 
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + "getTaiwanFutures:" + "\r\n" + ex.Message;
				logger.Error(_Log);
			}
			return _FuturePrice;
		}
		/// <summary>
		/// 關閉警示音
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCloseSound_Click(object sender, EventArgs e)
		{
			System.Media.SoundPlayer sndPlayer = new System.Media.SoundPlayer(Application.StartupPath + @"/Media/WindowsDing.wav");
			sndPlayer.Stop();
			btnCloseSound.BackColor = Color.Yellow;
		}
		/// <summary>
		/// 取得最新台指價格
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerGetFuture_Tick(object sender, EventArgs e)
		{
			string _Log = "";
			int i = 0;
			try
			{
				string _Id = "Price1_lbTPrice2";
				string _Spread = ConfigurationManager.AppSettings["Spread"];
				string _StockFuture_URL = ConfigurationManager.AppSettings["StockFuture_URL"];
				// string[] _Future = ConfigurationManager.AppSettings["Future"].Split(';');
				List<WebApiService.Models.StockInventory> _StockInventoryList = getStockInventoryList();
				string _FuturePrice = getTaiwanFutures();
				this.lbFuturePrice.Text = _FuturePrice;
				if (this.cbDateMM.SelectedIndex == 1) this.lbFuturePrice.Text = Convert.ToString(Convert.ToInt32(this.lbFuturePrice.Text) - Convert.ToInt32(_Spread));
				Button _Btn = new Button();
				string _MM = DateTime.Now.AddMonths(1).Month.ToString().PadLeft(2, '0');
				double _Exposure = 0.0;
				//庫存
				foreach (WebApiService.Models.StockInventory s in _StockInventoryList.Where(x => x.Type == true))
				{
					if (s.Qty != 0)
					{
						string _EnCode = s.Code.Substring(0, 2);
						string _Code = "";
						string _Price = _FuturePrice;
						if (this._StockFutureCodeList.Where(x => x.EnCode == _EnCode).ToList().Count > 0)
						{
							_Code = this._StockFutureCodeList.Where(x => x.EnCode == _EnCode).First().Code;
							_Price = Common.Job.GetTaiwanFutures(_StockFuture_URL + _Code, _Id, true);
						}

						if (s.Name == "小台期" + _MM) _Price = Convert.ToString(Convert.ToInt32(_FuturePrice) - Convert.ToInt32(_Spread));
						double _SubTotal = realTimeFuture(s.Code, _Price);
						_Exposure += _SubTotal;
						i++;
					}
				}
				this.lbExposureFuture.Text = "水位:" + _Exposure.ToString("#,#", CultureInfo.InvariantCulture);

			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " timerGetFuture:" + "\r\n" + ex.Message;
				logger.Error(_Log);
			}
		}

		private void btnFutureAddStock_Click(object sender, EventArgs e)
		{
			addMinusFuture(sender, true, true);
		}

		private void btnFutureMinusStock_Click(object sender, EventArgs e)
		{
			addMinusFuture(sender, true, false);
		}

		private void timerFutureStopLost_Tick(object sender, EventArgs e)
		{
			if (!chkCloseAllStopLost.Checked)
			{
				//  stopLostStrategy(2, "AVG", false);
			}
		}

		/// <summary>
		/// 指數均價計算
		/// </summary>
		private void getAVG()
		{
			string _Log = "";
			int _Yestoday = -1;
			try
			{
				List<WebApiService.Models.StockIndex> _StockIndexList = _DataAccess.getStockIndexList();
				double _MTX_MA5 = _StockIndexList.Where(x => Convert.ToDouble(x.TX) != 0).OrderByDescending(x => x.Date).Take(5).Sum(x => Convert.ToDouble(x.TX)) / 5;
				double _MTX_MA10 = _StockIndexList.Where(x => Convert.ToDouble(x.TX) != 0).OrderByDescending(x => x.Date).Take(10).Sum(x => Convert.ToDouble(x.TX)) / 10;
				double _MTX_MA20 = _StockIndexList.Where(x => Convert.ToDouble(x.TX) != 0).OrderByDescending(x => x.Date).Take(20).Sum(x => Convert.ToDouble(x.TX)) / 20;
				double _MTX_MA60 = _StockIndexList.Where(x => Convert.ToDouble(x.TX) != 0).OrderByDescending(x => x.Date).Take(60).Sum(x => Convert.ToDouble(x.TX)) / 60;
				double _MA5 = _StockIndexList.Where(x => Convert.ToDouble(x.TX) != 0).OrderByDescending(x => x.Date).Take(5).Sum(x => Convert.ToDouble(x.TAIEX)) / 5;
				double _MA10 = _StockIndexList.Where(x => Convert.ToDouble(x.TX) != 0).OrderByDescending(x => x.Date).Take(10).Sum(x => Convert.ToDouble(x.TAIEX)) / 10;
				double _MA20 = _StockIndexList.Where(x => Convert.ToDouble(x.TX) != 0).OrderByDescending(x => x.Date).Take(20).Sum(x => Convert.ToDouble(x.TAIEX)) / 20;
				double _MA60 = _StockIndexList.Where(x => Convert.ToDouble(x.TX) != 0).OrderByDescending(x => x.Date).Take(60).Sum(x => Convert.ToDouble(x.TAIEX)) / 60;
				this.txtFutureMa5.Text = Math.Ceiling(_MTX_MA5).ToString();
				this.txtFutureMa10.Text = Math.Ceiling(_MTX_MA10).ToString();
				this.txtFutureMa20.Text = Math.Ceiling(_MTX_MA20).ToString();
				this.txtFutureMa60.Text = Math.Ceiling(_MTX_MA60).ToString();
				if (DateTime.Now.DayOfWeek.ToString() == "Monday")
					_Yestoday = -3;
				else if (DateTime.Now.DayOfWeek.ToString() == "Sunday")
					_Yestoday = -2;
				TAIEX = getTopIndex(_Yestoday);
				this.txtMa5.Text = Math.Ceiling(_MA5).ToString();
				this.txtMa10.Text = Math.Ceiling(_MA10).ToString();
				this.txtMa20.Text = Math.Ceiling(_MA20).ToString();
				this.txtMa60.Text = Math.Ceiling(_MA60).ToString();
				this.lbTAIEXMA5.Text = this.txtMa5.Text;
				this.lbTAIEXMA20.Text = this.txtMa20.Text;
				this.lbTAIEXMA60.Text = this.txtMa60.Text;
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " getAVG:" + ex.Message + "\r\n";
				logger.Error(_Log);
				this.richTxtInfo.Text += _Log;
			}

		}
		/// <summary>
		/// 證交所取不到的資料則轉由HiStock取得
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timerGetLackOffPrice_Tick(object sender, EventArgs e)
		{
			string _HiStock_URL = ConfigurationManager.AppSettings["HiStock_URL"];
			foreach (string s in Oberservers)
			{
				Label _LbPrice = new Label();
				Label _LbStockName = new Label();
				Label _LbDealQty = new Label();
				Label _LbTotalDealQty = new Label();
				if (_StockLackOffList.IndexOf(s) < 0)
				{
					if (flowLayoutPanel1.Controls.Find("lbPrice" + s, true).Count() > 0)
					{
						_LbPrice = (Label)flowLayoutPanel1.Controls.Find("lbPrice" + s, true)[0];
						_LbPrice.Text = Common.Job.GetTaiwanFutures(_HiStock_URL + s, "Price1_lbTPrice", true);
						_LbTotalDealQty = (Label)flowLayoutPanel1.Controls.Find("lbTotalDealQty" + s, true)[0];
						_LbTotalDealQty.Text = Common.Job.GetTaiwanFutures(_HiStock_URL + s, "Price1_lbTVolume", true);
					}
				}
			}
		}


		private void timerPtStopLost_Tick(object sender, EventArgs e)
		{
			if (!chkCloseAllStopLost.Checked)
			{
				stopFutureLostStrategy(2, "Pt", false);
			}
		}

		private void btnLine_Click(object sender, EventArgs e)
		{
			if (LineNotify)
			{
				btnLine.BackColor = default(Color);
				LineNotify = false;
			}
			else
			{
				btnLine.BackColor = Color.LightGreen;
				LineNotify = true;
			}
		}
		/// <summary>
		/// 指數LINE通知功能
		/// </summary>
		/// <param name="token">噴起來俱樂部/台指交流群</param>
		/// <param name="ptName">累計上漲/點數</param>
		/// <param name="ptBearName">累計下跌點數</param>
		public async void CallLineNotifyApi(string token, string ptName, string ptBearName, WebApiService.Models.StockLineNotify stockLineNotify)
		{

			string _Log = "";
			try
			{
				int _Pt = Convert.ToInt32(ptName);
				int _PtBear = Convert.ToInt32(ptBearName);
				bool _Open = false;
				int _Yestoday = 0;
				string _Msg = "";
				bool _Mode = DateTime.Now.Hour < 8 || DateTime.Now.Hour > 13 ? true : false;
				string _LineToken = token;
				DataAccess _DataAccesss = new DataAccess();
				string _Tx = getTopTxIndex(_Yestoday, _Mode);
				if (this.lbFuturePrice.Text != "")
				{
					double _Minus = Convert.ToDouble(this.lbFuturePrice.Text) - Convert.ToDouble(_Tx);
					if (_Minus > _Pt)
					{
						_Msg = "已上漲超過" + _Pt + "點,空方請確實停損";
						_Pt = _Pt + 30;
						_PtBear = -30;
						_Open = true;
					}
					if (_Minus < _PtBear)
					{
						_Msg = "已下跌超過" + _PtBear + "點,多方請確實停損";
						_PtBear = _PtBear - 30;
						_Pt = 30;
						_Open = true;
					}
					string _LineMsg = "台指:" + this.lbFuturePrice.Text + "," + _Msg;
					if (_Open)
					{
						await _DataAccess.postLineMsg(_LineMsg, _LineToken);
						stockLineNotify.Point = _Pt;
						stockLineNotify.PointBear = _PtBear;
						_DataAccesss.UpdateStockLineNotify(stockLineNotify);
					}
				}
			}
			catch (Exception ex)
			{
				_Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " CallLineNotifyApi:" + "\r\n" + ex.Message;
				logger.Error(_Log);
			}
		}
		public async void CallLineNotifyApiIndex(string token, string ptName, string ptBearName, WebApiService.Models.StockLineNotify stockLineNotify)
		{

			string _Log = "";
			try
			{
				int _Pt = Convert.ToInt32(ptName);
				int _PtBear = Convert.ToInt32(ptBearName);
				bool _Open = false;
				int _Yestoday = 0;
				if (DateTime.Now.DayOfWeek.ToString() == "Monday")
					_Yestoday = -3;
				else if (DateTime.Now.DayOfWeek.ToString() == "Sunday")
					_Yestoday = -2;
				string _Msg = "";
				bool _Mode = DateTime.Now.Hour > 8 && DateTime.Now.Hour < 14 ? true : false;
				string _LineToken = token;
				DataAccess _DataAccesss = new DataAccess();
				string _Tx = getTopIndex(_Yestoday);
				if (this.lbTaiwanStock.Text != "")
				{
					double _Minus = Convert.ToDouble(this.lbTaiwanStock.Text) - Convert.ToDouble(_Tx);
					if (_Minus > _Pt)
					{
						_Msg = "已上漲超過" + _Pt + "點" + getAlertMsg(_Pt);
						_Pt = _Pt + 30;
						_PtBear = -30;
						_Open = true;
					}
					if (_Minus < _PtBear)
					{
						_Msg = "已下跌超過" + _PtBear + "點" + getAlertMsg(_PtBear);
						_PtBear = _PtBear - 30;
						_Pt = 30;
						_Open = true;
					}
					string _LineMsg = "加權:" + this.lbTaiwanStock.Text + "," + _Msg;
					if (_Open)
					{
						await _DataAccess.postLineMsg(_LineMsg, _LineToken);
						stockLineNotify.Point = _Pt;
						stockLineNotify.PointBear = _PtBear;
						_DataAccesss.UpdateStockLineNotify(stockLineNotify);
					}
				}
			}
			catch (Exception ex)
			{
				_Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " CallLineNotifyApiIndex:" + "\r\n" + ex.Message;
				logger.Error(_Log);
			}
		}
		private string getAlertMsg(int point)
		{
			string _Msg = "";
			if (point <= -50 && point > -100) _Msg = ",抖";
			else if (point <= -100 && point > -150) _Msg = ",怕爆";
			else if (point <= -150 && point > -200) _Msg = ",救命啊";
			else if (point <= -200 && point > -250) _Msg = ",塊陶啊";
			else if (point <= -250) _Msg = ",殺起乃";
			else if (point >= 100 && point < 150) _Msg = ",噴起乃";
			else if (point >= 150 && point < 200) _Msg = ",殺爆空軍";
			else if (point >= 200) _Msg = ",我好興奮啊!我好興奮阿!";
			return _Msg;

		}
		/// <summary>
		/// 取得前一日加權指數,取到為止
		/// </summary>
		/// <param name="day"></param>
		/// <returns></returns>
		private string getTopIndex(int day)
		{
			List<WebApiService.Models.StockIndex> _StockIndexList = _DataAccess.getStockIndexList();
			string _TopIndex = _StockIndexList.Where(x => x.Date == DateTime.Now.AddDays(day).ToString("yyyyMMdd")).First().TAIEX;
			if (_TopIndex == "0")
				return getTopIndex(day - 1);
			return _TopIndex;

		}

		private void timerLineNotify_Tick(object sender, EventArgs e)
		{
			string _DayOfWeek = DateTime.Now.DayOfWeek.ToString();
			DataAccess _DataAccess = new DataAccess();
			List<WebApiService.Models.StockLineNotify> _StockLineNotifyList = _DataAccess.getStockLineNotifyList();
			if (LineNotify)
			{
				foreach (WebApiService.Models.StockLineNotify s in _StockLineNotifyList.Where(x => x.Enable == true).ToList())
				{
					if (_DayOfWeek != "Sunday" && _DayOfWeek != "Saturday")
					{
						if (s.NotifyClass == "TX")
						{
							CallLineNotifyApi(s.Token, s.Point.ToString(), s.PointBear.ToString(), s);
						}
						if (DateTime.Now.Hour > 8 && DateTime.Now.Hour < 14)
						{
							if (s.NotifyClass == "Index")
							{
								CallLineNotifyApiIndex(s.Token, s.Point.ToString(), s.PointBear.ToString(), s);
							}
						}
						if (DateTime.Now.Hour >= 5 && DateTime.Now.Hour < 6)
						{
							btnLine.BackColor = Color.Gray;
							LineNotify = false;
						}
					}
				}
			}
		}

		private void txtFutureName_TextChanged(object sender, EventArgs e)
		{
			nupQty.Value = 1;
			if (txtFutureName.Text.Length == 4)
			{
				getFuturePrice();
			}
			if (txtFutureName.Text == "")
			{
				this.lbFutureStock.Text = this.lbFuturePrice.Text;
				getAVG();
			}
		}

		private void lbStockName_Click(object sender, EventArgs e)
		{
			string _Id = "Price1_lbTPrice";
			string _Volume = "Price1_lbTVolume";
			Label _Lb = sender as Label;
			string _StockFuture_URL = ConfigurationManager.AppSettings["HiStock_URL"];
			Label _LbPrice = new Label();
			Label _LbTotalDealQty = new Label();
			_LbPrice = (Label)flowLayoutPanel1.Controls.Find("lbPrice" + _Lb.Tag, true)[0];
			_LbPrice.Text = Common.Job.GetTaiwanFutures(_StockFuture_URL + _Lb.Tag, _Id, true);
			_LbTotalDealQty = (Label)flowLayoutPanel1.Controls.Find("lbTotalDealQty" + _Lb.Tag, true)[0];
			_LbTotalDealQty.Text = Common.Job.GetTaiwanFutures(_StockFuture_URL + _Lb.Tag, _Volume, true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="day"></param>
		/// <param name="mode">1:夜盤 0:日盤</param>
		/// <returns></returns>
		private string getTopTxIndex(int day, bool mode)
		{
			string _TopIndex = "";

			List<WebApiService.Models.StockIndex> _StockIndexList = _DataAccess.getStockIndexList();
			if (_StockIndexList.Where(x => x.Date == DateTime.Now.AddDays(day).ToString("yyyyMMdd")).ToList().Count > 0)
			{
				if (mode)
					_TopIndex = _StockIndexList.Where(x => x.Date == DateTime.Now.AddDays(day).ToString("yyyyMMdd")).First().TX;
				else
					_TopIndex = _StockIndexList.Where(x => x.Date == DateTime.Now.AddDays(day).ToString("yyyyMMdd")).First().MTX;
			}
			if (_TopIndex == "0" || _TopIndex == "")
				return getTopTxIndex(day - 1, mode);

			return _TopIndex;

		}
		/// <summary>
		/// 取得期貨當前庫存-改資料庫 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnFutureRefrash_Click(object sender, EventArgs e)
		{
			string _Log = "";
			int i = 0;
			List<WebApiService.Models.StockInventory> _StockInventoryList = getStockInventoryList();
			this.flowLayoutFutures.Controls.Clear();
			try
			{
				Button _Btn = new Button();
				//庫存
				foreach (WebApiService.Models.StockInventory s in _StockInventoryList.Where(x => x.Type == true))
				{
					if (s.Qty != 0)
					{
						Panel p = dynamicFutures(s.Code, Convert.ToDecimal(s.Qty), s.Cost, s.Status, s.Name, Convert.ToBoolean(s.ClosePtLost));
						flowLayoutFutures.Controls.Add(p);
						_Btn = (Button)flowLayoutFutures.Controls.Find("btnDeal" + s.Code, true)[0];
						showButton(_Btn, null);
						i++;
					}
				}
			}
			catch (Exception ex)
			{
				_Log = "\r\n" + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + " btnReflash:" + "\r\n" + ex.Message;
				logger.Error(_Log);
			}
		}
	}
}
