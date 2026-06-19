using Microsoft.AspNetCore.Mvc;
using PetFeast.Data;
using PetFeast.Models.Products;
using PetFeast.Models.Interfaces;

namespace PetFeast.Models.Services
{
    public class CategoryRepository : CategoryIRepository
    {
        private readonly PetFeastDBContext _context;

        public CategoryRepository(PetFeastDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Categories.ToList();
        }

        public Category? GetById(int id)
        {
            return _context.Categories.Find(id);
        }

        public void Add(Category category)
        {
            _context.Categories.Add(category);
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }

        public void Delete(int id)
        {
            var category = _context.Categories.Find(id);

            if (category != null)
            {
                _context.Categories.Remove(category);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
