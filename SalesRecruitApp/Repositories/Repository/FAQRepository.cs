using SalesRecruitApp.Data;
using SalesRecruitApp.Models;
using SalesRecruitApp.Repositories.IRepository;

namespace SalesRecruitApp.Repositories.Repository
{
    public class FAQRepository : Repository<FAQ>, IFAQRepository
    {
        public FAQRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public void UpdateAsync(FAQ faq)
        {
            _dbContext.Update(faq);
        }
    }
}
