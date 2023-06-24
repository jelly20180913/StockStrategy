using System.Collections.Generic;
using WebApiService.Models;
namespace WebApiService.Services.Interface.Tables
{
    public interface IStockBacktestService
    {
        string Create(StockBacktest instance);

        void Update(StockBacktest instance);

        void Delete(int Id);

        bool IsExists(int Id);

        StockBacktest GetByID(int Id);

        IEnumerable<StockBacktest> GetAll();
        List<string> MiltiCreate(List<StockBacktest> instance);
    }
}