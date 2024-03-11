using AutoMapper;
using Ecom.Helper.Dtos;
using Ecom.Core.Entities;
using Ecom.API.Helper;

namespace Ecom.API.MappingProfiles
{
    public class MappingProduct : Profile
    {
        public MappingProduct()
        {
            CreateMap<Product,ProductDto>()     
            .ForMember(d=>d.ProductPicture,o=>o.MapFrom<ProductUrlResolver>()).ReverseMap();

            CreateMap<CreateProductDto,Product>().ReverseMap();
            //Note Also those working for mapping categoryName 
           // CreateMap<Product, ProductDto>().ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name)).ReverseMap();
           CreateMap<Product,UpdateProductDto>().ReverseMap();

        }
    }
}
