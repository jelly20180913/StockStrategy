 
using System.Web.Http;

namespace WebApiService.Controllers
{
    public class IndexController : ApiController
    {
        public string Get(int id)
        { 
            return "ok";
        }
    }
}
