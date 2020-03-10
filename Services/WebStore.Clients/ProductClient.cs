using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Consts;
using WebStore.Domain.Entities;
using System.Net.Http;
using WebStore.Interfaces.Services;
using WebStore.Domain.DTO.Products;

namespace WebStore.Clients
{
    public class ProductClient : BaseClient, IProductData
    {
        public ProductClient(IConfiguration config)
            : base(config, WebApiConsts.Products)
        {
        }

        public BrandDTO GetBrandById(int id) => Get<BrandDTO>($"{_ServiceAddress}/brands/{id}");

        public IEnumerable<Brand> GetBrands() => Get<List<Brand>>($"{_ServiceAddress}/brands");
    
        public ProductDTO GetProductById(int id) => Get<ProductDTO>($"{_ServiceAddress}/{id}");


        public IEnumerable<ProductDTO> GetProducts(ProductFilter Filter = null)
        {
            var res = Post(_ServiceAddress, Filter);

            if (res.IsSuccessStatusCode)
                return res.Content.ReadAsAsync<List<ProductDTO>>().Result;

            return Array.Empty<ProductDTO>();
        }

        public SectionDTO GetSectionById(int id) => Get<SectionDTO>($"{_ServiceAddress}/sections/{id}");

        public IEnumerable<Section> GetSections() => Get<List<Section>>($"{_ServiceAddress}/sections");
    }
}
