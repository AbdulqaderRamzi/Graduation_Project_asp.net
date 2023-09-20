using Graduation.Models;

namespace Graduation.DataAccess.Repository.IRepository
{
    public interface IMessageRepository : IRepository<Message>
    {
        void Update(Message obj);
    }
}
