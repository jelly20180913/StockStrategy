using System.Collections.Generic;
using WebApiService.Models;
namespace WebApiService.Services.Interface.Tables
{
    public interface IStockService
    {
        string Create(Stock instance);

        void Update(Stock instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Stock GetByID(int Id);

        IEnumerable<Stock> GetAll();
        List<string> MiltiCreate(List<Stock> instance);
        IEnumerable<Stock> Filter(string sql);
    }
}