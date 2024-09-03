using Microsoft.AspNetCore.Mvc;
using SalesRecruitApp.Models;
using SalesRecruitApp.Repositories;

namespace SalesRecruitApp.Controllers
{
    public class ProductBuddleController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductBuddleController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var productBuddles = await _unitOfWork.ProductBundles.GetAllAsync();
            return View(productBuddles);
        }

        public IActionResult Create()
        {
            return View(new ProductBuddle());
        }


        [HttpPost]
        public async Task<IActionResult> Create(ProductBuddle productBuddle, IFormFile? imageFile, IFormFile? videoFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        if (imageFile.Length > 2 * 1024 * 1024)
                        {
                            ModelState.AddModelError("ImageFile", "Image file size should not exceed 2MB.");
                            return View(productBuddle);
                        }

                        string[] allowedImageExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
                        string imageExtension = Path.GetExtension(imageFile.FileName).ToLower();
                        if (!allowedImageExtensions.Contains(imageExtension))
                        {
                            ModelState.AddModelError("ImageFile", "Invalid image file format.");
                            return View(productBuddle);
                        }

                        string imageFileName = Guid.NewGuid().ToString() + imageExtension;
                        string imagePath = Path.Combine(wwwRootPath, "images/productBuddle");

                        if (!Directory.Exists(imagePath))
                        {
                            Directory.CreateDirectory(imagePath);
                        }

                        using (var fileStream = new FileStream(Path.Combine(imagePath, imageFileName), FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }

                        productBuddle.ImageUrl = $"/images/productBuddle/{imageFileName}";
                    }

                    if (videoFile != null && videoFile.Length > 0)
                    {
                        if (videoFile.Length > 50 * 1024 * 1024) // Check if file size exceeds 50MB
                        {
                            ModelState.AddModelError("VideoFile", "Video file size should not exceed 50MB.");
                            return View(productBuddle);
                        }

                        string[] allowedVideoExtensions = { ".mp4", ".webm", ".ogg" };
                        string videoExtension = Path.GetExtension(videoFile.FileName).ToLower();

                        if (!allowedVideoExtensions.Contains(videoExtension))
                        {
                            ModelState.AddModelError("VideoFile", "Invalid video file format.");
                            return View(productBuddle);
                        }

                        string videoFileName = Guid.NewGuid().ToString() + videoExtension;
                        string videoPath = Path.Combine(wwwRootPath, "videos/productBuddle");

                        if (!Directory.Exists(videoPath))
                        {
                            Directory.CreateDirectory(videoPath);
                        }

                        using (var fileStream = new FileStream(Path.Combine(videoPath, videoFileName), FileMode.Create))
                        {
                            await videoFile.CopyToAsync(fileStream);
                        }

                        productBuddle.VideoUrl = $"/videos/productBuddle/{videoFileName}";
                    }

                    _unitOfWork.ProductBundles.AddAsync(productBuddle);
                    await _unitOfWork.SaveAsync();

                    TempData["success"] = "Product Buddle created successfully";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["error"] = $"An error occurred while creating the Product Buddle. Please try again. {ex.Message}";
                }
            }

            return View(productBuddle);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var productBuddle = await _unitOfWork.ProductBundles.GetAsync(pb => pb.Id == id);
            if (productBuddle == null)
            {
                return NotFound();
            }
            return View(productBuddle);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductBuddle productBuddle, IFormFile? file, IFormFile? videoFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch the category from the database
                    var productBuddleFromDb = await _unitOfWork.ProductBundles.GetAsync(pb => pb.Id == productBuddle.Id);

                    if (productBuddleFromDb == null)
                    {
                        TempData["error"] = "ProductBuddle not found.";
                        return RedirectToAction(nameof(Index));
                    }

                    // Update the category's basic details
                    productBuddleFromDb.Name = productBuddle.Name;
                    productBuddleFromDb.Description = productBuddle.Description;

                    string wwwRootPath = _webHostEnvironment.WebRootPath;

                    // Image upload handling
                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string imagePath = Path.Combine(wwwRootPath, "images/productBuddle");

                        if (!Directory.Exists(imagePath))
                        {
                            Directory.CreateDirectory(imagePath);
                        }

                        // Delete the old image if it exists
                        if (!string.IsNullOrEmpty(productBuddleFromDb.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, productBuddleFromDb.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Upload the new image
                        using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        productBuddleFromDb.ImageUrl = $"/images/productBuddle/{fileName}";
                    }

                    // Video upload handling
                    if (videoFile != null)
                    {
                        string videoFileName = Guid.NewGuid().ToString() + Path.GetExtension(videoFile.FileName);
                        string videoPath = Path.Combine(wwwRootPath, "videos/productBuddle");

                        if (!Directory.Exists(videoPath))
                        {
                            Directory.CreateDirectory(videoPath);
                        }

                        // Delete the old video if it exists
                        if (!string.IsNullOrEmpty(productBuddleFromDb.VideoUrl))
                        {
                            var oldVideoPath = Path.Combine(wwwRootPath, productBuddleFromDb.VideoUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldVideoPath))
                            {
                                System.IO.File.Delete(oldVideoPath);
                            }
                        }

                        // Upload the new video
                        using (var videoStream = new FileStream(Path.Combine(videoPath, videoFileName), FileMode.Create))
                        {
                            await videoFile.CopyToAsync(videoStream);
                        }

                        productBuddleFromDb.VideoUrl = $"/videos/productBuddle/{videoFileName}";
                    }

                    _unitOfWork.ProductBundles.UpdateAsync(productBuddleFromDb);
                    await _unitOfWork.SaveAsync();

                    TempData["success"] = "Product Buddle updated successfully";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    TempData["error"] = "An error occurred while updating the Product Buddle. Please try again.";
                }
            }

            return View(productBuddle);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var productBuddle = await _unitOfWork.ProductBundles.GetAsync(c => c.Id == id);
            if (productBuddle == null)
            {
                return NotFound();
            }
            return View(productBuddle);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // Fetch the category from the database
                var productBuddle = await _unitOfWork.ProductBundles.GetAsync(pb => pb.Id == id);

                if (productBuddle == null)
                {
                    return NotFound();
                }

                string wwwRootPath = _webHostEnvironment.WebRootPath;

                // Delete the associated image file if it exists
                if (!string.IsNullOrEmpty(productBuddle.ImageUrl))
                {
                    var imagePath = Path.Combine(wwwRootPath, productBuddle.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                // Delete the associated video file if it exists
                if (!string.IsNullOrEmpty(productBuddle.VideoUrl))
                {
                    var videoPath = Path.Combine(wwwRootPath, productBuddle.VideoUrl.TrimStart('/'));
                    if (System.IO.File.Exists(videoPath))
                    {
                        System.IO.File.Delete(videoPath);
                    }
                }

                _unitOfWork.ProductBundles.RemoveAsync(productBuddle);
                await _unitOfWork.SaveAsync();

                TempData["success"] = "Product Buddle deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["error"] = "An error occurred while deleting the Product Buddle. Please try again.";
                return View();
            }
        }
    }
}
