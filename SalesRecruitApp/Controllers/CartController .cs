using Microsoft.AspNetCore.Mvc;
using SalesRecruitApp.Models;
using SalesRecruitApp.Models.ViewModels;
using SalesRecruitApp.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRecruitApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Cart/Index
        public async Task<IActionResult> Index(int productBuddleId, int quantity)
        {
            var productBundle = await _unitOfWork.ProductBundles.GetAsync(pb => pb.Id == productBuddleId);

            if (productBundle == null)
            {
                return NotFound();
            }
            var cartViewModelList = new List<CartItemViewModel>
            {
                new CartItemViewModel
                {
                ProductBuddleId = productBundle.Id,
                ProductName = productBundle.Name,
                Price = productBundle.Price,
                Quantity = quantity,
                Total = quantity * productBundle.Price,
                ImageUrl = productBundle.ImageUrl
                }
      
            };

            return View(cartViewModelList);
        }


        // GET: Cart/Checkout
        public IActionResult Checkout()
        {
            return View();
        }
    }
}
