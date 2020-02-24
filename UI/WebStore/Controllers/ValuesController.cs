using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Api;

namespace WebStore.Controllers
{
    public class ValuesController : Controller
    {
        private readonly IValuesService _ValuesService;

        public ValuesController(IValuesService valuesService)
        {
            _ValuesService = valuesService;
        }

        public IActionResult Index()
        {
            var values = _ValuesService.Get();
            return View(values);
        }
    }
}