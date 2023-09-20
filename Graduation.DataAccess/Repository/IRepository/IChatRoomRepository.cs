using Graduation.Models;

namespace Graduation.DataAccess.Repository.IRepository
{
    public interface IChatRoomRepository : IRepository<ChatRoom>
    {
        void Update(ChatRoom obj);
    }
}
