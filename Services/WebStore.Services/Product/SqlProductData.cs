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

        public IEnumerable<Section> GetSections() => _db.Sections
           //.Include(section => section.Products)
           .AsEnumerable();

        public IEnumerable<Brand> GetBrands() => _db.Brands
           //.Include(brand => brand.Products)
           .AsEnumerable();

        public IEnumerable<ProductDTO> GetProducts(ProductFilter Filter = null)
        {
            IQueryable<Product> query = _db.Products;

            if (Filter?.BrandId != null)
                query = query.Where(product => product.BrandId == Filter.BrandId);

            if (Filter?.SectionId != null)
                query = query.Where(product => product.SectionId == Filter.SectionId);

            return query
                .Select(_Mapper.Map<ProductDTO>)
                .AsEnumerable();    
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
