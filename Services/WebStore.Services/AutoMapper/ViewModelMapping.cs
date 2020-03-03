using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;

namespace WebStore.Services.AutoMapper
{
    public class ViewModelMapping : Profile
    {
        public ViewModelMapping()
        {
            CreateMap<OrderViewModel, Order>();
        }
    }
}
