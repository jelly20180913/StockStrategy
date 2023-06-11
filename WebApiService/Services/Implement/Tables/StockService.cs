using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Policy;
using WebApiService.Models;
using WebApiService.Models.Repository;
using WebApiService.Services.Interface.Tables;
namespace WebApiService.Services.Implement.Tables
{
	public class StockService : IStockService
	{
		private IRepository<Stock> _repository;

		public StockService(IRepository<Stock> repository)
		{
			this._repository = repository;
		}
		public string Create(Stock instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException();
			}
			return this._repository.Create(instance);
		}

		public void Update(Stock instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException();
			}
			this._repository.Update(instance);
		}

		public void Delete(int Id)
		{
			var instance = this.GetByID(Id);
			this._repository.Delete(instance);
		}

		public bool IsExists(int Id)
		{
			return this._repository.GetAll().Any(x => x.Id == Id);
		}

		public Stock GetByID(int Id)
		{
			return this._repository.Get(x => x.Id == Id);
		}

		public IEnumerable<Stock> GetAll()
		{
			return this._repository.GetAll();
		}
		public List<string> MiltiCreate(List<Stock> instance)
		{
			List<string> _ListError = new List<string>();
			_ListError = this._repository.CreateBatch(instance);
			return _ListError;
		}
		public IEnumerable<Stock> Filter(string sql)
		{
			return this._repository.Filter(sql);
		}
		/// <summary>
		/// 效能問題
		/// 1. DbSet抽出寫成Linq撈取時就先限制條件
		/// 2. 只取得所需欄位,減少傳輸成本
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public IEnumerable<DataModel.Stock.Stock> GetByDate(string parameter)
		{
			DbSet<Stock> _StockDbSet = this._repository.GetDbSet();
			var query = from c in _StockDbSet
						where c.Date == parameter
						select new
						{
							Code = c.Code,
							Name = c.Name,
							TradeVolume = c.TradeVolume,
							ClosingPrice = c.ClosingPrice,
							Gain = c.Gain,
							Shock = c.Shock,
							Date = c.Date,
							HighestPrice = c.HighestPrice,
							OpeningPrice=c.OpeningPrice

						}; 
			IEnumerable<DataModel.Stock.Stock> _StockList = from p in query.AsEnumerable()
															select new DataModel.Stock.Stock(
			    p.Code,
				p.Name,
				p.TradeVolume,
			    p.HighestPrice,
				p.ClosingPrice,
				p.Date,
				p.Gain,
				p.Shock,
				p.OpeningPrice
			 ) ;
			return _StockList;
		}
		public IEnumerable<DataModel.Stock.Stock> GetByCode(string parameter)
		{
			DbSet<Stock> _StockDbSet = this._repository.GetDbSet();
			var query = from c in _StockDbSet
						where c.Code == parameter
						select new
						{
							Code = c.Code,
							Name = c.Name,
							TradeVolume = c.TradeVolume,
							ClosingPrice = c.ClosingPrice,
							Gain = c.Gain,
							Shock = c.Shock,
							Date = c.Date,
							HighestPrice = c.HighestPrice,
							OpeningPrice = c.OpeningPrice
						};
			IEnumerable<DataModel.Stock.Stock> _StockList = from p in query.AsEnumerable()
															select new DataModel.Stock.Stock(
				p.Code,
				p.Name,
				p.TradeVolume,
				p.HighestPrice,
				p.ClosingPrice,
				p.Date,
				p.Gain,
				p.Shock,
				p.OpeningPrice
			 ); 
			return _StockList;
		}
		/// <summary>
		/// 撈一個月的資料
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public IEnumerable<DataModel.Stock.Stock> GetByPredays(string parameter)
		{
			DbSet<Stock> _StockDbSet = this._repository.GetDbSet();
			int _EndDate = Convert.ToInt32(parameter);
			
			DateTime _DtStart = DateTime.ParseExact(_EndDate.ToString(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces).AddMonths(-1); 
			int _StartDate =Convert.ToInt32( _DtStart.ToString("yyyyMMdd"));
			//AsEnumerable() 不然轉型會報錯
			var query = from c in _StockDbSet.AsEnumerable()
						where Convert.ToInt32(c.Date) <= _EndDate && Convert.ToInt32(c.Date) > _StartDate
						select new
						{
							Code = c.Code,
							Name = c.Name,
							TradeVolume = c.TradeVolume,
							ClosingPrice = c.ClosingPrice,
							Gain = c.Gain,
							Shock = c.Shock,
							Date = c.Date,
							HighestPrice = c.HighestPrice,
							OpeningPrice = c.OpeningPrice
						};
			IEnumerable<DataModel.Stock.Stock> _StockList = from p in query.AsEnumerable()
															select new DataModel.Stock.Stock(
				p.Code,
				p.Name,
				p.TradeVolume,
				p.HighestPrice,
				p.ClosingPrice,
				p.Date,
				p.Gain,
				p.Shock,
				p.OpeningPrice
			 );
			return _StockList;
		}
	}
}