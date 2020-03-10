using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Controllers;
using WebStore.Domain.ViewModels.BreadCrumbs;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class BreadCrumbsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public BreadCrumbsViewComponent(IProductData ProductData) => _ProductData = ProductData;

        private (BreadCrumbsType Type, int Id, BreadCrumbsType FromType) GetParameters()
        {
            BreadCrumbsType type = Request.Query.ContainsKey("SectionId")
                ? BreadCrumbsType.Section
                : Request.Query.ContainsKey("BrandId")
                ? BreadCrumbsType.Brand
                : BreadCrumbsType.None;

            if((string)ViewContext.RouteData.Values["action"] == nameof(CatalogController.Details))
            {
                type = BreadCrumbsType.Product;
            }

            int id = 0;
            BreadCrumbsType fromType = BreadCrumbsType.Section;

            switch(type)
            {
                case BreadCrumbsType.None:
                    break;
                case BreadCrumbsType.Section:
                    id = int.Parse(Request.Query["SectionId"].ToString());
                    break;
                case BreadCrumbsType.Brand:
                    id = int.Parse(Request.Query["BrandId"].ToString());
                    break;
                case BreadCrumbsType.Product:
                    id = int.Parse(ViewContext.RouteData.Values["id"].ToString());
                    if(Request.Query.ContainsKey("FromBrand"))
                    {
                        fromType = BreadCrumbsType.Brand;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return (type, id, fromType);
        }

        public IViewComponentResult Invoke()
        {
            var (type, id, fromType) = GetParameters();

            switch (type)
            {
                case BreadCrumbsType.Section:
                case BreadCrumbsType.Brand:
                    return View(new []
                    {
                        new BreadCrumbsViewModel
                        {
                            BreadCrumbsType = type,
                            Id = id.ToString(),
                            Name = (type == BreadCrumbsType.Section) ? _ProductData.GetSectionById(id)?.Name
                                : _ProductData.GetBrandById(id)?.Name
                        }
                    });
                case BreadCrumbsType.Product:
                    var product = _ProductData.GetProductById(id);
                    return View(new[]
                    {                   
                        new BreadCrumbsViewModel
                        {
                            BreadCrumbsType = fromType,
                            Id = (fromType == BreadCrumbsType.Section) ? product?.Section?.Id.ToString()
                                : product?.Brand?.Id.ToString(),
                            Name = (fromType == BreadCrumbsType.Section) ? _ProductData.GetSectionById(id)?.Name
                                : _ProductData.GetBrandById(id)?.Name
                        },
                        new BreadCrumbsViewModel
                        {
                            BreadCrumbsType = BreadCrumbsType.Product,
                            Id = product.Id.ToString(),
                            Name = product.Name
                        }
                    });
                default:
                    return View(Array.Empty<BreadCrumbsViewModel>());
            }
        }
    }
}
