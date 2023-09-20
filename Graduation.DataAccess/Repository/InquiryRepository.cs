using Graduation.DataAccess.Data;
using Graduation.DataAccess.Repository.IRepository;
using Graduation.Models;

namespace Graduation.DataAccess.Repository
{
    public class InquiryRepository : Repository<Inquiry>, IInquiryRepository
    {
        private ApplicationDbContext _db;
        public InquiryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Inquiry obj)
        {
            _db.Inquiries.Update(obj);
        }
    }
}
