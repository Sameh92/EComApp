using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly IUnitOfWork _uOW;

        public ProfilesController(IUnitOfWork uOW)
        {
            _uOW = uOW;
        }
        [HttpGet("GetProfileById")]
        public async Task<ActionResult> Get(Guid id)
        {
            var res = await _uOW.ProfileRepository.GetById(id);

            return Ok(res);

        }
    }
}

