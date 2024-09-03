using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalesRecruitApp.Models.ViewModels;
using SalesRecruitApp.Models;
using SalesRecruitApp.Repositories;

namespace SalesRecruitApp.Controllers
{
    public class TestimonialController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TestimonialController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var testimonial = await _unitOfWork.Testimonials.GetAllAsync(includeProperties: "ProductBuddle");
            return View(testimonial);
        }

        public async Task<IActionResult> Create()
        {
            TestimonialViewModel testimonialViewModel = new()
            {
                ProductBundleList = (await _unitOfWork.ProductBundles.GetAllAsync()).Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Testimonial = new Testimonial()
            };

            return View(testimonialViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Create(TestimonialViewModel testimonialViewModel, IFormFile file)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;

                    // Handle image upload
                    if (file != null)
                    {
                        string imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string imagePath = Path.Combine(wwwRootPath, "images/testimonial");

                        if (!Directory.Exists(imagePath))
                        {
                            Directory.CreateDirectory(imagePath);
                        }

                        using (var imageStream = new FileStream(Path.Combine(imagePath, imageFileName), FileMode.Create))
                        {
                            await file.CopyToAsync(imageStream);
                        }

                        testimonialViewModel.Testimonial.ImageUrl = $"/images/testimonial/{imageFileName}";
                    }


                    _unitOfWork.Testimonials.AddAsync(testimonialViewModel.Testimonial);
                    await _unitOfWork.SaveAsync();

                    TempData["success"] = "Testimonial created successfully";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["error"] = $"An error occurred while creating the testimonial: {ex.Message}";
                }
            }

            else
            {
                testimonialViewModel.ProductBundleList = (await _unitOfWork.ProductBundles.GetAllAsync()).Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            }
            return View(testimonialViewModel);

        }

        public async Task<IActionResult> Edit(int id)
        {
            var testimonial = await _unitOfWork.Testimonials.GetAsync(t => t.Id == id);

            if (testimonial == null)
            {
                TempData["error"] = "Testimonial not found.";
                return RedirectToAction(nameof(Index));
            }

            TestimonialViewModel testimonialViewModel = new()
            {
                ProductBundleList = (await _unitOfWork.ProductBundles.GetAllAsync()).Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Testimonial = testimonial
            };

            return View(testimonialViewModel);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(TestimonialViewModel testimonialViewModel, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch the testimonial from the database
                    var testimonialToBeEdited = await _unitOfWork.Testimonials.GetAsync(t => t.Id == testimonialViewModel.Testimonial.Id);

                    if (testimonialToBeEdited == null)
                    {
                        TempData["error"] = "Testimonial not found.";
                        return RedirectToAction(nameof(Index));
                    }

                    testimonialToBeEdited.CustomerName = testimonialViewModel.Testimonial.CustomerName;
                    testimonialToBeEdited.Comment = testimonialViewModel.Testimonial.Comment;
                    testimonialToBeEdited.ProductBuddleId = testimonialViewModel.Testimonial.ProductBuddleId;

                    // Image upload handling
                    if (file != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string testimonialPath = Path.Combine(wwwRootPath, "images/testimonial");

                        if (!Directory.Exists(testimonialPath))
                        {
                            Directory.CreateDirectory(testimonialPath);
                        }

                        // Delete the old image if it exists
                        if (!string.IsNullOrEmpty(testimonialToBeEdited.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, testimonialToBeEdited.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Upload the new image
                        using (var fileStream = new FileStream(Path.Combine(testimonialPath, fileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        testimonialToBeEdited.ImageUrl = $"/images/testimonial/{fileName}";
                    }

                    _unitOfWork.Testimonials.UpdateAsync(testimonialToBeEdited);
                    await _unitOfWork.SaveAsync();

                    TempData["success"] = "Testimonial updated successfully";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    TempData["error"] = "An error occurred while updating the testimonial. Please try again.";
                }
            }

            testimonialViewModel.ProductBundleList = (await _unitOfWork.ProductBundles.GetAllAsync()).Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(testimonialViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var testimonial = await _unitOfWork.Testimonials.GetAsync(t => t.Id == id);
            if (testimonial == null)
            {
                TempData["error"] = "Testimonial not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(testimonial);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var testimonial = await _unitOfWork.Testimonials.GetAsync(t => t.Id == id);
                if (testimonial == null)
                {
                    TempData["error"] = "Testimonial not found.";
                    return RedirectToAction(nameof(Index));
                }

                if (!string.IsNullOrEmpty(testimonial.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, testimonial.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unitOfWork.Testimonials.RemoveAsync(testimonial);
                await _unitOfWork.SaveAsync();
                TempData["success"] = "Testimonial deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["error"] = "An error occurred while deleting the testimonial. Please try again.";
                return View();
            }
        }

    }
}
