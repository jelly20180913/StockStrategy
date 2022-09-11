using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiService.Models;
using WebApiService.Services.Implement.Tables;
using WebApiService.Services.Interface;
using WebApiService.Services.Interface.Tables;

namespace WebApiService.Services.Implement
{
	public class StockStrategyService: IStockStrategyService
	{
		private IStockService _stockService; 
		/// <summary>
		/// Dependence Injection
		/// </summary>

		public StockStrategyService(IStockService stockService)
		{
			this._stockService = stockService; 
		}
		/// <summary>
		/// get vote item list
		/// </summary>
		/// <param name="className"></param>
		/// <returns></returns>
		public List<DataModel.Stock.Stock> GetStockList(string parameter, string mode)
		{
			List<DataModel.Stock.Stock> _StockList = new List<DataModel.Stock.Stock>();
			if (mode == "Code") _StockList = this._stockService.GetByCode(parameter).ToList();
			else if(mode == "Date") _StockList = this._stockService.GetByDate(parameter).ToList();
			else if (mode == "Predays") _StockList = this._stockService.GetByPredays(parameter).ToList(); //取前N日會有六日問題
			return _StockList;
		}
		public bool InsertStock(List<Stock> s)
		{
			this._stockService.MiltiCreate(s);
			return true;
		}
		public bool UpdateStock(Stock s)
		{
			this._stockService.Update(s);
			return true;
		}
	}
}
