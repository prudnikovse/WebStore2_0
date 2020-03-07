using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.DTO.Orders;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
//using WebStore.Services.Helpers;

namespace WebStore.Services
{
    public class SqlOrderService : IOrderService
    {
        private readonly WebStoreContext _db;
        private readonly UserManager<User> _UserManager;
        private readonly IMapper _Mapper;

        public SqlOrderService(WebStoreContext db, UserManager<User> UserManager, IMapper Mapper)
        {
            _db = db;
            _UserManager = UserManager;
            _Mapper = Mapper;
        }

        public IEnumerable<OrderDTO> GetUserOrders(string UserName)
        {
            var res = _db.Orders
           .Include(order => order.User)
           .Include(order => order.OrderItems)
           .Where(order => order.User.UserName == UserName)
           .Select(_Mapper.Map<OrderDTO>)
           .ToArray();

            return res;
        }

        public OrderDTO GetOrderById(int id)
        {
            var order = _db.Orders
             .Include(o => o.OrderItems)
             .FirstOrDefault(o => o.Id == id);

            return order == null ? null : _Mapper.Map<OrderDTO>(order);
        }

        public OrderDTO CreateOrder(CreateOrderModel OrderModel, string UserName)
        {
            var user = _UserManager.FindByNameAsync(UserName).Result;

            using (var transaction = _db.Database.BeginTransaction())
            {
                //var order = new Order
                //{
                //    Name = OrderModel.OrderViewModel.Name,
                //    Address = OrderModel.OrderViewModel.Address,
                //    Phone = OrderModel.OrderViewModel.Phone,
                //    User = user,
                //    Date = DateTime.Now
                //};

                var order = _Mapper.Map<Order>(OrderModel.OrderViewModel);

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

                return _Mapper.Map<OrderDTO>(order);

                //return WebStore.Services.Helpers.Mapper.Map(order, new OrderDTO()
                //{
                //    OrderItems = order.OrderItems.Select(oi => new OrderItemDTO()
                //    {
                //        Id = oi.Id,
                //        Price = oi.Price,
                //        Quantity = oi.Quantity
                //    }).ToList()
                //});
            }
        }
    }
}
