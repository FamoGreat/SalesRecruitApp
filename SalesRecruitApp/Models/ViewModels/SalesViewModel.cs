namespace SalesRecruitApp.Models.ViewModels
{
    public class SalesViewModel
    {
        public ProductBuddle ProductBuddle { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public List<Testimonial> Testimonials { get; set; }  
        public List<FAQ> FAQs { get; set; }

    }
}
