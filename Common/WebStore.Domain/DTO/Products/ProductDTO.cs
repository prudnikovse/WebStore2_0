using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.DTO.Products
{
    public class ProductDTO : INamedEntity, IOrderedEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public int Order { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public virtual BrandDTO Brand { get; set; }

        public virtual SectionDTO Section { get; set; }
    }   
}
