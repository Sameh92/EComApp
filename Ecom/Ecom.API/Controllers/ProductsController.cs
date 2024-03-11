using AutoMapper;
using Ecom.Helper.Dtos;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ecom.API.Response;
using Ecom.API.Helper;
using Ecom.Helper.RequestHelper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _uOW;
        private readonly IMapper _mapper;
        
        public ProductsController(IUnitOfWork uOW,IMapper mapper)
        {
            _uOW = uOW;
            _mapper = mapper;
        }
        [HttpGet("get-all-products")]
        public async Task<ActionResult> Get([FromQuery] ProductParams productParams)
        {
            // using Include Category inside GetAllAsync()
            //var src=await _uOW.ProductRepository.GetAllAsync(x=>x.Category);

            var src = await _uOW.ProductRepository.GetAllAsync(productParams);        
            //  var result = _mapper.Map<IReadOnlyList<ProductDto>>(src.ProductDtos);
            var result = src.ProductDtos;
            var data = new Pagination<ProductDto>(productParams.PageNumber, productParams.PageSize, src.TotalItems, result);
            return Ok(new BaseCommonResponse<Pagination<ProductDto>>(data) {StatusCode=200});
        }

        [HttpGet("get-product-by-id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseCommonResponse<Product>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get(int id)
        {
            // prouct with Category include inside GetAllAsync()
            var src = await _uOW.ProductRepository.GetByIdAsync(id, x => x.Category);
            if (src is null)
            {
                return NotFound(new BaseCommonResponse<Product>(404));
            }
            var data = _mapper.Map<ProductDto>(src);

            return Ok(new BaseCommonResponse<ProductDto>{Data=data,StatusCode=200});
        }
        [HttpPost("add-new-product")]
        public async Task<ActionResult> Post([FromForm]CreateProductDto productDto)
        {
            try
            {
                if(ModelState.IsValid)
                {
                   // var res = _mapper.Map<Product>(productDto);
                    var res = await _uOW.ProductRepository.AddAsync(productDto);
                    return res?Ok(productDto) :BadRequest(res);
                }
                return BadRequest(productDto);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("update-exiting-product/{id}")]
        public async Task<ActionResult> Put(int id,[FromForm] UpdateProductDto productDto)
        { 
            try
            {
               
                if( ModelState.IsValid )
                {
                    var res = await _uOW.ProductRepository.UpdateAsync(id,productDto);
                    return res ? Ok(productDto) : BadRequest();
                }
                return BadRequest(productDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("delete-exiting-product/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var res = await _uOW.ProductRepository.DeleteAsyncWithPicture(id);
                    return res ? Ok(res) : BadRequest(res);
                }
                return NotFound($"This is {id} Not Found");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
