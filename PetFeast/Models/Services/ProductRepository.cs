using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetFeast.Data;
using PetFeast.Models.Products;
using PetFeast.Models.Interfaces;

namespace PetFeast.Models.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly PetFeastDBContext _context;

        public ProductRepository(PetFeastDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products
                           .Include(p => p.Category)
                           .ToList();
        }

        public Product? GetById(int id)
        {
            return _context.Products
                           .Include(p => p.Category)
                           .FirstOrDefault(p => p.ProductId == id);
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }

        public void Delete(int id)
        {
            var product = _context.Products.Find(id);

            if (product != null)
            {
                _context.Products.Remove(product);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
        public IEnumerable<Product> GetBestSellingProducts(int count)
        {
            return _context.OrderDetails
                .GroupBy(od => od.ProductId)
                .OrderByDescending(g => g.Sum(x => x.Quantity))
                .Take(count)
                .Select(g => g.First().Product)
                .ToList();
        }
    }
}
