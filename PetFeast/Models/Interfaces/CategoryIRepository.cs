using PetFeast.Models.Products;

namespace PetFeast.Models.Interfaces
{
    public interface CategoryIRepository
    {
        IEnumerable<Category> GetAll();

        Category? GetById(int id);

        void Add(Category category);

        void Update(Category category);

        void Delete(int id);

        void Save();
    }
}
