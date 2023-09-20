using Graduation.Models;

namespace Graduation.DataAccess.Repository.IRepository
{
    public interface ICommunityRepository : IRepository<Community>
    {
        void Update(Community obj);
    }
}
