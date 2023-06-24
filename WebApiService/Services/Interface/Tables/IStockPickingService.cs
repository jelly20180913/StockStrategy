using System.Collections.Generic;
using WebApiService.Models;
namespace WebApiService.Services.Interface.Tables
{
    public interface IStockPickingService
    {
        string Create(StockPicking instance);

        void Update(StockPicking instance);

        void Delete(int Id);

        bool IsExists(int Id);

        StockPicking GetByID(int Id);

        IEnumerable<StockPicking> GetAll();
        List<string> MiltiCreate(List<StockPicking> instance);
    }
}