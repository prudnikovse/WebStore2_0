﻿using Microsoft.Extensions.Configuration;
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

        public IEnumerable<BrandDTO> GetBrands() => Get<List<BrandDTO>>($"{_ServiceAddress}/brands");
    
        public ProductDTO GetProductById(int id) => Get<ProductDTO>($"{_ServiceAddress}/{id}");


        public PageProductsDTO GetProducts(ProductFilter Filter = null)
        {
            var res = Post(_ServiceAddress, Filter ?? new ProductFilter());

            if (res.IsSuccessStatusCode)
                return res.Content.ReadAsAsync<PageProductsDTO>().Result;

            return new PageProductsDTO();
        }

        public SectionDTO GetSectionById(int id) => Get<SectionDTO>($"{_ServiceAddress}/sections/{id}");

        public IEnumerable<SectionDTO> GetSections() => Get<List<SectionDTO>>($"{_ServiceAddress}/sections");
    }
}
