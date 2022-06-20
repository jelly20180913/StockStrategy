using System.Collections.Generic;
using WebApiService.Models;
namespace WebApiService.Services.Interface.Tables
{
    public interface IStockIndexService
    {
        string Create(StockIndex instance);

        void Update(StockIndex instance);

        void Delete(int Id);

        bool IsExists(int Id);

        StockIndex GetByID(int Id);

        IEnumerable<StockIndex> GetAll();
        List<string> MiltiCreate(List<StockIndex> instance);
    }
}