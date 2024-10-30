using SalesRecruitApp.Models;

namespace SalesRecruitApp.Repositories.IRepository
{
    public interface ICartItemRepository : IRepository<CartItem>
    {
        void UpdateAsync(CartItem cartItem);
    }
}
