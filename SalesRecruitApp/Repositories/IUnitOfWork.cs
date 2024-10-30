using SalesRecruitApp.Repositories.IRepository;

namespace SalesRecruitApp.Repositories
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        IProductBundleRepository ProductBundles { get; }
        ITestimonialRepository Testimonials { get; }
        IFAQRepository FAQs { get; }
        ICartRepository Carts { get; }
        ICartItemRepository CartItems { get; }


        Task SaveAsync();

    }
}
