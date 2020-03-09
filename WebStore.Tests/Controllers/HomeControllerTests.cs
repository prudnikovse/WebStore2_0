using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using WebStore.Controllers;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        private HomeController _Controller;

        public HomeControllerTests()
        {
            var logger = new Mock<ILogger<HomeController>>();
            _Controller = new HomeController(logger.Object);
        }

        [TestMethod]
        public void IndexView()
        {          
            var res = _Controller.Index();

            Assert.IsType<ViewResult>(res);
        }

        [TestMethod]
        public void Error404View()
        {
            var res = _Controller.ErrorStatus("404");

            var action = Assert.IsType<RedirectToActionResult>(res);
            Assert.Null(action.ControllerName);
            Assert.Equal(nameof(HomeController.Error404), action.ActionName);
        }
    }
}
