namespace SalesRecruitApp.Models.ViewModels
{
    public class CartItemViewModel
    {
        public int ProductBuddleId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total {  get; set; }
        public string ImageUrl { get; set; }
    }
}
