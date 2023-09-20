using Graduation.DataAccess.Data;
using Graduation.DataAccess.Repository.IRepository;
using Graduation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.DataAccess.Repository
{
    internal class UserRepository : Repository<User>, IUserRepository
    {
        private ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(User obj)
        {
            _db.Users.Update(obj);
        }
    }
}
