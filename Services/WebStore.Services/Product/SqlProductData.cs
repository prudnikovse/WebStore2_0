using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreContext _db;
        private readonly IMapper _Mapper;

        public SqlProductData(WebStoreContext db, IMapper Mapper)
        {
            _db = db;
            _Mapper = Mapper;
        }

        public IEnumerable<SectionDTO> GetSections() => _db.Sections
           //.Include(section => section.Products)
           .AsEnumerable()
           .Select(_Mapper.Map<SectionDTO>);

        public IEnumerable<BrandDTO> GetBrands() => _db.Brands
           //.Include(brand => brand.Products)
            .AsEnumerable()
            .Select(brand => { 
               var brandDto = _Mapper.Map<BrandDTO>(brand);
                brandDto.ProductCount = _db.Products.Where(pr => pr.BrandId == brand.Id).Count();
               return brandDto;
           });

        public PageProductsDTO GetProducts(ProductFilter Filter = null)
        {
            IQueryable<Product> query = _db.Products;

            if (Filter?.BrandId != null)
                query = query.Where(product => product.BrandId == Filter.BrandId);

            if (Filter?.SectionId != null)
                query = query.Where(product => product.SectionId == Filter.SectionId);

            if (Filter?.Ids?.Count > 0)
                query = query.Where(product => Filter.Ids.Contains(product.Id));

            var totalCount = query.Count();

            if (Filter.PageSize.HasValue)
                query = query.Skip((Filter.Page - 1) * Filter.PageSize.Value)
                    .Take(Filter.PageSize.Value);
          
            var res =  new PageProductsDTO
            {
                Products = query.Select(_Mapper.Map<ProductDTO>).AsEnumerable(),
                TotalCount = totalCount
            };

            return res;
        }                 

        public ProductDTO GetProductById(int id)
        {
            var product = _db.Products
           .Include(p => p.Brand)
           .Include(p => p.Section)
           .FirstOrDefault(p => p.Id == id);

            return product == null ? null : _Mapper.Map<ProductDTO>(product);       
        }

        public SectionDTO GetSectionById(int id) => _Mapper.Map<SectionDTO>(_db.Sections.Find(id));

        public BrandDTO GetBrandById(int id) => _Mapper.Map<BrandDTO>(_db.Brands.Find(id));
    }
}
