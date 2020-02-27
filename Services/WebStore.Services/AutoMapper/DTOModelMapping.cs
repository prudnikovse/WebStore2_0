using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.DTO.Orders;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;

namespace WebStore.Services.AutoMapper
{
    public class DTOModelMapping : Profile
    {
        public DTOModelMapping()
        {
            CreateMap<Order, OrderDTO>();
            CreateMap<OrderItem, OrderItemDTO>();
            CreateMap<Product, ProductDTO>();
            CreateMap<Brand, BrandDTO>();
            CreateMap<Section, SectionDTO>();



            //.ForMember( um => um.UserName, opt => opt.MapFrom(vm => vm.UserName));

            //CreateMap<ProductDTO, ProductViewModel>()
            //    .ReverseMap();
        }
    }
}
