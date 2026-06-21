using Microsoft.AspNetCore.Mvc;
using PetFeast.Data;
using PetFeast.Models.Orders;
using PetFeast.Models.Interfaces;

namespace PetFeast.Models.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly PetFeastDBContext _context;
        public OrderRepository(PetFeastDBContext context)
        {
            _context = context;
        }
        public void Add(Order order)
        {
            _context.Orders.Add(order);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
