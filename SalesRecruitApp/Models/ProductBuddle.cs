namespace SalesRecruitApp.Models
{
    public class ProductBuddle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Highlights { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? UsageInstructions { get; set; }
        public decimal Price { get; set; }
        public int? StockQuantity { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public virtual List<Product> Products { get; set; } = new List<Product>();
        public virtual List<FAQ> FAQs { get; set; } = new List<FAQ>();
        public virtual List<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
    }
}
