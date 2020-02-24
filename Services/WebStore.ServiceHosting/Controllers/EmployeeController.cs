using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Consts;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebApiConsts.Employees)]
    [ApiController]
    public class EmployeeController : ControllerBase, IEmployeesData
    {
        private readonly IEmployeesData _EmployeesData;

        public EmployeeController(IEmployeesData employeesData) => _EmployeesData = employeesData;

        [HttpPost, ActionName("Post")]
        public void Add([FromBody] EmployeeView Employee) => _EmployeesData.Add(Employee);

        [HttpDelete("{id}")]
        public bool Delete(int id) => _EmployeesData.Delete(id);

        [HttpPut("{id}"), ActionName("Put")]
        public EmployeeView Edit(int id, [FromBody] EmployeeView Employee) => _EmployeesData.Edit(id, Employee);

        [HttpGet, ActionName("Get")]
        public IEnumerable<EmployeeView> GetAll() => _EmployeesData.GetAll();

        [HttpGet("{id}"), ActionName("Get")]
        public EmployeeView GetById(int id) => _EmployeesData.GetById(id);

        [NonAction]
        public void SaveChanges() => _EmployeesData.SaveChanges();
    }
}