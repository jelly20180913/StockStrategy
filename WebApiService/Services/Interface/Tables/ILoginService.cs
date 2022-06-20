using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiService.Models;
namespace WebApiService.Services.Interface.Tables
{
   public interface ILoginService
    {
        void Create(Login instance);

        void Update(Login instance);

        void Delete(int Id);

        bool IsExists(int Id);

        Login GetByID(int Id);

        IEnumerable<Login> GetAll();

        bool VerifyAccount(string account, string password);

        Login GetByAccount(string account, string password);
    }
}
