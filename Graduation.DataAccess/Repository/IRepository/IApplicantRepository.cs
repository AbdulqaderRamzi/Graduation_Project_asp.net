using Graduation.Models;

namespace Graduation.DataAccess.Repository.IRepository
{
    public interface IApplicantRepository : IRepository<Applicant>
    {
        void Update(Applicant obj);
    }
}
