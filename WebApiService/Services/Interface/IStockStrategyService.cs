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
		bool InsertStockPicking(List<StockPicking> s);
		bool InsertStockResult(List<StockResult> s);
		List<StockPicking> GetStockPickingList();
		bool UpdateStockPicking(StockPicking s);
		bool DeleteStockPicking(int id);
		List<StockResult> GetStockResultList();
		bool DeleteStockResult(int id);
		List<StockHighLow> GetStockHighLowList();
		bool InsertStockHighLow(List<StockHighLow> s);
		bool InsertStockGroupTrend(List<StockGroupTrend> s);
		List<StockGroupTrend> GetStockGroupTrendList();
		bool InsertStockThreeInstitutional(List<StockThreeInstitutional> s);
		List<StockThreeInstitutional> GetStockThreeInstitutionalList();
		List<StockGroupTotalCount> GetStockGroupTotalCountList();
	}
}
