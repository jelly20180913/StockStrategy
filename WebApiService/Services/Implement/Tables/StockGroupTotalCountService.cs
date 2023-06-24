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
	public class StockGroupTotalCountService : IStockGroupTotalCountService
	{
		private IRepository<StockGroupTotalCount> _repository;

		public StockGroupTotalCountService(IRepository<StockGroupTotalCount> repository)
		{
			this._repository = repository;
		}
		public string Create(StockGroupTotalCount instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException();
			}
			return this._repository.Create(instance);
		}

		public void Update(StockGroupTotalCount instance)
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

		public StockGroupTotalCount GetByID(int Id)
		{
			return this._repository.Get(x => x.Id == Id);
		}

		public IEnumerable<StockGroupTotalCount> GetAll()
		{
			return this._repository.GetAll();
		}
		public List<string> MiltiCreate(List<StockGroupTotalCount> instance)
		{
			List<string> _ListError = new List<string>();
			_ListError = this._repository.CreateBatch(instance);
			return _ListError;
		}
		public IEnumerable<StockGroupTotalCount> Filter(string sql)
		{
			return this._repository.Filter(sql);
		}
	}
}