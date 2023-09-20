using Graduation.DataAccess.Data;
using Graduation.DataAccess.Repository.IRepository;
using Graduation.Models;

namespace Graduation.DataAccess.Repository
{
    public class ApplicantRepository : Repository<Applicant>, IApplicantRepository
    {
        private ApplicationDbContext _db;
        public ApplicantRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Applicant obj)
        {
            _db.Applicants.Update(obj);
        }
    }
}
