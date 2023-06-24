using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Stock
{
	public class StockGroupTrend
	{
		[Description("股票類別")]
		public string GroupName { get; set; }
		[Description("日期")]
		public string Date { get; set; }
		[Description("強勢/弱勢")]
		public string StrengthType { get; set; }
		[Description("強弱比")]
		public decimal StrengthPercent { get; set; }
		[Description("紅燈")]
		public string RedLight { get; set; }
		[Description("綠燈")]
		public string GreenLight { get; set; }
		[Description("類股")]
		public string Remark { get; set; }
	}
}
