using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using NLog;
using WebApiService.DataModel;
namespace WebApiService.Filters
{
    public class AllExceptionResponse : ExceptionFilterAttribute
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// every exception will entry this method
        /// and will output log file at App_Data\logs
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(System.Web.Http.Filters.HttpActionExecutedContext actionExecutedContext)
        {
            base.OnException(actionExecutedContext);
            logger.Log(LogLevel.Error, actionExecutedContext.Exception);
            var errorMessage = actionExecutedContext.Exception.Message;
            var result = new ApiResultEntity()
            {
                StatusCode = HttpStatusCode.OK,
                Status = "ERROR",
                ErrorMessage = errorMessage
            };
            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(result.StatusCode, result);
        }
    }
}