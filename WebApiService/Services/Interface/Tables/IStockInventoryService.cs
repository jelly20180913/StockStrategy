using System.Collections.Generic;
using WebApiService.Models;
namespace WebApiService.Services.Interface.Tables
{
    public interface IStockInventoryService
    {
        string Create(StockInventory instance);

        void Update(StockInventory instance);

        void Delete(int Id);

        bool IsExists(int Id);

        StockInventory GetByID(int Id);

        IEnumerable<StockInventory> GetAll();
        List<string> MiltiCreate(List<StockInventory> instance);
    }
}