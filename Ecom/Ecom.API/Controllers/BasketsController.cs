using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Helper.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BasketsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("get-basket-item/{Id}")]
        public async Task<IActionResult> GetBasketById(string id)
        {
          
            var _basket=await _unitOfWork.BasketRepository.GetBasketAsync(id);
            return Ok(_basket?? new CustomerBasket(id));
        }
        [HttpPost("update-basket")]
        public async Task<IActionResult>UpdateBasket(CustomerBasketDto customerBasket)
        {
            var _basket= await _unitOfWork.BasketRepository.UpdateBasketAsync(customerBasket);
            return Ok(_basket);
        }
        [HttpDelete("delete-basket-item/{id}")]
        public async Task <IActionResult> DeleteBasket(string id)
        {
            return Ok(await _unitOfWork.BasketRepository.DeleteBasketAsync(id));
        }
    }
}
