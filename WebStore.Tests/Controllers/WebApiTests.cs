using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Controllers;
using WebStore.Interfaces.Api;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class WebApiTests
    {       
        [TestMethod]
        public void IndexTests()
        {
            var valuesService = new Mock<IValuesService>();

            valuesService
                .Setup(srv => srv.Get())
                .Returns(new[] { "1", "2", "3" });

            var valueController = new ValuesController(valuesService.Object);

            var result = valueController.Index();

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<IEnumerable<string>>(viewResult.Model);

            valuesService.Verify(srv => srv.Get());
            valuesService.VerifyNoOtherCalls();
        }
    }

    
}
