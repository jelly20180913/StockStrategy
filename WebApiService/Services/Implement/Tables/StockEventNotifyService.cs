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
	public class StockEventNotifyService : IStockEventNotifyService
	{
		private IRepository<StockEventNotify> _repository;

		public StockEventNotifyService(IRepository<StockEventNotify> repository)
		{
			this._repository = repository;
		}
		public string Create(StockEventNotify instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException();
			}
			return this._repository.Create(instance);
		}

		public void Update(StockEventNotify instance)
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

		public StockEventNotify GetByID(int Id)
		{
			return this._repository.Get(x => x.Id == Id);
		}

		public IEnumerable<StockEventNotify> GetAll()
		{
			return this._repository.GetAll();
		}
		public List<string> MiltiCreate(List<StockEventNotify> instance)
		{
			List<string> _ListError = new List<string>();
			_ListError = this._repository.CreateBatch(instance);
			return _ListError;
		}
		public IEnumerable<StockEventNotify> Filter(string sql)
		{
			return this._repository.Filter(sql);
		}
	}
}