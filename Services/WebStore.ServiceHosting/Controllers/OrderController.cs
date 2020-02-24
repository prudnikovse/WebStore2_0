using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Consts;
using WebStore.Domain.DTO.Orders;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebApiConsts.Orders)]
    [ApiController]
    public class OrderController : ControllerBase, IOrderService
    {
        private readonly IOrderService _OrderService;

        public OrderController(IOrderService OrderService) => _OrderService = OrderService;

        [HttpGet("orders/{UserName}")]
        public IEnumerable<OrderDTO> GetUserOrders(string UserName) => _OrderService.GetUserOrders(UserName);

        [HttpGet("{id}"), ActionName("Get")]
        public OrderDTO GetOrderById(int id) => _OrderService.GetOrderById(id);

        [HttpPost("{UserName?}"), ActionName("Post")]
        public OrderDTO CreateOrder(CreateOrderModel OrderModel, string UserName) => _OrderService.CreateOrder(OrderModel, UserName);
    }
}
