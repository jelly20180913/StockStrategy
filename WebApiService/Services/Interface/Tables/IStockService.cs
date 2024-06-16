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
        IEnumerable<DataModel.Stock.Stock> GetByDate(string parameter);
        IEnumerable<DataModel.Stock.Stock> GetByCode(string parameter);
		IEnumerable<DataModel.Stock.Stock> GetByPredays(string parameter);
        IEnumerable<DataModel.Stock.Stock> GetByBetweenDate(string dtStart, string dtEnd);
        IEnumerable<Stock> GetByStockCode(string parameter);
        IEnumerable<Stock> GetByStockDate(string parameter);

	}
}