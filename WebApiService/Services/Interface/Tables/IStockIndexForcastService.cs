using System.Collections.Generic;
using WebApiService.Models;
namespace WebApiService.Services.Interface.Tables
{
    public interface IStockIndexForcastService
    {
        string Create(StockIndexForcast instance);

        void Update(StockIndexForcast instance);

        void Delete(int Id);

        bool IsExists(int Id);

		StockIndexForcast GetByID(int Id);

        IEnumerable<StockIndexForcast> GetAll();
        List<string> MiltiCreate(List<StockIndexForcast> instance);
        IEnumerable<StockIndexForcast> Filter(string sql); 

	}
}