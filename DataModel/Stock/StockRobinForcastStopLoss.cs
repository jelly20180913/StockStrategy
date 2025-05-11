using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Stock
{
	public class StockRobinForcastStopLoss
	{
		[Description("停損策略")]
		public string StopLossPoint { get; set; }
		[Description("準")]
		public string IsRight { get; set; }
		[Description("不準")]
		public string IsFail { get; set; }
		[Description("幾點")]
		public string Point { get; set; }
		[Description("準確率")]
		public string ForcastRate { get; set; }
	}
}
