using SalesRecruitApp.Models;

namespace SalesRecruitApp.Repositories.IRepository
{
    public interface ICartRepository : IRepository<Cart>
    {
        void UpdateAsync(Cart cart);

    }
}
