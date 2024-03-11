namespace Ecom.API.Response
{
    public class BaseCommonResponse<T> where T : class
    {
        public BaseCommonResponse()
        {
            
        }
        public BaseCommonResponse(T data,string message=null)
        {
            IsSuccess = true;
            Message = message;
            Data = data;
        }

        public BaseCommonResponse(T data, int statusCode, string message = null)
        {
            IsSuccess = true;
            Data = data;
            StatusCode = statusCode;
            Message = message ?? DefaultMessageForStatusCode(statusCode);
        }
        public BaseCommonResponse(int statusCode, string message = null)
        {
            IsSuccess = false;
            StatusCode = statusCode;
            Message = message ?? DefaultMessageForStatusCode(statusCode);
        }
       
        public BaseCommonResponse(string message)
        {
            IsSuccess = false;
            Message = message;
        }

        private string DefaultMessageForStatusCode(int statusCode)
        {
            string message = statusCode switch
            {
                400 => "bad request",
                401=>"not authorize",
                404=>"resourse not found",
                500=>"server error",
                _=>null
            };
            return message;
           
        }
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }

      //public IEnumerable<string> Errors { get; set; }  // no need to add this property since it is exist in ApiValidationErrorResponse class
    }
}
