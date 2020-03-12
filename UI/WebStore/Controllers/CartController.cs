using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.DTO.Orders;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _CartService;
        public CartController(ICartService CartService) => _CartService = CartService;

        public IActionResult Details() => 
            View(new DetailsViewModel
            {
                CartViewModel = _CartService.TransformFromCart(),
                OrderViewModel = new OrderViewModel()
            });

        public IActionResult AddToCart(int id)
        {
            _CartService.AddToCart(id);
            return RedirectToAction("Details");
        }

        public IActionResult DecrimentFromCart(int id)
        {
            _CartService.DecrementFromCart(id);
            return RedirectToAction("Details");
        }

        public IActionResult RemoveFromCart(int id)
        {
            _CartService.RemoveFromCart(id);
            return RedirectToAction("Details");
        }

        public IActionResult RemoveAll()
        {
            _CartService.RemoveAll();
            return RedirectToAction("Details");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CheckOut(OrderViewModel Model, [FromServices] IOrderService OrderService)
        {
            if (!ModelState.IsValid)
                return View(nameof(Details), new DetailsViewModel
                {
                    CartViewModel = _CartService.TransformFromCart(),
                    OrderViewModel = Model
                });

            var create_order_model = new CreateOrderModel()
            {
                OrderViewModel = Model,
                OrderItems = _CartService.TransformFromCart()
                .Items
                .Select(it => new OrderItemDTO()
                {
                    Id = it.Key.Id,
                    Price = it.Key.Price,
                    Quantity = it.Value
                })
                .ToList()
            };

            var order = OrderService.CreateOrder(create_order_model, User.Identity.Name);

            _CartService.RemoveAll();

            return RedirectToAction("OrderConfirmed", new { id = order.Id });
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }

        #region API

        public IActionResult GetCartView() => ViewComponent("Cart");

        public IActionResult AddToCartAPI(int id)
        {
            _CartService.AddToCart(id);
            return Json(new { id, message = $"Товар с id {id} был добавлен в корзину" });
        }

        public IActionResult DecrimentFromCartAPI(int id)
        {
            _CartService.DecrementFromCart(id);
            return Json(new { id, message = $"Количество товар с id {id} в корзине было уменьшено на 1" });
        }

        public IActionResult RemoveFromCartAPI(int id)
        {
            _CartService.RemoveFromCart(id);
            return Json(new { id, message = $"Товар с id {id} был удален из корзины" });
        }

        public IActionResult RemoveAllAPI()
        {
            _CartService.RemoveAll();
            return Json(new { message = "Все товары были удалены из корзины" });
        }

        #endregion
    }
}