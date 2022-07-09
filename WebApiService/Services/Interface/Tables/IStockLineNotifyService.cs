using System.Collections.Generic;
using WebApiService.Models;
namespace WebApiService.Services.Interface.Tables
{
    public interface IStockFutureCodeService
    {
        string Create(StockFutureCode instance);

        void Update(StockFutureCode instance);

        void Delete(int Id);

        bool IsExists(int Id);

        StockFutureCode GetByID(int Id);

        IEnumerable<StockFutureCode> GetAll();
        List<string> MiltiCreate(List<StockFutureCode> instance);
    }
}