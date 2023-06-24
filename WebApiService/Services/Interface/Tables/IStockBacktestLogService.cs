using System.Collections.Generic;
using WebApiService.Models;
namespace WebApiService.Services.Interface.Tables
{
    public interface IStockResultService
    {
        string Create(StockResult instance);

        void Update(StockResult instance);

        void Delete(int Id);

        bool IsExists(int Id);

        StockResult GetByID(int Id);

        IEnumerable<StockResult> GetAll();
        List<string> MiltiCreate(List<StockResult> instance);
    }
}