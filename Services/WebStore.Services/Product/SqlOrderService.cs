using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.DTO.Orders;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Infrastructure.Services
{
    public class SqlOrderService : IOrderService
    {
        private readonly WebStoreContext _db;
        private readonly UserManager<User> _UserManager;

        public SqlOrderService(WebStoreContext db, UserManager<User> UserManager)
        {
            _db = db;
            _UserManager = UserManager;
        }

        public IEnumerable<OrderDTO> GetUserOrders(string UserName) => _db.Orders
           .Include(order => order.User)
           .Include(order => order.OrderItems)
           .Where(order => order.User.UserName == UserName)
           .Select(it => new OrderDTO()
           {
                Id = it.Id,
                Name = it.Name,
                Address = it.Address,
                Date = it.Date,
                Phone = it.Phone,
                OrderItems = it.OrderItems.Select(oi => new OrderItemDTO()
                {
                    Id = oi.Id,
                    Price = oi.Price,
                    Quantity = oi.Quantity
                }).ToList()
           })
           .ToArray();

        public OrderDTO GetOrderById(int id)
        {
            var order = _db.Orders
             .Include(o => o.OrderItems)
             .FirstOrDefault(o => o.Id == id);

            return order == null ? null : new OrderDTO()
            {
                Id = order.Id,
                Name = order.Name,
                Address = order.Address,
                Date = order.Date,
                Phone = order.Phone,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDTO()
                {
                    Id = oi.Id,
                    Price = oi.Price,
                    Quantity = oi.Quantity
                }).ToList()
            };
        }

        public OrderDTO CreateOrder(CreateOrderModel OrderModel, string UserName)
        {
            var user = _UserManager.FindByNameAsync(UserName).Result;

            using (var transaction = _db.Database.BeginTransaction())
            {
                var order = new Order
                {
                    Name = OrderModel.OrderViewModel.Name,
                    Address = OrderModel.OrderViewModel.Address,
                    Phone = OrderModel.OrderViewModel.Phone,
                    User = user,
                    Date = DateTime.Now
                };

                _db.Orders.Add(order);

                foreach (var item in OrderModel.OrderItems)
                {
                    var product = _db.Products.FirstOrDefault(p => p.Id == item.Id);
                    if(product is null)
                        throw new InvalidOperationException($"Товар с идентификатором id:{item.Id} отсутствует в БД");

                    var order_item = new OrderItem
                    {
                        Order = order,
                        Price = product.Price,
                        Quantity = item.Quantity,
                        Product = product
                    };

                    _db.OrderItems.Add(order_item);
                }

                _db.SaveChanges();
                transaction.Commit();
                return new OrderDTO()
                {
                    Id = order.Id,
                    Name = order.Name,
                    Address = order.Address,
                    Date = order.Date,
                    Phone = order.Phone,
                    OrderItems = order.OrderItems.Select(oi => new OrderItemDTO()
                    {
                        Id = oi.Id,
                        Price = oi.Price,
                        Quantity = oi.Quantity
                    }).ToList()
                }; ;
            }
        }
    }
}
