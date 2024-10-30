using System.ComponentModel.DataAnnotations.Schema;

namespace SalesRecruitApp.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public int ProductBuddleId { get; set; }
        [ForeignKey("ProductBuddleId")]
        public ProductBuddle ProductBuddle { get; set; }

        public int CartId { get; set; }
        [ForeignKey("CartId")]
        public Cart Cart { get; set; }
    }
}
