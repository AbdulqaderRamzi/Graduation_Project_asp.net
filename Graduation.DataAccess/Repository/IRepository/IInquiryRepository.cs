using Graduation.Models;

namespace Graduation.DataAccess.Repository.IRepository
{
    public interface IInquiryRepository : IRepository<Inquiry>
    {
        void Update(Inquiry obj);
    }
}
