using System.Collections.Generic;
using WebApiService.Models;
namespace WebApiService.Services.Interface.Tables
{
    public interface IStockGroupService
    {
        string Create(StockGroup instance);

        void Update(StockGroup instance);

        void Delete(int Id);

        bool IsExists(int Id);

        StockGroup GetByID(int Id);

        IEnumerable<StockGroup> GetAll();
        List<string> MiltiCreate(List<StockGroup> instance);

    }
}
