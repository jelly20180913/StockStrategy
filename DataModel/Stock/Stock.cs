using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Stock
{
	public class Stock
	{
		 
		public string Code { get; set; }
		public string Name { get; set; }
		public string TradeVolume { get; set; }  
		public string HighestPrice { get; set; } 
		public string ClosingPrice { get; set; }  
		public string Date { get; set; } 
		public string Gain { get; set; }
		public string Shock { get; set; }
		public string OpeningPrice { get; set; }
		public string ForeignInvestment { get; set; }
		public string Investment { get; set; }
		public string Dealer { get; set; }
		public Stock(string code,string name,string tradeVolume,string highestPrice,string closingPrice,string date,string gain,string shock,string openingPrice,string foreignInvestment,string investment,string dealer )
		{
			Code = code;
			Name = name;
			TradeVolume = tradeVolume;
			HighestPrice = highestPrice;
			ClosingPrice = closingPrice;
			Date = date;
			Gain = gain;
			Shock = shock;
			OpeningPrice = openingPrice;
			ForeignInvestment = foreignInvestment;
			Investment = investment;
			Dealer = dealer;
			// doing something with CampaignID here like setting some calculated properties.
		}
		 
	}
	
}
