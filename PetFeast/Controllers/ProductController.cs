using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetFeast.Models.Interfaces;
using PetFeast.Models.Products;

namespace PetFeast.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly CategoryIRepository _categoryRepo;

        public ProductController(
            IProductRepository productRepo,
            CategoryIRepository categoryRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
        }

        public IActionResult Index()
        {
            var products = _productRepo.GetAll();

            return View(products);
        }

        public IActionResult Detail(int id)
        {
            var product = _productRepo.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public IActionResult Create()
        {
            ViewBag.Categories =
                new SelectList(
                    _categoryRepo.GetAll(),
                    "CategoryId",
                    "CategoryName");

            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepo.Add(product);
                _productRepo.Save();

                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }
    }
}
