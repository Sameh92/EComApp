using AutoMapper;
using Ecom.Core.Entities;
using Ecom.Helper.Dtos;

namespace Ecom.API.MappingProfiles
{
    public class MappingBasket:Profile
    {
        public MappingBasket()
        {
            CreateMap<CustomerBasket,CustomerBasketDto>().ReverseMap();
            CreateMap<BasketItem,BasketItemDto>().ReverseMap();
        }
    }
}
