using Microsoft.AspNetCore.Mvc;
using PetFeast.Models.Interfaces;
using PetFeast.Models.Products;
using PetFeast.Models.Services;
namespace PetFeast.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryIRepository _categoryRepo;

        public CategoryController(
            CategoryIRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public IActionResult Index()
        {
            return View(_categoryRepo.GetAll());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Add(category);
                _categoryRepo.Save();

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }
    }
}
