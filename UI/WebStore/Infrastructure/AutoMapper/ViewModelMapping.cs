using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels;
using WebStore.Domain.ViewModels.Identity;

namespace WebStore.Infrastructure.AutoMapper
{
    public class ViewModelMapping : Profile
    {
        public ViewModelMapping()
        {
            CreateMap<RegisterUserViewModel, User>();
                //.ForMember( um => um.UserName, opt => opt.MapFrom(vm => vm.UserName));

            CreateMap<ProductDTO, ProductViewModel>()
                .ReverseMap();
        }
    }
}
