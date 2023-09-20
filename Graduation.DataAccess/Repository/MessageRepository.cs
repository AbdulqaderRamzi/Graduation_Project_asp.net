using Graduation.DataAccess.Data;
using Graduation.DataAccess.Repository.IRepository;
using Graduation.Models;

namespace Graduation.DataAccess.Repository
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        private ApplicationDbContext _db;
        public MessageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Message obj)
        {
            _db.Messages.Update(obj);
        }
    }
}
