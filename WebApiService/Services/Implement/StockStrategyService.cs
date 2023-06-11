using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using WebApiService.Models;
using WebApiService.Services.Implement.Tables;
using WebApiService.Services.Interface;
using WebApiService.Services.Interface.Tables;

namespace WebApiService.Services.Implement
{
	public class StockStrategyService: IStockStrategyService
	{
		private IStockService _stockService;
		private IHolidayService _holidayService;
		private IStockPickingService _stockPickingService;
		private IStockResultService _stockResultService;
		/// <summary>
		/// Dependence Injection
		/// </summary>

		public StockStrategyService(IStockService stockService, IHolidayService holidayService, IStockPickingService stockPickingService,IStockResultService stockResultService)
		{
			this._stockService = stockService;
			_holidayService = holidayService;
			_stockPickingService = stockPickingService;	
			_stockResultService=stockResultService;
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
		public List<Holiday> GetHolidayList( )
		{
			List<Holiday> _HolidayList = new List<Holiday>();
			_HolidayList = this._holidayService.GetAll().ToList();
			return _HolidayList;
		}
		public bool InsertStockPicking(List<StockPicking> s)
		{
			this._stockPickingService.MiltiCreate(s);
			return true;
		}
		public bool InsertStockResult(List<StockResult> s)
		{
			this._stockResultService.MiltiCreate(s);
			return true;
		}
		public List<StockPicking> GetStockPickingList()
		{
			List<StockPicking> _StockPickingList = new List<StockPicking>();
			_StockPickingList = this._stockPickingService.GetAll().ToList();
			return _StockPickingList;
		}
		public List<StockResult> GetStockResultList()
		{
			List<StockResult> _StockResultList = new List<StockResult>();
			_StockResultList = this._stockResultService.GetAll().ToList();
			return _StockResultList;
		}
		public bool UpdateStockPicking(StockPicking s)
		{
			this._stockPickingService.Update(s);
			return true;
		}
		public bool DeleteStockPicking(int id)
		{
			this._stockPickingService.Delete(id);
			return true;
		}
		public bool DeleteStockResult(int id)
		{
			this._stockResultService.Delete(id);
			return true;
		}
	}
}
