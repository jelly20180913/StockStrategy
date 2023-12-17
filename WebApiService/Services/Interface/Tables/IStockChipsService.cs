using System.Collections.Generic;
using WebApiService.Models;
namespace WebApiService.Services.Interface.Tables
{
    public interface IStockChipsService
    {
        string Create(StockChips instance);

        void Update(StockChips instance);

        void Delete(int Id);

        bool IsExists(int Id);

        StockChips GetByID(int Id);

        IEnumerable<StockChips> GetAll();
        List<string> MiltiCreate(List<StockChips> instance);
        IEnumerable<StockChips> Filter(string sql); 

	}
}