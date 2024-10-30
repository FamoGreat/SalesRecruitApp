using Microsoft.EntityFrameworkCore;
using SalesRecruitApp.Data;
using SalesRecruitApp.Models;
using SalesRecruitApp.Repositories.IRepository;

namespace SalesRecruitApp.Repositories.Repository
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }


        public void UpdateAsync(CartItem cartItem)
        {
            _dbContext.CartItems.Update(cartItem);
        }
    }
}
