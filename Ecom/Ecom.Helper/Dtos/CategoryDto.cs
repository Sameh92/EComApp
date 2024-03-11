using System.ComponentModel.DataAnnotations;

namespace Ecom.Helper.Dtos
{
    public class CategoryDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class ListingCategoryDto:CategoryDto
    {
        public int Id { get; set; }


    }
}
