using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.Entities.Order;
using Ecom.Helper.Dtos;

namespace Ecom.API.MappingProfiles
{
    public class MappingOrders:Profile
    {
        public MappingOrders() {

        CreateMap<Order,OrderToReturnDto>()
                .ForMember(d=>d.DeliveryMethod,o=>o.MapFrom(s=>s.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price))
                .ReverseMap();
        CreateMap<OrderItem,OrderItemDto>()
                .ForMember(d => d.ProductItemId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.ProductItemName, o => o.MapFrom(s => s.ProductItemOrderd.ProductItemName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ProductItemOrderd.PictureUrl))
                .ForMember(d=>d.PictureUrl,o=>o.MapFrom<OrderItemUrlResolver>())
                .ReverseMap();
        CreateMap<ShipAddress,AddressDto>().ReverseMap();
        CreateMap<ShipAddress,ShipAddressDto>().ReverseMap();
        
        }
    }
}
