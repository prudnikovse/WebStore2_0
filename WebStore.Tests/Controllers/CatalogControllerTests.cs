using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.Controllers;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CatalogControllerTests
    {
        [TestMethod]
        public void DetailsViewGetProduct()
        {
            var expectedProductId = 1;
            var expectedPrice = 10m;
            var expectedProductName = $"Product id {expectedProductId}";
            var expectedBrandName = $"Brand of product {expectedProductId}";

            var productDataMock = new Mock<IProductData>();

            productDataMock
                .Setup(p => p.GetProductById(It.IsAny<int>()))
                .Returns<int>(id => new ProductDTO
                {
                    Id = id,
                    Name = $"Product id {id}",
                    ImageUrl = $"Image_id_{id}.png",
                    Order = 1,
                    Price = expectedPrice,
                    Brand = new BrandDTO
                    {
                        Id = 1,
                        Name = $"Brand of product {id}"
                    },
                    Section = new SectionDTO
                    {
                        Id = 1,
                        Name = $"Section of product {id}"
                    }
                });

            var configMock = new Mock<IConfiguration>();

            var controller = new CatalogController(productDataMock.Object, configMock.Object);

            var result = controller.Details(expectedProductId);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<ProductViewModel>(viewResult.Model);

            Assert.Equal(expectedProductId, model.Id);
            Assert.Equal(expectedProductName, model.Name);
            Assert.Equal(expectedBrandName, model.Brand);
        }

        [TestMethod]
        public void DetailsViewProductNotFound()
        {
            var expectedProductId = 1;

            var productDataMock = new Mock<IProductData>();

            var configMock = new Mock<IConfiguration>();

            productDataMock
                .Setup(p => p.GetProductById(It.IsAny<int>()))
                .Returns(default(ProductDTO));

            var controller = new CatalogController(productDataMock.Object, configMock.Object);

            var result = controller.Details(expectedProductId);

            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [TestMethod]
        public void ShopView()
        {
            var productDataMock = new Mock<IProductData>();

            var configMock = new Mock<IConfiguration>();

            productDataMock
                .Setup(p => p.GetProducts(It.IsAny<ProductFilter>()))
                .Returns<ProductFilter>(filter => new PageProductsDTO
                {
                    Products = new List<ProductDTO>
                    {
                        new ProductDTO
                        {
                            Id = 1,
                            Name = "Product 1",
                            ImageUrl = "Product1.png",
                            Order = 0,
                            Price = 10m,
                            Brand = new BrandDTO
                            {
                                Id = 1,
                                Name = $"Brand of product 1"
                            },
                            Section = new SectionDTO
                            {
                                Id = 1,
                                Name = $"Section of product 1"
                            }
                        },
                        new ProductDTO
                        {
                            Id = 2,
                            Name = "Product 2",
                            ImageUrl = "Product2.png",
                            Order = 0,
                            Price = 20m,
                            Brand = new BrandDTO
                            {
                                Id = 2,
                                Name = $"Brand of product 2"
                            },
                            Section = new SectionDTO
                            {
                                Id = 2,
                                Name = $"Section of product 2"
                            }
                        }
                    },
                    TotalCount = 2
                });

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<ProductViewModel>(It.IsAny<ProductDTO>()))
                .Returns<ProductDTO>(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ImageUrl = p.ImageUrl,
                    Order = p.Order,
                    Price = 10m,
                    Brand = p.Brand.Name
                });

            var controller = new CatalogController(productDataMock.Object, configMock.Object);

            var expectedSectionId = 1;
            var expectedBrandId = 5;

            var result = controller.Shop(expectedSectionId, expectedBrandId, mapperMock.Object);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<CatalogViewModel>(viewResult.Model);

            Assert.Equal(2, model.Products.Count());
            Assert.Equal(expectedSectionId, model.SectionId);
            Assert.Equal(expectedBrandId, model.BrandId);

        }
    }
}
