using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Stock
{
	public class StockRobinForcast
	{
		[Description("日期")]
		public string Date { get; set; }
		[Description("準")]
		public bool IsRight { get; set; }
		[Description("幾點")] 
		public decimal Point { get; set; }
		[Description("預測結果")]
		public bool IsBull { get; set; }
		[Description("開盤價")]
		public string TX_Open { get; set; }
		[Description("最低價")]
		public string TX_Low { get; set; }
		[Description("最高價")]
		public string TX_High { get; set; }
		[Description("收盤價")]
		public string TX { get; set; }
	}
}
