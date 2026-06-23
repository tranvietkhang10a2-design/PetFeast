using System.ComponentModel.DataAnnotations;

namespace PetFeast.Models.Orders
{
    public class Order
    {
        public int OrderId { get; set; }

        [Required]
        public string CustomerName { get; set; } = "";

        [Required]
        public string Phone { get; set; } = "";

        [Required]
        public string Address { get; set; } = "";
        public string DeliveryMethod { get; set; } = "";
        public string? Note { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal TotalAmount { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public List<OrderDetail>? OrderDetails { get; set; }
    }
}