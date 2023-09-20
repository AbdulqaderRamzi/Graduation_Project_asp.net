using Graduation.DataAccess.Data;
using Graduation.DataAccess.Repository.IRepository;
using Graduation.Models;

namespace Graduation.DataAccess.Repository
{
    public class SubscriptionTypeRepository : Repository<SubscriptionType>, ISubscriptionTypeRepository
    {
        private ApplicationDbContext _db;
        public SubscriptionTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(SubscriptionType obj)
        {
            _db.SubscriptionTypes.Update(obj);
        }
    }
}
