using System.Collections.Generic;
using WebApiService.Models;
namespace WebApiService.Services.Interface.Tables
{
    public interface IStockEpsService
    {
        string Create(StockEps instance);

        void Update(StockEps instance);

        void Delete(int Id);

        bool IsExists(int Id);

        StockEps GetByID(int Id);

        IEnumerable<StockEps> GetAll();
        List<string> MiltiCreate(List<StockEps> instance);
        IEnumerable<StockEps> Filter(string sql); 

	}
}