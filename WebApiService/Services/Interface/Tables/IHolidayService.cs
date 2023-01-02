using System.Collections.Generic;
using WebApiService.Models;
namespace WebApiService.Services.Interface.Tables
{
    public interface IHolidayService
    {
        string Create(Holiday instance);

        void Update(Holiday instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Holiday GetByID(int Id);

        IEnumerable<Holiday> GetAll();
        List<string> MiltiCreate(List<Holiday> instance);
        IEnumerable<Holiday> Filter(string sql); 

	}
}