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
    public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(IConfiguration config)
            : base(config, WebApiConsts.Products)
        {
        }

        public IEnumerable<Brand> GetBrands() => Get<List<Brand>>($"{_ServiceAddress}/brands");
    
        public ProductDTO GetProductById(int id) => Get<ProductDTO>($"{_ServiceAddress}/{id}");


        public IEnumerable<ProductDTO> GetProducts(ProductFilter Filter = null)
        {
            var res = Post(_ServiceAddress, Filter);

            if (res.IsSuccessStatusCode)
                return res.Content.ReadAsAsync<List<ProductDTO>>().Result;

            return Array.Empty<ProductDTO>();
        }

        public IEnumerable<Section> GetSections() => Get<List<Section>>($"{_ServiceAddress}/sections");
    }
}
