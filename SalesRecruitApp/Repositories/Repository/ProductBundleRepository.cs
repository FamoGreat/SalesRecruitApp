using Microsoft.EntityFrameworkCore;
using SalesRecruitApp.Data;
using SalesRecruitApp.Models;
using SalesRecruitApp.Repositories.IRepository;
using SalesRecruitApp.Repositories.Repository;

namespace SalesRecruitApp.Repositories.Repository
{
    public class ProductBundleRepository : Repository<ProductBuddle>, IProductBundleRepository
    {
        public ProductBundleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public void UpdateAsync(ProductBuddle productBuddle)
        {
            _dbContext.Update(productBuddle);
        }
    }
}