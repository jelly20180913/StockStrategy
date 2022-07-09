using System;
using System.Collections.Generic;
using System.Linq;
using WebApiService.Models;
using WebApiService.Models.Repository;
using WebApiService.Services.Interface.Tables;
namespace WebApiService.Services.Implement.Tables
{
    public class StockLineNotifyService:IStockLineNotifyService
    {
        private IRepository<StockLineNotify> _repository;

        public StockLineNotifyService(IRepository<StockLineNotify> repository)
        {
            this._repository = repository;
        }
        public string Create(StockLineNotify instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            return this._repository.Create(instance);
        }

        public void Update(StockLineNotify instance)
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

        public StockLineNotify GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }

        public IEnumerable<StockLineNotify> GetAll()
        {
            return this._repository.GetAll();
        }
        public List<string> MiltiCreate(List<StockLineNotify> instance)
        {
            List<string> _ListError = new List<string>();
            _ListError = this._repository.CreateBatch(instance);
            return _ListError;
        }
    }
}