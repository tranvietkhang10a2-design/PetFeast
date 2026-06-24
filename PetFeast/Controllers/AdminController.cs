using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetFeast.Data;
using PetFeast.Models.Identity;
using PetFeast.Models.Interfaces;
using PetFeast.Models.Products;

namespace PetFeast.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly CategoryIRepository _categoryRepo;
        private readonly UserManager<ApplicationUser> _userManager;


        private readonly PetFeastDBContext _context;

        public AdminController(
         IProductRepository productRepo,
         CategoryIRepository categoryRepo,
         PetFeastDBContext context,
         UserManager<ApplicationUser> userManager)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Dashboard()
        {
            ViewBag.TotalProducts =
                _context.Products.Count();

            ViewBag.TotalCategories =
                _context.Categories.Count();

            ViewBag.TotalOrders =
                _context.Orders.Count();

            ViewBag.TotalRevenue =
                _context.Orders.Sum(x => (decimal?)x.TotalAmount) ?? 0;

            ViewBag.BestProducts =
                _productRepo.GetBestSellingProducts(5);

            return View();
        }

        // PRODUCT
        public IActionResult ProductList()
        {
            var products = _context.Products
                .Include(x => x.Category)
                .ToList();

            return View("Product/ProductList", products);
        }

        public IActionResult CreateProduct()
        {
            ViewBag.Categories =
                _categoryRepo.GetAll();

            return View("Product/CreateProduct");
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(x => x.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }


                ViewBag.Categories = _categoryRepo.GetAll();

                return View("Product/CreateProduct", product);
            }



            if (product.ImageFile != null)
            {
                string folder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/img/products");


                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);



                string fileName =
                    Guid.NewGuid() +
                    Path.GetExtension(product.ImageFile.FileName);



                string path =
                    Path.Combine(folder, fileName);



                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(stream);
                }



                product.ImageUrl =
                    "/img/products/" + fileName;
            }



            _productRepo.Add(product);

            _productRepo.Save();


            return RedirectToAction(nameof(ProductList));
        }

        public IActionResult EditProduct(int id)
        {
            var product =
                _productRepo.GetById(id);

            ViewBag.Categories =
                _categoryRepo.GetAll();

            return View("Product/EditProduct", product);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                var oldProduct = _productRepo.GetById(product.ProductId);

                if (oldProduct == null)
                    return NotFound();

                if (product.ImageFile != null)
                {
                    string folder = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot/img/products");

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    string fileName =
                        Guid.NewGuid() +
                        Path.GetExtension(product.ImageFile.FileName);

                    string path = Path.Combine(folder, fileName);

                    using (var stream =
                        new FileStream(path, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(stream);
                    }

                    oldProduct.ImageUrl =
                        "/img/products/" + fileName;
                }

                oldProduct.ProductName = product.ProductName;
                oldProduct.Price = product.Price;
                oldProduct.Quantity = product.Quantity;
                oldProduct.Description = product.Description;
                oldProduct.CategoryId = product.CategoryId;
                oldProduct.DiscountPercent = product.DiscountPercent;

                _productRepo.Save();

                return RedirectToAction(nameof(ProductList));
            }

            ViewBag.Categories = _categoryRepo.GetAll();

            return View("Product/EditProduct", product);
        }

        public IActionResult DeleteProduct(int id)
        {
            _productRepo.Delete(id);
            _productRepo.Save();

            return RedirectToAction(nameof(ProductList));
        }

        // CATEGORY

        public IActionResult CategoryList()
        {
            return View("Categories/CategoriesList", _categoryRepo.GetAll());
        }

        public IActionResult CreateCategory()
        {
            return View("Categories/CreateCategories");
        }

        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Add(category);
                _categoryRepo.Save();

                return RedirectToAction(nameof(CategoryList));
            }

            return View("Categories/CreateCategories", category);
        }

        public IActionResult EditCategory(int id)
        {
            return View("Categories/EditCategories", _categoryRepo.GetById(id));
        }

        [HttpPost]
        public IActionResult EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Update(category);
                _categoryRepo.Save();

                return RedirectToAction(nameof(CategoryList));
            }

            return View("Categories/EditCategories", category);
        }

        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
                return NotFound();

            bool hasProduct =
                _context.Products.Any(x => x.CategoryId == id);

            if (hasProduct)
            {
                TempData["Error"] =
                    "Danh mục đang chứa sản phẩm nên không thể xóa.";

                return RedirectToAction(nameof(CategoryList));
            }

            _categoryRepo.Delete(id);
            _categoryRepo.Save();

            return RedirectToAction(nameof(CategoryList));
        }
        public IActionResult OrderList()
        {
            var orders = _context.Orders
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return View("Order/OrderList", orders);
        }
        public IActionResult OrderDetail(int id)
        {
            var order = _context.Orders
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .FirstOrDefault(x => x.OrderId == id);

            if (order == null)
                return NotFound();

            return View("Order/OrderDetail", order);
        }
        public IActionResult ConfirmOrder(int id)
        {
            var order = _context.Orders.Find(id);

            if (order != null)
            {
                order.Status = "Đang giao";

                _context.SaveChanges();
            }

            return RedirectToAction(nameof(OrderList));
        }
        public IActionResult CompleteOrder(int id)
        {
            var order = _context.Orders.Find(id);

            if (order != null)
            {
                order.Status = "Hoàn thành";

                _context.SaveChanges();
            }

            return RedirectToAction(nameof(OrderList));
        }
        public IActionResult CancelOrder(int id)
        {
            var order = _context.Orders.Find(id);

            if (order != null)
            {
                order.Status = "Đã hủy";

                _context.SaveChanges();
            }

            return RedirectToAction(nameof(OrderList));
        }

        public IActionResult ProductSearch(string keyword)
        {

            var products = _context.Products
                .Include(x => x.Category)
                .Where(x => x.ProductName.Contains(keyword))
                .ToList();


            return View("Product/ProductList", products);

        }
    }
}