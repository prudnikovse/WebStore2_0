using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Consts;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using System.Net.Http;

namespace WebStore.Clients
{
    public class EmployeeClient : BaseClient, IEmployeesData
    {
        public EmployeeClient(IConfiguration config)
            : base(config, WebApiConsts.Employees)
        {
        }

        public void Add(EmployeeView Employee) => Post(_ServiceAddress, Employee);

        public bool Delete(int id) => Delete($"{_ServiceAddress}/{id}").IsSuccessStatusCode;

        public EmployeeView Edit(int id, EmployeeView Employee)
        {
            var res = Put($"{_ServiceAddress}/{id}", Employee);
            return res.Content.ReadAsAsync<EmployeeView>().Result;
        }

        public IEnumerable<EmployeeView> GetAll() => Get<List<EmployeeView>>(_ServiceAddress);

        public EmployeeView GetById(int id) => Get<EmployeeView>($"{_ServiceAddress}/{id}");

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
