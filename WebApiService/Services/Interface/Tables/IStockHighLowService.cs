using System.Collections.Generic;
using WebApiService.Models;
namespace WebApiService.Services.Interface.Tables
{
    public interface IStockHighLowService
    {
        string Create(StockHighLow instance);

        void Update(StockHighLow instance);

        void Delete(int Id);

        bool IsExists(int Id);

        StockHighLow GetByID(int Id);

        IEnumerable<StockHighLow> GetAll();
        List<string> MiltiCreate(List<StockHighLow> instance);
    }
}