using Graduation.Models;

namespace Graduation.DataAccess.Repository.IRepository
{
    public interface ISubscriptionRepository : IRepository<Subscription>
    {
        void Update(Subscription obj);
    }
}
