using System.Collections.Generic;
using WebApiService.Models;
namespace WebApiService.Services.Interface.Tables
{
    public interface IStockEventNotifyService
    {
        string Create(StockEventNotify instance);

        void Update(StockEventNotify instance);

        void Delete(int Id);

        bool IsExists(int Id);

        StockEventNotify GetByID(int Id);

        IEnumerable<StockEventNotify> GetAll();
        List<string> MiltiCreate(List<StockEventNotify> instance);
    }
}