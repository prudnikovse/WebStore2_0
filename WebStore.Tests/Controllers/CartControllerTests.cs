using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using WebStore.Controllers;
using WebStore.Domain.DTO.Orders;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CartControllerTests
    {
        [TestMethod]
        public void CheckOutViewModel()
        {
            var cartServiceMock = new Mock<ICartService>();
            var orderServiceMock = new Mock<IOrderService>();
            
            var controller = new CartController(cartServiceMock.Object);

            controller.ModelState.AddModelError("error", "InvalidModel");

            const string expectedModelName = "Test order";

            var result = controller.CheckOut(new OrderViewModel() { Name = expectedModelName }, orderServiceMock.Object);

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<DetailsViewModel>(viewResult.Model);

            Assert.Equal(expectedModelName, model.OrderViewModel.Name);
        }

        [TestMethod]
        public void CheckOutService()
        {
            var cartServiceMock = new Mock<ICartService>();
            var orderServiceMock = new Mock<IOrderService>();

            cartServiceMock
                .Setup(c => c.TransformFromCart())
                .Returns(() => new CartViewModel
                {
                    Items = new Dictionary<ProductViewModel, int>
                    {
                        { new ProductViewModel { Name = "Product" }, 1 }
                    }                 
                });

            const int expectedOrderId = 1;

            orderServiceMock
                .Setup(c => c.CreateOrder(It.IsAny<CreateOrderModel>(), It.IsAny<string>()))
                .Returns(() => new OrderDTO
                {
                    Id = expectedOrderId
                });

            var controller = new CartController(cartServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new System.Security.Claims.ClaimsPrincipal(
                            new ClaimsIdentity(
                                new[] { new Claim(ClaimTypes.NameIdentifier, "1") }))
                    }
                }
            };

            var result = controller.CheckOut(new OrderViewModel() 
            { 
                Name = "Test",
                Address = "Address",
                Phone = "Phone"
            }, orderServiceMock.Object);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Null(redirectResult.ControllerName);
            Assert.Equal(nameof(CartController.OrderConfirmed), redirectResult.ActionName);
            Assert.Equal(expectedOrderId, redirectResult.RouteValues["id"]);
        }
    }
}
