using Microsoft.AspNetCore.Mvc;
using PetFeast.Models.Interfaces;
using PetFeast.Models.Orders;

namespace PetFeast.Controllers
{
    public class OrderController : Controller
    {
        private readonly IShoppingCartRepository _cartRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IProductRepository _productRepo;
        public OrderController(
            IShoppingCartRepository cartRepo,
            IOrderRepository orderRepo,
            IProductRepository productRepo)
        {
            _cartRepo = cartRepo;
            _orderRepo = orderRepo;
            _productRepo = productRepo;
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
            if (order.DeliveryMethod == "Ship")
            {
                order.Address =
                    city + ", " +
                    ward + ", " +
                    order.Address;
            }
            else
            {
                order.Address =
                    "PetFeast - Đà Lạt, Lâm Đồng";
            }
            // Tổng tiền
            var productTotal = cart.Sum(x => x.TotalPrice);


            // Tính phí ship
            if (order.DeliveryMethod == "Ship")
            {
                if (productTotal >= 199000)
                {
                    order.ShippingFee = 0;
                }
                else
                {
                    order.ShippingFee = 20000;
                }
            }
            else
            {
                order.ShippingFee = 0;
            }
            // Tổng thanh toán
            order.TotalAmount = productTotal + order.ShippingFee;
            // OrderDetail
            order.OrderDetails = new List<OrderDetail>();
            foreach (var item in cart)
            {
                var product =
                    _productRepo.GetById(item.ProductId);

                if (product == null)
                {
                    return BadRequest();
                }

                if (product.Quantity < item.Quantity)
                {
                    TempData["Error"] =
                        $"Sản phẩm {product.ProductName} chỉ còn {product.Quantity} sản phẩm.";

                    return RedirectToAction(
                        "Index",
                        "ShoppingCart");
                }

                product.Quantity -= item.Quantity;

                order.OrderDetails.Add(
                    new OrderDetail
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    });
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
