using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesRecruitApp.Models
{
    public class FAQ
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int ProductBuddleId { get; set; }
        [ValidateNever]
        [ForeignKey(("ProductBuddleId"))]
        public ProductBuddle ProductBuddle { get; set; }
    }
}
