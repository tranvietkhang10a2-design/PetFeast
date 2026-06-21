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
        public IActionResult AddToCart(
            int productId,
            int quantity)
        {
            var product =
                _productRepo.GetById(productId);
            if (product == null)
            {
                return NotFound();
            }
            var item = new ShoppingCartItem
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                Quantity = quantity
            };
            _cartRepo.AddToCart(item);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int id)
        {
            _cartRepo.Remove(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult Update(
            int productId,
            int quantity)
        {
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
