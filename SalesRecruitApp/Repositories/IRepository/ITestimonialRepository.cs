using SalesRecruitApp.Models;

namespace SalesRecruitApp.Repositories.IRepository
{
    public interface ITestimonialRepository : IRepository<Testimonial>
    {
        void UpdateAsync(Testimonial testimonial);
    }
}
