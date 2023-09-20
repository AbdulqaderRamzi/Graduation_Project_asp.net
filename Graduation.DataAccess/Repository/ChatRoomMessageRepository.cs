using Graduation.DataAccess.Data;
using Graduation.DataAccess.Repository.IRepository;
using Graduation.Models;

namespace Graduation.DataAccess.Repository
{
    public class ChatRoomMessageRepository : Repository<ChatRoomMessage>, IChatRoomMessageRepository
    {
        private ApplicationDbContext _db;
        public ChatRoomMessageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(ChatRoomMessage obj)
        {
            _db.ChatRoomMessages.Update(obj);
        }
    }
}
