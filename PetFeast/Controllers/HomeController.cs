using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetFeast.Models;
using PetFeast.Models.Interfaces;
using System.Diagnostics;

namespace PetFeast.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;

        public HomeController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            var products = _productRepository.GetAll();

            ViewBag.FeaturedProducts =
                _productRepository.GetBestSellingProducts(8);

            return View(products);
        }
        public IActionResult Discount()
        {
            var products = _productRepository
                .GetAll()
                .Where(x => x.DiscountPercent > 0)
                .ToList();


            return View(products);
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
