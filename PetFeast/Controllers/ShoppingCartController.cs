using Microsoft.AspNetCore.Mvc;
using PetFeast.Models.Interfaces;
using PetFeast.Models.ShoppingCart;

namespace PetFeast.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartRepository _cartRepo;
        private readonly IProductRepository _productRepo;
        public ShoppingCartController(
            IShoppingCartRepository cartRepo,
            IProductRepository productRepo)
        {
            _cartRepo = cartRepo;
            _productRepo = productRepo;
        }
        public IActionResult Index()
        {
            var cart =
                _cartRepo.GetCart();
            return View(cart);
        }
        [HttpPost]
        public IActionResult AddToCart( int productId, int quantity)
        {
            var product =
                _productRepo.GetById(productId);
            if (product == null)
            {
                return NotFound();
            }
            if (product.Quantity <= 0)
            {
                TempData["Error"] = "Sản phẩm hiện đã hết hàng";
                return RedirectToAction(
                    "Detail",
                    "Product",
                    new { id = productId });
            }
            if (quantity > product.Quantity)
            {
                TempData["Error"] = $"Chỉ còn {product.Quantity} sản phẩm trong kho";
                return RedirectToAction(
                    "Detail",
                    "Product",
                    new { id = productId });
            }
            var item = new ShoppingCartItem
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                Quantity = quantity
            };
            var cart = _cartRepo.GetCart();

            var exist = cart.FirstOrDefault(
                x => x.ProductId == productId);

            if (exist != null)
            {
                if (exist.Quantity + quantity > product.Quantity)
                {
                    TempData["Error"] =
                        $"Chỉ còn {product.Quantity} sản phẩm trong kho";

                    return RedirectToAction(
                        "Detail",
                        "Product",
                        new { id = productId });
                }
            }

            _cartRepo.AddToCart(item);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int id)
        {
            _cartRepo.Remove(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult UpdateCart( int productId, int quantity)
        {
            var product =
                _productRepo.GetById(productId);

            if (product == null)
            {
                return NotFound();
            }

            if (quantity > product.Quantity)
            {
                quantity = product.Quantity;
            }

            if (quantity < 1)
            {
                quantity = 1;
            }

            _cartRepo.UpdateQuantity(
                productId,
                quantity);

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult UpdateSelect(int productId, bool isSelected)
        {

            _cartRepo.UpdateSelect(
                productId,
                isSelected);


            return Ok();

        }
    }
}
