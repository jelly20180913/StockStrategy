using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Stock
{
	public class StockHighLow
	{
		[Description("股票代碼")]
		public string Code { get; set; }
		[Description("股票名稱")]
		public string Name { get; set; }
		[Description("目前價格")] 
		public decimal CurrentPrice { get; set; }
		[Description("最高價")]
		public decimal HighestPrice { get; set; }
		[Description("最低價")]
		public decimal LowestPrice { get; set; }
		[Description("最高價日")]
		public string HighestDate { get; set; }
		[Description("最低價日")]
		public string LowestDate { get; set; }
		[Description("幾倍")]
		public decimal HighLowRate { get; set; }
		[Description("創高")]
		public bool NewRecord { get; set; }
		[Description("產業類別")]
		public string Class { get; set; }
		[Description("更新時間")]
		public string UpdateTime { get; set; }
	}
}
