using AutoMapper;
using Azure;
using Ecom.API.Errors;
using Ecom.API.Extensions;
using Ecom.API.Response;
using Ecom.Core.Entities;
using Ecom.Core.Service;
using Ecom.Helper.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,ITokenService tokenService,IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;   
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.EmaiL);
            if (user is null) return Unauthorized(new BaseCommonResponse<UserDto>(401));
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (result is null || result.Succeeded==false) return Unauthorized(new BaseCommonResponse<UserDto>(401));
            var data = new UserDto
            {
                DisplayName = user.DisplayName,
                Token =_tokenService.CreateToken(user),
                Email = user.Email
            };
            return Ok(new BaseCommonResponse<UserDto>(data) { StatusCode=200});

        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var res = await CheckEmailExist(registerDto.Email);
            var baseResponse = (res.Result as OkObjectResult)?.Value as BaseCommonResponse<object>;
            var response = (bool)baseResponse.Data;
            if (response) 
            {
               
                    return new BadRequestObjectResult(new ApiValidationErrorResponse<object>
                    {
                        Errors = new[] { "This Email Already taken" }
                    });
                
            }
            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                UserName = registerDto.Email,
                Email = registerDto.Email
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            var test = result.Errors;
            if (result.Succeeded == false) return BadRequest(new ApiValidationErrorResponse<UserDto>()
            {                               
                Errors = result.Errors.Select(x=>x.Description)
            });
            var data= new UserDto {  DisplayName=registerDto.DisplayName,Email=registerDto.Email, Token=_tokenService.CreateToken(user)};
            return Ok(new BaseCommonResponse<UserDto>(data) { StatusCode = 200});
        }
        [HttpGet("check-email-exist")]
        public async Task<ActionResult<BaseCommonResponse<object>>> CheckEmailExist([FromQuery] string email)
        {
            var data = await _userManager.FindByNameAsync(email) != null;
            return Ok(new BaseCommonResponse<object>(data));
        }
        [Authorize]
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Test");
        }
        [Authorize]
        [HttpGet("get-current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            //var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;   
            //var user = await _userManager.FindByNameAsync(email);

            var user = await _userManager.FindEmailByClaimPrinceipal(HttpContext.User);
            
            var ageClaim = HttpContext.User?.Claims.FirstOrDefault(x => x.Type == "Age");
            //var Key = ageClaim.Type;  // Type to get the key
            var age = ageClaim.Value;
            int ageNumber = int.Parse(age);
            var data = new UserDto { DisplayName = user.DisplayName, Email = user.Email, Token = _tokenService.CreateToken(user) };
            return Ok(new BaseCommonResponse<UserDto>(data));
        }
    
        [Authorize]
        [HttpGet("get-user-address")]
        public async Task<IActionResult> GetUserAddress()
        {
            var user = await _userManager.FindByClaimPrinceipalWithAddress(HttpContext.User);

            var data = _mapper.Map<Address, AddressDto>(user.Address);
            return Ok(new BaseCommonResponse<AddressDto>(data));

        }
        [Authorize]
        [HttpPut("update-user-address")]
        public async Task<IActionResult> UpdateUserAddress(AddressDto dto)
        {
            var user = await _userManager.FindByClaimPrinceipalWithAddress(HttpContext.User);
            user.Address = _mapper.Map<AddressDto, Address>(dto);
            var result= await _userManager.UpdateAsync(user);
            var data = _mapper.Map<Address, AddressDto>(user.Address);
            if (result.Succeeded) return Ok(new BaseCommonResponse<AddressDto>(data));
            return BadRequest(new BaseCommonResponse<object>(400));
        }
    }
}
