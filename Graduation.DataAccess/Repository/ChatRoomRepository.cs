using Graduation.DataAccess.Data;
using Graduation.DataAccess.Repository.IRepository;
using Graduation.Models;

namespace Graduation.DataAccess.Repository
{
    public class ChatRoomRepository : Repository<ChatRoom>, IChatRoomRepository
    {
        private ApplicationDbContext _db;
        public ChatRoomRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(ChatRoom obj)
        {
            _db.ChatRooms.Update(obj);
        }
    }
}
