using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesRecruitApp.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        
    }
}
