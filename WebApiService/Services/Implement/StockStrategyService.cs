﻿using System;
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
		private IStockHighLowService _stockHighLowService;
		private IStockGroupTrendService _stockGroupTrendService;
		private IStockThreeInstitutionalService _stockThreeInstitutionalService;
		private IStockGroupTotalCountService _stockGroupTotalCountService;
		private IStockEventNotifyService _stockEventNotifyService;
		private IStockRevenueService _stockRevenueService;
		private IStockChipsService _stockChipsService;
		private IStockEpsService _stockEpsService;
		/// <summary>
		/// Dependence Injection
		/// </summary>

		public StockStrategyService(IStockService stockService, IHolidayService holidayService, IStockPickingService stockPickingService,IStockResultService stockResultService,IStockHighLowService stockHighLowService
			, IStockGroupTrendService stockGroupTrendService, IStockThreeInstitutionalService stockThreeInstitutionalService, IStockGroupTotalCountService stockGroupTotalCountService,IStockEventNotifyService stockEventNotifyService
			,IStockRevenueService stockRevenueService,IStockChipsService stockChipsService,IStockEpsService stockEpsService)
		{
			this._stockService = stockService;
			_holidayService = holidayService;
			_stockPickingService = stockPickingService;	
			_stockResultService=stockResultService;
			_stockHighLowService=stockHighLowService;
			_stockGroupTrendService=stockGroupTrendService;
			_stockThreeInstitutionalService=stockThreeInstitutionalService;
			_stockGroupTotalCountService=stockGroupTotalCountService;
			_stockEventNotifyService=stockEventNotifyService;
			_stockRevenueService = stockRevenueService;
			_stockChipsService = stockChipsService;
			_stockEpsService = stockEpsService;
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
			else if (mode == "Date") _StockList = this._stockService.GetByDate(parameter).ToList();
			else if (mode == "Predays") _StockList = this._stockService.GetByPredays(parameter).ToList(); //取前N日會有六日問題
			else if (mode == "BetweenDate")
			{
				string[] _Date = parameter.Split('~');
				_StockList = this._stockService.GetByBetweenDate(_Date[0], _Date[1]).ToList();
			}
			return _StockList;
		}
		public List< Stock> GetByStockCodeList(string parameter, string mode)
		{
			List< Stock> _StockList = new List< Stock>();
			if (mode == "Code") _StockList = this._stockService.GetByStockCode(parameter).ToList(); 
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
		public bool InsertStockHighLow(List<StockHighLow> s)
		{
			this._stockHighLowService.MiltiCreate(s);
			return true;
		}
		public List<StockHighLow> GetStockHighLowList()
		{
			List<StockHighLow> _StockHighLowList = new List<StockHighLow>();
			_StockHighLowList = this._stockHighLowService.GetAll().ToList();
			return _StockHighLowList;
		}
		public bool InsertStockGroupTrend(List<StockGroupTrend> s)
		{
			this._stockGroupTrendService.MiltiCreate(s);
			return true;
		}
		public List<StockGroupTrend> GetStockGroupTrendList()
		{
			List<StockGroupTrend> _StockGroupTrendList = new List<StockGroupTrend>();
			_StockGroupTrendList = this._stockGroupTrendService.GetAll().ToList();
			return _StockGroupTrendList;
		}
		public bool InsertStockThreeInstitutional(List<StockThreeInstitutional> s)
		{
			this._stockThreeInstitutionalService.MiltiCreate(s);
			return true;
		}
		public List<StockThreeInstitutional> GetStockThreeInstitutionalList()
		{
			List<StockThreeInstitutional> _StockThreeInstitutionalList = new List<StockThreeInstitutional>();
			_StockThreeInstitutionalList = this._stockThreeInstitutionalService.GetAll().ToList();
			return _StockThreeInstitutionalList;
		}
		public List<StockGroupTotalCount> GetStockGroupTotalCountList()
		{
			List<StockGroupTotalCount> _StockGroupTotalCountList = new List<StockGroupTotalCount>();
			_StockGroupTotalCountList = this._stockGroupTotalCountService.GetAll().ToList();
			return _StockGroupTotalCountList;
		}

		public bool InsertStockEventNotify(List<StockEventNotify> s)
		{
			this._stockEventNotifyService.MiltiCreate(s);
			return true;
		}
		public List<StockEventNotify> GetStockEventNotifyList()
		{
			List<StockEventNotify> _StockEventNotifyList = new List<StockEventNotify>();
			_StockEventNotifyList = this._stockEventNotifyService.GetAll().ToList();
			return _StockEventNotifyList;
		}
		public bool InsertStockRevenue(List<StockRevenue> s)
		{
			this._stockRevenueService.MiltiCreate(s);
			return true;
		}
		public bool InsertStockEps(List<StockEps> s)
		{
			this._stockEpsService.MiltiCreate(s);
			return true;
		}
		public bool InsertStockChips(List<StockChips> s)
		{
			this._stockChipsService.MiltiCreate(s);
			return true;
		}
	}
}
