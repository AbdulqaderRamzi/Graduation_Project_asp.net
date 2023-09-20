using Graduation.Models;

namespace Graduation.DataAccess.Repository.IRepository
{
    public interface IChatRoomMessageRepository : IRepository<ChatRoomMessage>
    {
        void Update(ChatRoomMessage obj);
    }
}
