using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class BrandsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;
        private readonly IMapper _Mapper;

        public BrandsViewComponent(IProductData ProductData, IMapper Mapper)
        {
            _ProductData = ProductData;
            _Mapper = Mapper;
        }

        public IViewComponentResult Invoke(string BrandId) => View(new BrandCompleteViewModel
        {
            Brands = GetBrands(),
            CurrentBrandId = int.TryParse(BrandId, out var id) ? id : (int?)null
        });

        private IEnumerable<BrandViewModel> GetBrands() => _ProductData
           .GetBrands()         
           .Select(_Mapper.Map<BrandViewModel>)
           //.Select(brand => new BrandViewModel
           // {
           //     Id = brand.Id,
           //     Name = brand.Name,
           //     Order = brand.Order,
           //     ProductCount = brand.ProductCount
           // })
           .OrderBy(brand => brand.Order)
           .ToList();

    }
}
