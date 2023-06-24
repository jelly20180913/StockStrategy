using System.Collections.Generic;
using WebApiService.Models;
namespace WebApiService.Services.Interface.Tables
{
    public interface IStockGroupTrendService
    {
        string Create(StockGroupTrend instance);

        void Update(StockGroupTrend instance);

        void Delete(int Id);

        bool IsExists(int Id);

        StockGroupTrend GetByID(int Id);

        IEnumerable<StockGroupTrend> GetAll();
        List<string> MiltiCreate(List<StockGroupTrend> instance);
    }
}