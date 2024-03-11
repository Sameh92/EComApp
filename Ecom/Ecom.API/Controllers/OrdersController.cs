using AutoMapper;
using Ecom.API.Response;
using Ecom.Core.Entities.Order;
using Ecom.Core.Interfaces;
using Ecom.Core.Service;
using Ecom.Helper.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _uOW;
        private readonly IOrderServices _orderServices;
        private readonly IMapper _mapper;

        public OrdersController(IUnitOfWork uOW,IOrderServices orderServices,IMapper mapper)
        {
            _uOW = uOW;
            _orderServices = orderServices;
            _mapper = mapper;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder(OrderDto orderDto)
        {
            var email = HttpContext.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var address = _mapper.Map<AddressDto, ShipAddress>(orderDto.ShipToAddress);

            var order=await _orderServices.CreateOrderAsync(email,orderDto.DeliveryMethodId,orderDto.BasketId,address);

            if(order is null) return BadRequest(new BaseCommonResponse<object>(400,"Error While Creating Order"));
            return Ok(new BaseCommonResponse<Order>(order));

        }

        [HttpGet("get-orders-for-user")]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()       
        {
            var email= HttpContext.User?.Claims.FirstOrDefault(x=>x.Type == ClaimTypes.Email)?.Value;
            var order= await _orderServices.GetOrdersForUserAsync(email);
            var result=_mapper.Map<IReadOnlyList<Order>, IReadOnlyList< OrderToReturnDto>>(order);
      
            
            return Ok(new BaseCommonResponse<IReadOnlyList<OrderToReturnDto>>(result));
        }
        [HttpGet("get-order-by-id/{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(int id)
        {
            var email= HttpContext.User?.Claims.FirstOrDefault(x=>x.Type== ClaimTypes.Email)?.Value;
            var order = await _orderServices.GetOrderByIdAsync(id, email);
            if (order is null) return NotFound(new BaseCommonResponse<object>(404));
            var result= _mapper.Map<Order,OrderToReturnDto>(order);
            return Ok(new BaseCommonResponse<OrderToReturnDto>(result));
        }

        [HttpGet("get-delivery-methods")]
        public async Task<ActionResult<BaseCommonResponse<IReadOnlyList<DeliveryMethod>>>>GetDeliveyMethods()
        {
            var data=await _orderServices.GetDeliveryMethodsAsync();
            return Ok(new BaseCommonResponse<IReadOnlyList<DeliveryMethod>>(data));
        }
    }
}
