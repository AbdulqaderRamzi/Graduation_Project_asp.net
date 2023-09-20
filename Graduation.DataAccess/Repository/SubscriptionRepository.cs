using Graduation.DataAccess.Data;
using Graduation.DataAccess.Repository.IRepository;
using Graduation.Models;

namespace Graduation.DataAccess.Repository
{
    public class SubscriptionRepository : Repository<Subscription>, ISubscriptionRepository
    {
        private ApplicationDbContext _db;
        public SubscriptionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Subscription obj)
        {
            _db.Subscriptions.Update(obj);
        }
    }
}
