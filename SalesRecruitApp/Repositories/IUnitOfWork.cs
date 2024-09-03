using SalesRecruitApp.Repositories.IRepository;

namespace SalesRecruitApp.Repositories
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        IProductBundleRepository ProductBundles { get; }
        ITestimonialRepository Testimonials { get; }
        IFAQRepository FAQs { get; }
        Task SaveAsync();

    }
}
