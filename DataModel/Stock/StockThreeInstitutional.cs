using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Stock
{
	public class StockThreeInstitutional
	{
		[Description("股票代碼")]
		public string Code { get; set; }
        [Description("股票名稱")]
		public string Name { get; set; }
		[Description("外資")]
		public string ForeignInvestment { get; set; }
		[Description("投信")]
		public string Investment { get; set; }
		[Description("自營商")]
		public string Dealer { get; set; }
		[Description("排名")]
		public Nullable<int> Rank { get; set; } 
	}
}
