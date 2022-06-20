using DataModel.Login;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApiService.Services.Interface;
namespace WebApiService.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TokenController : ApiController
    {
        private ITokenService _tokenService;

        public TokenController(ITokenService tokenService)
        {
            this._tokenService = tokenService;
        }
        // POST api/values
        /// <summary>
        /// first use this webapi you have to call this method
        /// </summary>
        /// <param name="loginData">login object</param>
        /// <param name="Username">Username</param>
        /// <param name="Password">Password</param>
        /// <param name="Origin">Owner/Customer</param>
        /// <returns></returns>
        public object Post( LoginData loginData)
        {
            return this._tokenService.SetToken(loginData);
        }
        public string Get(int id)
        {
            return "ok";
        }
    }
}
