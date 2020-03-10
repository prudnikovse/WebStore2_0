using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Consts;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebApiConsts.Products)]
    [ApiController]
    public class ProductController : ControllerBase, IProductData
    {

        private readonly IProductData _ProductData;

        public ProductController(IProductData ProductData) => _ProductData = ProductData;

        /// <summary>Получение всех разделов каталога товаров</summary>
        /// <returns>Перечисление всех разделов каталога</returns>
        [HttpGet("sections")]
        public IEnumerable<Section> GetSections() => _ProductData.GetSections();

        /// <summary>Получение всех брендов товаров из каталога</summary>
        /// <returns>Перечисление брендов товаров каталога</returns>
        [HttpGet("brands")]
        public IEnumerable<Brand> GetBrands() => _ProductData.GetBrands();

        /// <summary>Получение товаров, удовлетворяющих критерию поиска</summary>
        /// <param name="Filter">Фильтр - критерий поиска товаров в каталоге</param>
        /// <returns>Перечисление всех товаров из каталога, удовлетворяющих критерию поиска</returns>
        [HttpPost, ActionName("Post")]
        public IEnumerable<ProductDTO> GetProducts([FromBody] ProductFilter Filter = null) => _ProductData.GetProducts(Filter);

        /// <summary>Получение информации по товару, заданному идентификатором</summary>
        /// <param name="id">Идентификатор товара, информацию по которому требуется получить</param>
        /// <returns>Информацию по товару, заданному идентификатором</returns>
        [HttpGet("{id}"), ActionName("Get")]
        public ProductDTO GetProductById(int id) => _ProductData.GetProductById(id);

        /// <summary>Получение раздела каталога товаров по его id</summary>
        /// <returns>Раздел каталога</returns>
        [HttpGet("sections/{id}")]
        public SectionDTO GetSectionById(int id) => _ProductData.GetSectionById(id);

        /// <summary>Получение бренда товара из каталога по id</summary>
        /// <returns>Бренд товаров каталога</returns>
        [HttpGet("brands/{id}")]
        public BrandDTO GetBrandById(int id) => _ProductData.GetBrandById(id);
    }
}