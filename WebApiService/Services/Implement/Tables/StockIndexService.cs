using System;
using System.Collections.Generic;
using System.Linq;
using WebApiService.Models;
using WebApiService.Models.Repository;
using WebApiService.Services.Interface.Tables;
namespace WebApiService.Services.Implement.Tables
{
    public class StockIndexService:IStockIndexService
    {
        private IRepository<StockIndex> _repository;

        public StockIndexService(IRepository<StockIndex> repository)
        {
            this._repository = repository;
        }
        public string Create(StockIndex instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            return this._repository.Create(instance);
        }

        public void Update(StockIndex instance)
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

        public StockIndex GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }

        public IEnumerable<StockIndex> GetAll()
        {
            return this._repository.GetAll();
        }
        public List<string> MiltiCreate(List<StockIndex> instance)
        {
            List<string> _ListError = new List<string>();
            _ListError = this._repository.CreateBatch(instance);
            return _ListError;
        }
    }
}