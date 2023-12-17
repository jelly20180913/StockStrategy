using System.Collections.Generic;
using WebApiService.Models;
namespace WebApiService.Services.Interface.Tables
{
    public interface IStockRevenueService
    {
        string Create(StockRevenue instance);

        void Update(StockRevenue instance);

        void Delete(int Id);

        bool IsExists(int Id);

        StockRevenue GetByID(int Id);

        IEnumerable<StockRevenue> GetAll();
        List<string> MiltiCreate(List<StockRevenue> instance);
        IEnumerable<StockRevenue> Filter(string sql); 

	}
}