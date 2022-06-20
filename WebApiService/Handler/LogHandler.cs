using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
using System.Net.Http;
using System.ServiceModel.Channels;

namespace WebApiService.Handler
{
    public class LogHandler : DelegatingHandler
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        //REF: http://blog.kkbruce.net/2012/05/aspnet-web-api-8-http-http-message.html
        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            System.Threading.CancellationToken cancellationToken)
        {
            //取得Method、IP、Url等等，稍後寫入NLog
            string method = request.Method.ToString();
            //REF: http://bit.ly/16lpGKM
            //when Self-Hosted mode，get IP by RemoteEndpointMessageProperty 
            string ip = "Unknown";
            if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
                ip = ((RemoteEndpointMessageProperty)
                    request.Properties[RemoteEndpointMessageProperty.Name]).Address;
            string url = request.RequestUri.ToString();

            return base.SendAsync(request, cancellationToken)
                .ContinueWith((task) =>
                {
                    //when response insert NLog
                    HttpResponseMessage resp = task.Result as HttpResponseMessage;
                    logger.Log(LogLevel.Trace, string.Format("{0} {1} {2} {3}",
                        ip, method, url, (int)resp.StatusCode));
                    return resp;
                });
        }
    }
}