using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesRecruitApp.Models
{
    public class Testimonial
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string? Comment { get; set; }
        [ValidateNever]
        public string? ImageUrl { get; set; }
        public int ProductBuddleId { get; set; }
        [ValidateNever]
        [ForeignKey(("ProductBuddleId"))]
        public ProductBuddle ProductBuddle { get; set; }
    }
}
