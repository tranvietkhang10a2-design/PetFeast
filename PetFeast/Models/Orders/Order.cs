using System.ComponentModel.DataAnnotations;

namespace PetFeast.Models.Orders
{
    public class Order
    {
        public int OrderId { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
