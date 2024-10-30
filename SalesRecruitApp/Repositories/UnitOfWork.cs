using SalesRecruitApp.Data;
using SalesRecruitApp.Models;
using SalesRecruitApp.Repositories.IRepository;
using SalesRecruitApp.Repositories.Repository;

namespace SalesRecruitApp.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            Products = new ProductRepository(_dbContext);
            ProductBundles = new ProductBundleRepository(_dbContext);
            FAQs = new FAQRepository(_dbContext);
            Testimonials = new TestimonialRepository(_dbContext);
            Carts = new CartRepository(_dbContext);
            CartItems = new CartItemRepository(_dbContext);

        }

        public IProductRepository Products { get; private set; }
        public IProductBundleRepository ProductBundles { get; private set; }
        public IFAQRepository FAQs { get; private set; }
        public ITestimonialRepository Testimonials { get; private set; }
        public ICartRepository Carts { get; private set; }
        public ICartItemRepository CartItems { get; private set; }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
