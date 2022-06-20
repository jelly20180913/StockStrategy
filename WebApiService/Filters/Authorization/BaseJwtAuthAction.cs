using System;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Jose;
using DataModel;
namespace WebApiService.Filters.Authorization
{
    public class BaseJwtAuthAction : ActionFilterAttribute
    {
        /// <summary>
        /// use class name as secret key
        /// </summary>
        private string secret;

        public string Secret
        {
            get
            {
                return secret;
            }

            set
            {
                secret = value;
            }
        }
        /// <summary>
        /// 1. if your header has no authorization or scheme not is Bearer ,return error message "UNAUTHORIZATION"
        /// 2. if your header has no parameter ,return error message "NOPARAMETER"
        /// 3. if your signature is wrong will exception to return 
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null || actionContext.Request.Headers.Authorization.Scheme != "Bearer")
            {
                throw new Exception("UNAUTHORIZATION");
            }
            else if (actionContext.Request.Headers.Authorization.Parameter == null)
            {
                throw new Exception("NOPARAMETER");
            }
            else if (actionContext.Request.Headers.Authorization.Parameter == "null")
            {
                throw new Exception("NOPARAMETER");
            }
            else
            {
                //if you send error signature, in this side you got the exception
                var jwtObject = Jose.JWT.Decode<JwtAuthObject>(
                    actionContext.Request.Headers.Authorization.Parameter,
                    Encoding.UTF8.GetBytes(Secret),
                    JwsAlgorithm.HS256);
            }
            base.OnActionExecuting(actionContext);
        }
    }
}