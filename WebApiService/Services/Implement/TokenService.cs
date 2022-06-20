using System; 
using Jose;
using System.Text;  
using WebApiService.Services.Interface.Tables;
using WebApiService.Services.Interface;
using DataModel.Login;
using WebApiService.Models;
using DataModel;
namespace WebApiService.Services.Implement
{
    public class TokenService : ITokenService
    {
        private ILoginService _loginService;
        public TokenService(ILoginService loginService)
        {
            this._loginService = loginService;
        }
        // POST api/values
        /// <summary>
        /// p.s the expiration not work
        /// </summary>
        /// <param name="loginData"></param>
        /// <returns></returns>
        public object SetToken(LoginData loginData)
        { 
            var secret = "StockJwtAuthAction";
            // if ( loginData.Username ==_Token.Username && loginData.Password == _Token.Password ) 
            Login _Login = _loginService.GetByAccount(loginData.Username, loginData.Password);
            if (_Login != null)
            {
                //every day has diffrent token
                var payload = new JwtAuthObject()
                {
                    Id = _Login.Id,
                    exp = DateTime.Now.Ticks,
                    iat = DateTime.Now.AddSeconds(10).Ticks
                };
                return new
                {
                    token = Jose.JWT.Encode(payload, Encoding.UTF8.GetBytes(secret), JwsAlgorithm.HS256),
                    loginId = _Login.Id 
                };
            }
            else
            {
                throw new UnauthorizedAccessException("IDPASSWORDERROR:ID:" + loginData.Username + " PWD:" + loginData.Password);
            }
        } 
    }
}