using System.Net;

namespace WebApiService.DataModel
{
    public class ApiResultEntity
    {
        private HttpStatusCode statusCode { get; set; }

        public HttpStatusCode StatusCode
        {
            get
            {
                return statusCode;
            }

            set
            {
                statusCode = value;
            }
        }

        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }

            set
            {
                errorMessage = value;
            }
        }

        private string status;
        private string errorMessage;
        public object Data { get; set; }
    }
}