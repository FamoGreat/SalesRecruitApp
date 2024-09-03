using SalesRecruitApp.Models;

namespace SalesRecruitApp.Repositories.IRepository
{
    public interface IFAQRepository : IRepository<FAQ>
    {
        void UpdateAsync(FAQ faq);
    }
}
