﻿using AutoMapper;
using Ecom.Helper.Dtos;
using Ecom.Core.Entities;

namespace Ecom.API.MappingProfiles
{
    public class MappingCategory:Profile
    {
        public MappingCategory()
        {
            CreateMap<CategoryDto,Category>().ReverseMap();
            CreateMap<ListingCategoryDto,Category>().ReverseMap();
        }
    }
}
