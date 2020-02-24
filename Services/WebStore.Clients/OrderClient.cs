using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using WebStore.Domain.Consts;
using WebStore.Domain.DTO.Orders;
using WebStore.Interfaces.Services;

namespace WebStore.Clients
{
    public class OrderClient : BaseClient, IOrderService
    {
        public OrderClient(IConfiguration config)
            : base(config, WebApiConsts.Orders)
        {
        }

        public OrderDTO CreateOrder(CreateOrderModel OrderModel, string UserName) 
            => Post($"{_ServiceAddress}/{UserName}", OrderModel)
            .Content.ReadAsAsync<OrderDTO>()
            .Result;

        public OrderDTO GetOrderById(int id) => Get<OrderDTO>($"{_ServiceAddress}/{id}");

        public IEnumerable<OrderDTO> GetUserOrders(string UserName) => Get<List<OrderDTO>>($"{_ServiceAddress}/orders/{UserName}");
    }
}
