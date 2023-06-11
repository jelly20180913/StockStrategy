using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockStrategy.Model
{
	 
	public class GoodStock
	{
		public string Name { get; set; }
		public decimal Gain { get; set; }
		public int Volumn { get; set; }
		public int Multiple { get; set; }
		public int PreDays { get; set; }
		public int LessVolumn { get; set; }
		public int PreDays2 { get; set; }
		public int MoreGain { get; set; }
		public bool ChkLess { get; set; }
		public bool ChkNotHigherThan { get; set; }
		public int Class { get; set; }
	}
}
