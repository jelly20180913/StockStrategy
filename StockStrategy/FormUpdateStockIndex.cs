using StockStrategy.BBL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using WebApiService.Models;

namespace StockStrategy
{
	public partial class FormUpdateStockIndex : Form
	{
		DataAccess _DataAccess = new DataAccess();
		public FormUpdateStockIndex()
		{
			InitializeComponent();
		}

		private void btnUpdate_Click(object sender, EventArgs e)
		{
			string _Log = "";
			try
			{ 
				StockIndex s = new StockIndex();
				List<StockIndex> _ListStock = _DataAccess.getStockIndexList();
				string _Date = this.dtpStockIndex.Value.ToString() != "" ?Convert.ToDateTime( this.dtpStockIndex.Value ).Date.ToString("yyyyMMdd"): DateTime.Now.Date.ToString("yyyyMMdd");
				s = _ListStock.OrderByDescending(x =>x.Date== _Date).First();  
				s.TX_High =this.txtHigh.Text != "" ? this.txtHigh.Text : s.TX_High;
				s.TX_Open = this.txtOpen.Text != "" ? this.txtOpen.Text : s.TX_Open;
				s.TX_Low = this.txtLow.Text != "" ? this.txtLow.Text : s.TX_Low;
				s.C10YearBond_Index = this.txtBondIndex.Text != "" ? this.txtBondIndex.Text : s.C10YearBond_Index;
				s.C10YearBond_IndexQuotePercent = this.txtBondIndexPercent.Text != "" ? this.txtBondIndexPercent.Text : s.C10YearBond_IndexQuotePercent; 
				_DataAccess.UpdateStockIndex(s);
				_Log = DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + " update stock index ok.";
				MessageBox.Show(_Log);
			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " btnUpdate_Click:" + $"訊息:{ex.Message}|行號{ex.StackTrace}" + "\r\n";
				 
			}
		}
	}
}
