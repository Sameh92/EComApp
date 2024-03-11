using Ecom.API.Response;

namespace Ecom.API.Errors
{
    public class ApiException<T>:BaseCommonResponse<T> where T : class
    {
        public ApiException(int statusCode, string message = null,string details=null):base(statusCode,message)
        {
            Details = details;
        }
        public string Details { get; set; }
    }
}
