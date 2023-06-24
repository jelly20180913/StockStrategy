using System.Collections.Generic;
using WebApiService.Models;
namespace WebApiService.Services.Interface.Tables
{
    public interface IStockThreeInstitutionalService
    {
        string Create(StockThreeInstitutional instance);

        void Update(StockThreeInstitutional instance);

        void Delete(int Id);

        bool IsExists(int Id);

        StockThreeInstitutional GetByID(int Id);

        IEnumerable<StockThreeInstitutional> GetAll();
        List<string> MiltiCreate(List<StockThreeInstitutional> instance);
    }
}