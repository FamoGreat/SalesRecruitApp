using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SalesRecruitApp.Models.ViewModels
{
    public class FAQViewModel
    {
        public FAQ FAQ { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> ProductBundleList { get; set; }
    }
}
