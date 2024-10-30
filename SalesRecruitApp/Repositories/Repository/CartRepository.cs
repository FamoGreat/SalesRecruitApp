using SalesRecruitApp.Data;
using SalesRecruitApp.Models;
using SalesRecruitApp.Repositories.IRepository;

namespace SalesRecruitApp.Repositories.Repository
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }


        public void UpdateAsync(Cart cart)
        {
            _dbContext.Carts.Update(cart);
        }
    }
}
