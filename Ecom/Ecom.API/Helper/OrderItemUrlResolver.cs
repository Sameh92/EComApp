using AutoMapper;
using Ecom.Core.Entities.Order;
using Ecom.Helper.Dtos;

namespace Ecom.API.Helper
{
    public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.ProductItemOrderd.PictureUrl)) {
            return _configuration["APIURL"] +source.ProductItemOrderd.PictureUrl;
            }
            return null;
        }
    }
}
