 
using System.Web.Http;
using System.Web.Http.Cors;
namespace WebApiService.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class IndexController : ApiController
    {
        public string Get(int id)
        { 
            return "ok";
        }
    }
}
