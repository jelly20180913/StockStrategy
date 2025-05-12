using System.Collections.Generic;
using WebApiService.Models;
namespace WebApiService.Services.Interface.Tables
{
    public interface IStockIndexStopLossLogService
	{
        string Create(StockIndexStopLossLog instance);

        void Update(StockIndexStopLossLog instance);

        void Delete(int Id);

        bool IsExists(int Id);

        StockIndexStopLossLog GetByID(int Id);

        IEnumerable<StockIndexStopLossLog> GetAll();
        List<string> MiltiCreate(List<StockIndexStopLossLog> instance);
        IEnumerable<StockIndexStopLossLog> Filter(string sql); 

	}
}