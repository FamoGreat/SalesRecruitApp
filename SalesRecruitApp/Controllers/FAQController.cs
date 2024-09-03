using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalesRecruitApp.Models.ViewModels;
using SalesRecruitApp.Models;
using SalesRecruitApp.Repositories;

namespace SalesRecruitApp.Controllers
{
    public class FAQController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FAQController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var faqs = await _unitOfWork.FAQs.GetAllAsync(includeProperties: "ProductBuddle");
            return View(faqs);
        }

        public async Task<IActionResult> Create()
        {
            FAQViewModel faqViewModel = new()
            {
                ProductBundleList = (await _unitOfWork.ProductBundles.GetAllAsync()).Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                FAQ = new FAQ()
            };

            return View(faqViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FAQViewModel faqViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.FAQs.AddAsync(faqViewModel.FAQ);
                    await _unitOfWork.SaveAsync();
                    TempData["success"] = "FAQ created successfully";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    TempData["error"] = $"An error occurred while creating the FAQ: {ex.Message}";
                }

            }
            else
            {
                faqViewModel.ProductBundleList = (await _unitOfWork.ProductBundles.GetAllAsync()).Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            }
            return View(faqViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var faq = await _unitOfWork.FAQs.GetAsync(f => f.Id == id);
            if (faq == null)
            {
                TempData["error"] = "FAQ not found.";
                return RedirectToAction(nameof(Index));
            }

            FAQViewModel faqViewModel = new()
            {
                ProductBundleList = (await _unitOfWork.ProductBundles.GetAllAsync()).Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                FAQ = faq
            };

            return View(faqViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FAQViewModel faqViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var faqToBeEdited = await _unitOfWork.FAQs.GetAsync(t => t.Id == faqViewModel.FAQ.Id);

                    if (faqToBeEdited == null)
                    {
                        TempData["error"] = "FAQ not found.";
                        return RedirectToAction(nameof(Index));
                    }

                    // Update the fields
                    faqToBeEdited.ProductBuddleId = faqViewModel.FAQ.ProductBuddleId;
                    faqToBeEdited.Question = faqViewModel.FAQ.Question;
                    faqToBeEdited.Answer = faqViewModel.FAQ.Answer;

                    _unitOfWork.FAQs.UpdateAsync(faqToBeEdited);
                    await _unitOfWork.SaveAsync();

                    TempData["success"] = "FAQ updated successfully";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    TempData["error"] = "An error occurred while updating the FAQ. Please try again.";
                }
            }

            // If model state is invalid, repopulate the Product Bundles list
            faqViewModel.ProductBundleList = (await _unitOfWork.ProductBundles.GetAllAsync()).Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

            return View(faqViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var faq = await _unitOfWork.FAQs.GetAsync(f => f.Id == id);
            if (faq == null)
            {
                return NotFound();
            }
            return View(faq);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var faq = await _unitOfWork.FAQs.GetAsync(f => f.Id == id);
            if (faq == null)
            {
                return NotFound();
            }

            _unitOfWork.FAQs.RemoveAsync(faq);
            await _unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
