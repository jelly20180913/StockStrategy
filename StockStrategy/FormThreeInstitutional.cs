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

namespace StockStrategy
{
	public partial class FormThreeInstitutional : Form
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		public FormThreeInstitutional()
		{
			InitializeComponent();
		}

		private void btnGetData_Click(object sender, EventArgs e)
		{
			string _Log = "";
			try
			{
				string _WhereDate = Convert.ToDateTime(this.dtpDate.Value).ToString("yyyyMMdd");
				DataAccess _DataAccess = new DataAccess();
				List<DataModel.Stock.Stock>  _StockDayAllList = _DataAccess.getStockBySqlList(_WhereDate, "Date");
				List<DataModel.Stock.StockThreeInstitutional> _ListStockThreeInstitutional = new List<DataModel.Stock.StockThreeInstitutional>();
				int _Rank = 1;
				List<DataModel.Stock.Stock> _StockList = new List<DataModel.Stock.Stock>();
				if (cbType.SelectedItem.ToString() == "買超")
				{
					if (cbInstitutional.SelectedItem.ToString() == "外資")
					{
						_StockList = _StockDayAllList.OrderByDescending(x => Convert.ToDecimal(x.ForeignInvestment)).ToList().Take(10).ToList();
					}
					else if (cbInstitutional.SelectedItem.ToString() == "投信") {
						_StockList = _StockDayAllList.OrderByDescending(x => Convert.ToDecimal(x.Investment)).Take(10).ToList();
					}
					else if (cbInstitutional.SelectedItem.ToString() == "自營商")
					{
						_StockList = _StockDayAllList.OrderByDescending(x => Convert.ToDecimal(x.Dealer)).Take(10).ToList();
					}
				}
				else {
					if (cbInstitutional.SelectedItem.ToString() == "外資")
					{
						_StockList = _StockDayAllList.OrderBy(x => Convert.ToDecimal(x.ForeignInvestment)).Take(10).ToList();
					}
					else if (cbInstitutional.SelectedItem.ToString() == "投信")
					{
						_StockList = _StockDayAllList.OrderBy(x => Convert.ToDecimal(x.Investment)).Take(10).ToList();
					}
					else if (cbInstitutional.SelectedItem.ToString() == "自營商")
					{
						_StockList = _StockDayAllList.OrderBy(x => Convert.ToDecimal(x.Dealer)).Take(10).ToList();
					}
				}
				foreach (DataModel.Stock.Stock s in _StockList)
				{
					DataModel.Stock.StockThreeInstitutional _StockThreeInstitutional = new DataModel.Stock.StockThreeInstitutional();
					_StockThreeInstitutional.Code = s.Code;
					_StockThreeInstitutional.Name = s.Name;
					_StockThreeInstitutional.ForeignInvestment = s.ForeignInvestment;
					_StockThreeInstitutional.Investment = s.Investment;
					_StockThreeInstitutional.Dealer = s.Dealer;
					_StockThreeInstitutional.Rank = _Rank;
					_Rank++;
					_ListStockThreeInstitutional.Add(_StockThreeInstitutional);
				}
				SortableBindingList<DataModel.Stock.StockThreeInstitutional> _StockThreeInstitutionalList = new SortableBindingList<DataModel.Stock.StockThreeInstitutional>(_ListStockThreeInstitutional);
				this.dgvThreeInstitutional.DataSource= _StockThreeInstitutionalList;

			}
			catch (Exception ex)
			{
				_Log = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " btnGetData_Click:" + ex.Message + "\r\n";
				logger.Error(_Log);
			}
		}

		private void FormThreeInstitutional_Load(object sender, EventArgs e)
		{
			this.cbType.SelectedIndex = 0;
			this.cbInstitutional.SelectedIndex = 0;
		}
	}
}
