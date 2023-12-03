using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Stock
{
	public class StockReport
	{
		[Description("型態")]
		public string StockClass { get; set; }
		[Description("選股日")]
		public string StartDate { get; set; }
		[Description("股票代碼")]
		public string Code { get; set; }
		[Description("股票名稱")]
		public string Name { get; set; }
		[Description("選股收盤價")]
		public string StartPrice { get; set; }
		[Description("今日價")]
		public string TodayPrice { get; set; }
		[Description("總漲幅")]
		public decimal TotalGain { get; set; }
		[Description("報表日收盤價")]
		public string ClosingPrice { get; set; }
		[Description("報表日漲幅")]
		public decimal ClosingGain { get; set; }
		[Description("初量")]
		public string StartVolumn { get; set; }
		[Description("昨日量")]
		public string YesterdayVolumn { get; set; }
		
		[Description("漲幅")]
		public decimal AccumulatedGain { get; set; }
		[Description("獲利")]
		public decimal  GainProfit { get; set; }
		[Description("備註")]
		public string Remark { get; set; }
		
		
		[Description("核放")]
		public string Approve { get; set; }
		[Description("隔日開盤價")]
		public string SecondDayOpeningPrice { get; set; }
		[Description("Key")]
		public string StockPickingId { get; set; }
		[Description("分數")]
		public int Point { get; set; }
	}
}
