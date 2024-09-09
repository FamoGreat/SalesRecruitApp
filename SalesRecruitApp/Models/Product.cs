using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesRecruitApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }
        public int ProductBuddleId { get; set; }
        [ValidateNever]
        [ForeignKey(("ProductBuddleId"))]
        public ProductBuddle ProductBuddle { get; set; }
    }
}
