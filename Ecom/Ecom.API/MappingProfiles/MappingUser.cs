using AutoMapper;
using Ecom.Core.Entities;
using Ecom.Helper.Dtos;

namespace Ecom.API.MappingProfiles
{
    public class MappingUser:Profile
    {
        public MappingUser()
        {
            CreateMap<Address, AddressDto>().ReverseMap();
          //  CreateMap<ShipAddressDto,AddressDto>().ReverseMap();
        }
    }
}
