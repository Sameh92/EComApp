using Ecom.API.Response;
using Ecom.Core.Entities;
using Ecom.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BugsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("not-found")]
        public ActionResult GetNotFound()
        {
            var product = _context.Products.Find(50);
            if (product is null)
            {
                return NotFound(new BaseCommonResponse<Product>(404));
            }
            return Ok(product);
        }

        [HttpGet("bad-request")]
        public ActionResult GetBadRequet()
        {
            return BadRequest(new BaseCommonResponse<Product>(400));

        }
        [HttpGet("server-error")] //using middleware
        public ActionResult GetServerError()
        {
            var product = _context.Products.Find(50);
            product.Name = "Error";
            return Ok();
        }
        [HttpGet("bad-request/{id}")] // here if I pass to id string instead of integer I wil get bad request
        public ActionResult GetNotFoundRequet(int id)
        {
            var product = _context.Products.Find(id);
            return Ok(product);
        }

        // the end point not found using errors controller
    }
}
