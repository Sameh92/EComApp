using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Ecom.Helper.Dtos
{
    public class BaseProductDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(1,999,ErrorMessage = "Price Limited By {0} and {1}")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "{0} Must Be Number !")]
        
        public decimal Price { get; set; }      

    }

    public class ProductDto : BaseProductDto
    {
        public int Id { get; set; }


        //Note NameCategory will be null in Auto Mapper
        // public string NameCategory { get; set; }
        public string CategoryName { get; set; }
        public string ProductPicture { get; set; }
    }
    public class ReturnProductDto
    {
        public int TotalItems { get; set; }
        public List<ProductDto> ProductDtos { get; set; }
    }
    public class CreateProductDto:BaseProductDto
    {
        public int CategoryId { get; set; }
        public IFormFile Image { get; set; }
    }
    public class UpdateProductDto:BaseProductDto
    {
       
        public int CategoryId { get; set; }
        public string OldImage {  get; set; }
        public IFormFile Image { get; set; }
    }
}
