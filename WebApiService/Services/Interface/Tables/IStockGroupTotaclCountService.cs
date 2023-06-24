using System.Collections.Generic;
using WebApiService.Models;
namespace WebApiService.Services.Interface.Tables
{
    public interface IStockGroupTotalCountService
    {
        string Create(StockGroupTotalCount instance);

        void Update(StockGroupTotalCount instance);

        void Delete(int Id);

        bool IsExists(int Id);

        StockGroupTotalCount GetByID(int Id);

        IEnumerable<StockGroupTotalCount> GetAll();
        List<string> MiltiCreate(List<StockGroupTotalCount> instance);
    }
}