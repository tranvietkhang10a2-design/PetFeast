using Microsoft.AspNetCore.Mvc;
using PetFeast.Models.Interfaces;
using PetFeast.Models.Orders;

namespace PetFeast.Controllers
{
    public class OrderController : Controller
    {
        private readonly IShoppingCartRepository _cartRepo;
        private readonly IOrderRepository _orderRepo;
        public OrderController(
            IShoppingCartRepository cartRepo,
            IOrderRepository orderRepo)
        {
            _cartRepo = cartRepo;
            _orderRepo = orderRepo;
        }
        // HIỂN THỊ TRANG CHECKOUT
        public IActionResult CheckOut()
        {
            var cart = _cartRepo.GetCart()
                   .Where(x => x.IsSelected)
                   .ToList();
            if (cart.Count == 0)
            {
                return RedirectToAction(
                    "Index",
                    "ShoppingCart");
            }
            return View(cart);
        }
        // LƯU ĐƠN HÀNG
        [HttpPost]
        public IActionResult CheckOut(
            Order order,
            string city,
            string ward)
        {
            var cart = _cartRepo.GetCart()
                   .Where(x => x.IsSelected)
                   .ToList();
            if (cart.Count == 0)
            {
                return RedirectToAction(
                    "Index",
                    "ShoppingCart");
            }
            // Ghép địa chỉ
            order.Address = city + ", " + ward + ", " + order.Address;
            // Tổng tiền
            order.TotalAmount = cart.Sum(x => x.TotalPrice);
            // OrderDetail
            order.OrderDetails = new List<OrderDetail>();
            foreach (var item in cart)
            {
                order.OrderDetails.Add(
                    new OrderDetail
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    }
                );
            }
            _orderRepo.Add(order);
            _orderRepo.Save();
            _cartRepo.ClearCart();
            return RedirectToAction(
                nameof(CheckOutComplete));
        }
        public IActionResult CheckOutComplete()
        {
            return View();
        }
    }
}
