using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace WebStore.Services.Tests
{
    [TestClass]
    public class CartServiceTests
    {
        [TestMethod]
        public void CartClassItemsCount()
        {
            const int expectedCount = 4;

            var cart = new Cart
            {
                Items = new List<CartItem>
                    {
                        { new CartItem { ProductId = 1, Quantity = 1} },
                        { new CartItem { ProductId = 2, Quantity = 3 } }
                    }
            };

            var actualCount = cart.ItemsCount;

            Assert.Equal(expectedCount, actualCount);
        }

        [TestMethod]
        public void CartViewModelItemsCount()
        {
            const int expectedCount = 4;

            var cart = new CartViewModel
            {
                Items = new Dictionary<ProductViewModel, int>
                    {
                        { new ProductViewModel { Id = 1, Name = "", Price = 0.5m }, 1 },
                        { new ProductViewModel { Id = 2, Name = "", Price = 0.5m }, 3 }
                    }
            };

            var actualCount = cart.ItemsCount;

            Assert.Equal(expectedCount, actualCount);
        }

        [TestMethod]
        public void CartServiceAddToCart()
        {
            var cart = new Cart
            {
                Items = new List<CartItem>()                    
            };

            var productDataMock = new Mock<IProductData>();
            var cartStoreMock = new Mock<ICartStore>();

            cartStoreMock
                .Setup(c => c.Cart)
                .Returns(cart);

            var cartService = new CartService(productDataMock.Object, cartStoreMock.Object);

            const int expectedId = 5;

            cartService.AddToCart(expectedId);
            

            Assert.Equal(1, cart.ItemsCount);
            Assert.Single(cart.Items);
            Assert.Equal(expectedId, cart.Items[0].ProductId);
        }

        [TestMethod]
        public void CartServiceRemoveFromCart()
        {
            const int itemId = 1;

            var cart = new Cart
            {
                Items = new List<CartItem>
                    {
                        { new CartItem { ProductId = itemId, Quantity = 1} },
                        { new CartItem { ProductId = 2, Quantity = 3 } }
                    }
            };

            var productDataMock = new Mock<IProductData>();
            var cartStoreMock = new Mock<ICartStore>();

            cartStoreMock
                .Setup(c => c.Cart)
                .Returns(cart);

            var cartService = new CartService(productDataMock.Object, cartStoreMock.Object);

            cartService.RemoveFromCart(itemId);

            Assert.Single(cart.Items);
            Assert.Equal(2, cart.Items[0].ProductId);
        }

        [TestMethod]
        public void CartServiceRemoveAll()
        {
            var cart = new Cart
            {
                Items = new List<CartItem>
                    {
                        { new CartItem { ProductId = 1, Quantity = 1} },
                        { new CartItem { ProductId = 2, Quantity = 3 } }
                    }
            };

            var productDataMock = new Mock<IProductData>();
            var cartStoreMock = new Mock<ICartStore>();

            cartStoreMock
                .Setup(c => c.Cart)
                .Returns(cart);

            var cartService = new CartService(productDataMock.Object, cartStoreMock.Object);

            cartService.RemoveAll();

            Assert.Empty(cart.Items);
        }

        [TestMethod]
        public void CartServiceDecrement()
        {
            const int itemId = 1;

            var cart = new Cart
            {
                Items = new List<CartItem>
                    {
                        { new CartItem { ProductId = itemId, Quantity = 3} },
                        { new CartItem { ProductId = 2, Quantity = 5 } }
                    }
            };

            var productDataMock = new Mock<IProductData>();
            var cartStoreMock = new Mock<ICartStore>();

            cartStoreMock
                .Setup(c => c.Cart)
                .Returns(cart);

            var cartService = new CartService(productDataMock.Object, cartStoreMock.Object);

            cartService.DecrementFromCart(itemId);

            Assert.Equal(2, cart.Items[0].Quantity);
            Assert.Equal(7, cart.ItemsCount);
            Assert.Equal(2, cart.Items.Count);
            Assert.Equal(itemId, cart.Items[0].ProductId);
        }

        [TestMethod]
        public void CartServiceDecrementToZero()
        {
            const int itemId = 1;

            var cart = new Cart
            {
                Items = new List<CartItem>
                    {
                        { new CartItem { ProductId = itemId, Quantity = 1} },
                        { new CartItem { ProductId = 2, Quantity = 5 } }
                    }
            };

            var productDataMock = new Mock<IProductData>();
            var cartStoreMock = new Mock<ICartStore>();

            cartStoreMock
                .Setup(c => c.Cart)
                .Returns(cart);

            var cartService = new CartService(productDataMock.Object, cartStoreMock.Object);

            cartService.DecrementFromCart(itemId);

            Assert.Equal(5, cart.ItemsCount);
            Assert.Single(cart.Items);
        }

        [TestMethod]
        public void CartServiceTransformFromCart()
        {
            var cart = new Cart
            {
                Items = new List<CartItem>
                    {
                        { new CartItem { ProductId = 1, Quantity = 1} },
                        { new CartItem { ProductId = 2, Quantity = 5 } }
                    }
            };

            var products = new PageProductsDTO
            {
                Products = new List<ProductDTO> {
                    {
                        new ProductDTO
                        {
                            Id = 1,
                            Name = "Product 1",
                            ImageUrl = "Product1.png",
                            Order = 0,
                            Price = 1.1m,
                            Brand = new BrandDTO
                            {
                                Id = 1,
                                Name = "Brand of product 1"
                            },
                            Section = new SectionDTO
                            {
                                Id = 1,
                                Name = "Section of product 1"
                            }
                        }
                    },
                    {
                        new ProductDTO
                        {
                            Id = 2,
                            Name = "Product 2",
                            ImageUrl = "Product2.png",
                            Order = 0,
                            Price = 2.2m,
                            Brand = new BrandDTO
                            {
                                Id = 2,
                                Name = "Brand of product 2"
                            },
                            Section = new SectionDTO
                            {
                                Id = 2,
                                Name = "Section of product 2"
                            }
                        }
                    }
                },
                TotalCount = 2
            };

            var productDataMock = new Mock<IProductData>();
            var cartStoreMock = new Mock<ICartStore>();

            productDataMock
                .Setup(c => c.GetProducts(It.IsAny<ProductFilter>()))
                .Returns(products);

            cartStoreMock
                .Setup(c => c.Cart)
                .Returns(cart);

            var cartService = new CartService(productDataMock.Object, cartStoreMock.Object);

            var res = cartService.TransformFromCart();

            Assert.Equal(6, cart.ItemsCount);
            Assert.Equal(1.1m, res.Items.First().Key.Price);
        }
    }
}
