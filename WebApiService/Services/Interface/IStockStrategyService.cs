using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiService.Models;

namespace WebApiService.Services.Interface
{
	public interface IStockStrategyService
	{
		List<DataModel.Stock.Stock> GetStockList(string parameter, string mode);
		bool InsertStock(List<Stock> s);
		bool UpdateStock(Stock s);
	    List<Holiday> GetHolidayList();
	}
}
