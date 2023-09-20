using Graduation.Models;

namespace Graduation.DataAccess.Repository.IRepository
{
    public interface IDonationRepository : IRepository<Donation>
    {
        void Update(Donation obj);
    }
}
