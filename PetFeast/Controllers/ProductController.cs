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

        public IActionResult Index( string? keyword, int? categoryId, decimal? maxPrice, string? sortOrder)
        {
            var products = _productRepo.GetAll();

            if (!string.IsNullOrEmpty(keyword))
            {
                products = products.Where(p =>
                    p.ProductName.Contains(keyword,
                    StringComparison.OrdinalIgnoreCase));
            }

            if (categoryId.HasValue)
            {
                products = products.Where(p =>
                    p.CategoryId == categoryId.Value);
            }

            if (maxPrice.HasValue && maxPrice > 0)
            {
                products = products.Where(p =>
                    p.Price <= maxPrice.Value);
            }

            switch (sortOrder)
            {
                case "price_asc":
                    products = products.OrderBy(p => p.Price);
                    break;

                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
            }

            ViewBag.Keyword = keyword;
            ViewBag.Categories = _categoryRepo.GetAll();

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
