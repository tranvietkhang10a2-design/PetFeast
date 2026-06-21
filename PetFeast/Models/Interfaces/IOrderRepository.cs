using PetFeast.Models.Orders;

namespace PetFeast.Models.Interfaces
{
    public interface IOrderRepository
    {
        void Add(Order order);
        void Save();
    }
}
