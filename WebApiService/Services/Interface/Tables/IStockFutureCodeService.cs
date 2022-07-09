using System.Collections.Generic;
using WebApiService.Models;
namespace WebApiService.Services.Interface.Tables
{
    public interface IStockLineNotifyService
    {
        string Create(StockLineNotify instance);

        void Update(StockLineNotify instance);

        void Delete(int Id);

        bool IsExists(int Id);

        StockLineNotify GetByID(int Id);

        IEnumerable<StockLineNotify> GetAll();
        List<string> MiltiCreate(List<StockLineNotify> instance);
    }
}