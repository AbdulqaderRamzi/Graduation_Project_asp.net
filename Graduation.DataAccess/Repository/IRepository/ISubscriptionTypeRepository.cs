using Graduation.Models;

namespace Graduation.DataAccess.Repository.IRepository
{
    public interface ISubscriptionTypeRepository : IRepository<SubscriptionType>
    {
        void Update(SubscriptionType obj);
    }
}
