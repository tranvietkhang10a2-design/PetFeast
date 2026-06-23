using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PetFeast.Models.ShoppingCart;
using PetFeast.Models.Interfaces;

namespace PetFeast.Models.Services
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly IHttpContextAccessor _httpContext;
        private const string CART_KEY = "ShoppingCart";
        public ShoppingCartRepository(
            IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        private void UpdateCartCount(List<ShoppingCartItem> cart)
        {
            var count = cart.Sum(x => x.Quantity);

            _httpContext.HttpContext.Session
                .SetInt32("CartCount", count);
        }
        public List<ShoppingCartItem> GetCart()
        {
            var session =
                _httpContext.HttpContext.Session;
            var cart =
                session.GetString(CART_KEY);
            if (cart == null)
            {
                return new List<ShoppingCartItem>();
            }
            return JsonConvert.DeserializeObject<List<ShoppingCartItem>>(cart)
                   ?? new List<ShoppingCartItem>();
        }
        private void SaveCart(List<ShoppingCartItem> cart)
        {
            _httpContext.HttpContext.Session
                .SetString(
                    CART_KEY,
                    JsonConvert.SerializeObject(cart)
                );

            UpdateCartCount(cart);
        }
        public void AddToCart(ShoppingCartItem item)
        {
            var cart = GetCart();
            var exist =
                cart.FirstOrDefault(
                    x => x.ProductId == item.ProductId);
            if (exist != null)
            {
                exist.Quantity += item.Quantity;
            }
            else
            {
                cart.Add(item);
            }
            SaveCart(cart);
        }
        public void Remove(int productId)
        {
            var cart = GetCart();
            var item =
                cart.FirstOrDefault(
                    x => x.ProductId == productId);
            if (item != null)
            {
                cart.Remove(item);
            }
            SaveCart(cart);
        }
        public void UpdateQuantity(
            int productId,
            int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(
                    x => x.ProductId == productId);
            if (item != null)
            {
                item.Quantity = quantity;
            }
            SaveCart(cart);
        }
        public void ClearCart()
        {
            SaveCart(
                new List<ShoppingCartItem>()
            );
        }
        public void UpdateSelect(int productId, bool isSelected)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(
                x => x.ProductId == productId);


            if (item != null)
            {
                item.IsSelected = isSelected;
            }


            SaveCart(cart);
        }
    }
}
