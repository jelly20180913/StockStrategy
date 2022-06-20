using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Formatting;
using WebApiService.Filters;
namespace WebApiService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.AddQueryStringMapping("$format", "json", "application/json");
            //error action filter
             config.Filters.Add(new AllExceptionResponse());
            config.Filters.Add(new ApiResultAttribute());
            //add LogHandle 
            //config.MessageHandlers.Add(new LogHandler());
            // Web API 設定和服務

            // cross domain need
            config.EnableCors();
            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional } 
            );
            //use DI framwork  "Unity" 
            UnityConfig.RegisterComponents();
            config.Formatters.XmlFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data"));
        }
    }
}
