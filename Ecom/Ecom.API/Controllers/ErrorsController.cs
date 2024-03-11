using Ecom.API.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{

    //Note this API or end point it will hit when the user try to call an API not exisit in our project 
    [Route("errors/{statusCode}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi =true)]
    public class ErrorsController : ControllerBase
    {
        [HttpGet()]
        public IActionResult Error(int statusCode)
        {
           // return Ok("Hiiii");
            return new ObjectResult(new BaseCommonResponse<object>(statusCode));
        }
    }
}
