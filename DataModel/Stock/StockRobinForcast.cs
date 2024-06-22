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
	}
}
