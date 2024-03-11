using Ecom.API.Response;

namespace Ecom.API.Errors
{
    public class ApiValidationErrorResponse<T>:BaseCommonResponse<T> where T : class
    {
        public ApiValidationErrorResponse():base(400)
        {
            
        }
        public IEnumerable<string> Errors { get; set; }
    }
    
}
