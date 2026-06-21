namespace PetFeast.Models.ShoppingCart
{
    public class ShoppingCartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsSelected { get; set; } = true;
        public decimal TotalPrice => Price * Quantity;
    }
}
