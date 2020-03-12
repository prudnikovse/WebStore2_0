using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleMvcSitemap;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class SitemapController : Controller
    {
        public IActionResult Index([FromServices] IProductData ProductData)
        {
            var nodes = new List<SitemapNode>
            {
                new SitemapNode(Url.Action("Index", "Home")),
                new SitemapNode(Url.Action("ContactUs", "Home")),
                new SitemapNode(Url.Action("BlogSingle", "Home")),
                new SitemapNode(Url.Action("Shop", "Catalog"))
            };

            nodes.AddRange(ProductData.GetSections().Select(section => new SitemapNode(Url.Action("Shop", "Catalog",
                new { SectionId = section.Id }))));

            nodes.AddRange(ProductData.GetBrands().Select(brand => new SitemapNode(Url.Action("Shop", "Catalog",
                new { SectionId = brand.Id }))));

            nodes.AddRange(ProductData.GetProducts().Products.Select(product => new SitemapNode(Url.Action("Details", "Catalog",
                new { SectionId = product.Id }))));

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }
    }
}