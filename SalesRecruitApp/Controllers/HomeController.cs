using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesRecruitApp.Models;
using SalesRecruitApp.Models.ViewModels;
using SalesRecruitApp.Repositories;
using System.Diagnostics;

namespace SalesRecruitApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var productBundles = await _unitOfWork.ProductBundles.GetAllAsync();
            var testimonials = await _unitOfWork.Testimonials.GetAllAsync();

            var viewModel = new HomeViewModel
            {
                ProductBuddles = productBundles,
                Testimonials = testimonials
            };
            return View(viewModel);
        }

        public IActionResult ProductBundle()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var productBundle = await _unitOfWork.ProductBundles.GetAsync(pb => pb.Id == id);

            if (productBundle == null)
            {
                return NotFound();
            }
            
            var products = await _unitOfWork.Products.GetAllAsync(p => p.ProductBuddleId == id);
            var testimonials = await _unitOfWork.Testimonials.GetAllAsync(t => t.ProductBuddleId == id);
            var faqs = await _unitOfWork.FAQs.GetAllAsync(f => f.ProductBuddleId == id);

            var viewModel = new SalesViewModel
            {
                ProductBuddle = productBundle,
                Products = products.ToList(),
                Testimonials = testimonials.ToList(),
                FAQs = faqs.ToList()
            };

            return View(viewModel);
        }


        public IActionResult Privacy()
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
