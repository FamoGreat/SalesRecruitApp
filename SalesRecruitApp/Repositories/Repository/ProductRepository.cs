using SalesRecruitApp.Data;
using SalesRecruitApp.Models;
using SalesRecruitApp.Repositories.IRepository;

namespace SalesRecruitApp.Repositories.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public void UpdateAsync(Product product)
        {
            _dbContext.Products.Update(product);
        }
    }
}
