using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Login;
namespace WebApiService.Services.Interface
{
  public  interface ITokenService
    {
        object SetToken(LoginData loginData);
    }
}
