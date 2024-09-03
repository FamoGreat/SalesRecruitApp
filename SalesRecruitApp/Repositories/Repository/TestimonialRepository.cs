using SalesRecruitApp.Data;
using SalesRecruitApp.Models;
using SalesRecruitApp.Repositories.IRepository;

namespace SalesRecruitApp.Repositories.Repository
{
    public class TestimonialRepository : Repository<Testimonial>, ITestimonialRepository
    {
        public TestimonialRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public void UpdateAsync(Testimonial testimonial)
        {
            _dbContext.Update(testimonial);
        }
    }
}
