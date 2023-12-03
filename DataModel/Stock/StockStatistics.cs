using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Stock
{
	public class StockStatistics
	{
		[Description("型態")] 
		public string StockClass { get; set; }
		[Description("漲")]
		public string Increase { get; set; }
		[Description("跌")]
		public string Decrease { get; set; }
		[Description("總獲利")]
		public string TotalProfit { get; set; }
		[Description("總成本")]
		public string TotalCost { get; set; }
		[Description("報酬率")]
		public string Profit { get; set; }
		[Description("分數")]
		public decimal Point { get; set; }
		//[Description("贏")]
		//public string Win { get; set; }
		//[Description("輸")]
		//public string Lose { get; set; }
	}
}
