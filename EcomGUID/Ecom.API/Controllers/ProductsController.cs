using Ecom.Core.Interfaces;
using Ecom.Infrasctructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _uOW;

        public ProductsController(IUnitOfWork uOW)
        {
            _uOW = uOW;
        }
        [HttpGet("GetProductById")]
        public async Task<ActionResult>Get(int id)
        {
           var res=await _uOW.ProductRepository.GetById(id);

            return Ok(res);

        }
    }
}
