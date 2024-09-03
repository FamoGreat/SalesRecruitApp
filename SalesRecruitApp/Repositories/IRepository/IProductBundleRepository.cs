using SalesRecruitApp.Models;
using SalesRecruitApp.Repositories.IRepository;

namespace SalesRecruitApp.Repositories.IRepository
{
    public interface IProductBundleRepository : IRepository<ProductBuddle>
    {
        void UpdateAsync(ProductBuddle productBuddle);
    }
}

