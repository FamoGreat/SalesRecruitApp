using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalesRecruitApp.Models;
using SalesRecruitApp.Models.ViewModels;
using SalesRecruitApp.Repositories;

namespace SalesRecruitApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _unitOfWork.Products.GetAllAsync(includeProperties: "ProductBuddle");
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            ProductViewModel productViewModel = new()
            {
                ProductBundleList = (await _unitOfWork.ProductBundles.GetAllAsync()).Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };

            return View(productViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel productViewModel, IFormFile? imageFile)
        {



            if (ModelState.IsValid)
            {
                try
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;

                    // Handle image upload
                    if (imageFile != null)
                    {
                        string imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        string imagePath = Path.Combine(wwwRootPath, "images/product");

                        if (!Directory.Exists(imagePath))
                        {
                            Directory.CreateDirectory(imagePath);
                        }

                        using (var imageStream = new FileStream(Path.Combine(imagePath, imageFileName), FileMode.Create))
                        {
                            await imageFile.CopyToAsync(imageStream);
                        }

                        productViewModel.Product.ImageUrl = $"/images/product/{imageFileName}";
                    }


                    // Add product to database
                    _unitOfWork.Products.AddAsync(productViewModel.Product);
                    await _unitOfWork.SaveAsync();

                    TempData["success"] = "Product created successfully";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["error"] = $"An error occurred while creating the product: {ex.Message}";
                }
            }

            productViewModel.ProductBundleList = (await _unitOfWork.ProductBundles.GetAllAsync()).Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(productViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _unitOfWork.Products.GetAsync(p => p.Id == id);
            if (product == null)
            {
                TempData["error"] = "Product not found.";
                return RedirectToAction(nameof(Index));
            }
            var productBundles = await _unitOfWork.ProductBundles.GetAllAsync();

            var productBundleList = productBundles.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            var viewModel = new ProductViewModel
            {
                Product = product,
                ProductBundleList = productBundleList
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel viewModel, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch the product from the database
                    var product = await _unitOfWork.Products.GetAsync(p => p.Id == viewModel.Product.Id);

                    if (product == null)
                    {
                        TempData["error"] = "Product not found.";
                        return RedirectToAction(nameof(Index));
                    }

                    product.Name = viewModel.Product.Name;
                    product.Description = viewModel.Product.Description;
                    product.ProductBuddleId = viewModel.Product.ProductBuddleId;

                    string wwwRootPath = _webHostEnvironment.WebRootPath;

                    // Image upload handling
                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = Path.Combine(wwwRootPath, "images/product");

                        if (!Directory.Exists(productPath))
                        {
                            Directory.CreateDirectory(productPath);
                        }

                        // Delete the old image if it exists
                        if (!string.IsNullOrEmpty(product.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, product.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Upload the new image
                        using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        product.ImageUrl = $"/images/product/{fileName}";
                    }


                    _unitOfWork.Products.UpdateAsync(product);
                    await _unitOfWork.SaveAsync();

                    TempData["success"] = "Product updated successfully";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    TempData["error"] = "An error occurred while updating the product. Please try again.";
                }
            }

            viewModel.ProductBundleList = (await _unitOfWork.ProductBundles.GetAllAsync()).Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(viewModel);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.Products.GetAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var product = await _unitOfWork.Products.GetAsync(p => p.Id == id);
                if (product == null)
                {
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unitOfWork.Products.RemoveAsync(product);
                await _unitOfWork.SaveAsync();
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["error"] = "An error occurred while deleting the product. Please try again.";
                return View();
            }
        }


    }
}
