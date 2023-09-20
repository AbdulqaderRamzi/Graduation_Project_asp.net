using Graduation.DataAccess.Data;
using Graduation.DataAccess.Repository.IRepository;
using Graduation.Models;

namespace Graduation.DataAccess.Repository
{
    public class CommunityRepository : Repository<Community>, ICommunityRepository
    {
        private ApplicationDbContext _db;
        public CommunityRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Community obj)
        {
            _db.Communities.Update(obj);
        }
    }
}
