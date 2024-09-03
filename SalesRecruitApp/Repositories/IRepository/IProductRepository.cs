using SalesRecruitApp.Models;

namespace SalesRecruitApp.Repositories.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void UpdateAsync(Product product);
    }
}
