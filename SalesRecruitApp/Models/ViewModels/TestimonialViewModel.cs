using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SalesRecruitApp.Models.ViewModels
{
    public class TestimonialViewModel
    {
        public Testimonial Testimonial { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> ProductBundleList { get; set; }
    }
}
