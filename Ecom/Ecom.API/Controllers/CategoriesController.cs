using AutoMapper;
using Ecom.Helper.Dtos;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Ecom.API.Response;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _uOW;
        private readonly IMapper _mapper;
        public CategoriesController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _uOW = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet("get-all-categories")]
        public async Task<ActionResult> Get()
        {
            var allCategories=await _uOW.CategoryRepository.GetAllAsync();
            if(allCategories is not null) 
            {
                //  var res=allCategories.Select(x=>new ListingCategoryDto {Id=x.Id, Description=x.Description,Name=x.Name}).ToList();

                var res = _mapper.Map<IReadOnlyList<ListingCategoryDto>>(allCategories);
                // or you can use it in below way
                // var res = _mapper.Map<IReadOnlyList<Category>,IReadOnlyList<ListingCategoryDto>>(allCategories);

                return Ok(new BaseCommonResponse<IReadOnlyList<ListingCategoryDto>> { Data=res});
            }
         
            return BadRequest("Not Found");

        }

        [HttpGet("get-category-by-id/{id}")]
        public async Task<ActionResult>Get(int id)
        {
            var category = await _uOW.CategoryRepository.GetAsync(id);
            if(category is  null)
            {
                return BadRequest($"Not Found this id[{id}]");
            }
            //var newCatgoryDto = new ListingCategoryDto
            //{
            //    Id = category.Id,
            //    Name = category.Name,
            //    Description = category.Description,
            //};
            var newCatgoryDto = _mapper.Map<ListingCategoryDto>(category);
            
            return Ok(newCatgoryDto);
        }
        [HttpPost("add-new-category")]
        public async Task<ActionResult> Post(CategoryDto categoryDto)
        {
            try 
            {
                if(ModelState.IsValid)
                {
                    //var newCategory = new Category
                    //{
                    //    Name = categoryDto.Name,
                    //    Description = categoryDto.Description
                    //};
                    var newCategory = _mapper.Map<Category>(categoryDto);
                    await _uOW.CategoryRepository.AddAsync(newCategory);
                    return Ok(categoryDto);
                }
                return BadRequest(categoryDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
        [HttpPut("update-exiting-category-by-id/{id}")]

        public async Task<ActionResult>Put(int id, CategoryDto categoryDto)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var exitingCategory = await _uOW.CategoryRepository.GetAsync(id);
                    if (exitingCategory is not null)
                    {
                        //exitingCategory.Name = categoryDto.Name;
                        //exitingCategory.Description = categoryDto.Description;
                        _mapper.Map(categoryDto, exitingCategory);
                       
                        await _uOW.CategoryRepository.UpdateAsync(id,exitingCategory);
                        return Ok(categoryDto);

                    }

                }
                return BadRequest($"Category Not Found,Id [${id}] Incorrect");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("delete-category-by-id/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var exitingCategory = await _uOW.CategoryRepository.GetAsync(id);
                if(exitingCategory is not null)
                {
                    await _uOW.CategoryRepository.DeleteAsync(id);
                    return Ok($"This category [{exitingCategory.Name}] is deleted Successfully");
                }
                return BadRequest($"Category Not Found, Id [{id}] Incroorect");
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
