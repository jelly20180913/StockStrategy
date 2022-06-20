using System.Web.Http.Filters;
using System.Net.Http;
using WebApiService.DataModel;
namespace WebApiService.Filters
{
    public class ApiResultAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        /// <summary>
        /// Action Executed package output format
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            {
                // exclude exception action 
                if (actionExecutedContext.Exception != null)
                    return;
                // exclude download controller
                //if (actionExecutedContext.Request.RequestUri.ToString().IndexOf("download") > 0 || actionExecutedContext.Request.RequestUri.ToString().IndexOf("DownloadPos") > 0 || actionExecutedContext.Request.RequestUri.ToString().IndexOf("DownloadScheduleOrder") > 0)
                //    return;
                base.OnActionExecuted(actionExecutedContext);
                ApiResultEntity result = new ApiResultEntity();
                result.Status = "OK";
                result.StatusCode = actionExecutedContext.ActionContext.Response.StatusCode;
                if (actionExecutedContext.ActionContext.Response.Content != null)
                    result.Data = actionExecutedContext.ActionContext.Response.Content.ReadAsAsync<object>().Result;
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(result.StatusCode, result);

            }
        }
    }
}