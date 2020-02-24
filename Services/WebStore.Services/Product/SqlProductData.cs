using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Helpers;

namespace WebStore.Infrastructure.Services
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreContext _db;

        public SqlProductData(WebStoreContext db) => _db = db;

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

            Mapper.CreateMap<Product, ProductDTO>();

            return query
                .Select(p => Mapper.Map(p, new ProductDTO()
                {
                    Brand = p.Brand == null ? null : new BrandDTO()
                    {
                        Id = p.Brand.Id,
                        Name = p.Brand.Name
                    },
                    Section = p.Section == null ? null : new SectionDTO()
                    {
                        Id = p.Section.Id,
                        Name = p.Section.Name
                    }
                }))
                .AsEnumerable();
            //.Select(p => new ProductDTO()
            //{
            //    Id = p.Id,
            //    Name = p.Name,
            //    Order = p.Order,
            //    Price = p.Price,
            //    ImageUrl = p.ImageUrl,
            //    Brand = p.Brand == null ? null : new BrandDTO()
            //    {
            //        Id = p.Brand.Id,
            //        Name = p.Brand.Name
            //    },
            //    Section = p.Section == null ? null : new SectionDTO()
            //    {
            //        Id = p.Section.Id,
            //        Name = p.Section.Name
            //    }
            //})
            //.AsEnumerable(); /*query.ToArray();*/
        }

        public ProductDTO GetProductById(int id)
        {
            var product = _db.Products
           .Include(p => p.Brand)
           .Include(p => p.Section)
           .FirstOrDefault(p => p.Id == id);

            Mapper.CreateMap<Product, ProductDTO>();

            return product == null ? null : Mapper.Map(product, new ProductDTO()
            {
                Brand = product.Brand == null ? null : new BrandDTO()
                {
                    Id = product.Brand.Id,
                    Name = product.Brand.Name
                },
                Section = product.Section == null ? null : new SectionDTO()
                {
                    Id = product.Section.Id,
                    Name = product.Section.Name
                }
            });

            //new ProductDTO()
            //{
            //    Id = product.Id,
            //    Name = product.Name,
            //    Order = product.Order,
            //    Price = product.Price,
            //    ImageUrl = product.ImageUrl,
            //    Brand = product.Brand == null ? null : new BrandDTO()
            //    {
            //        Id = product.Brand.Id,
            //        Name = product.Brand.Name
            //    },
            //    Section = product.Section == null ? null : new SectionDTO()
            //    {
            //        Id = product.Section.Id,
            //        Name = product.Section.Name
            //    }
            //};
        }
    }
}
