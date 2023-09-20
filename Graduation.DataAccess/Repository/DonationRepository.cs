using Graduation.DataAccess.Data;
using Graduation.DataAccess.Repository.IRepository;
using Graduation.Models;

namespace Graduation.DataAccess.Repository
{
    public class DonationRepository : Repository<Donation>, IDonationRepository
    {
        private ApplicationDbContext _db;
        public DonationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Donation obj)
        {
            _db.Donations.Update(obj);
        }
    }
}
