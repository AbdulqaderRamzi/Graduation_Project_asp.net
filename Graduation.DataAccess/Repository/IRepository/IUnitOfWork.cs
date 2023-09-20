using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
        IChatRoomRepository ChatRoom { get; }
        IChatRoomMessageRepository ChatRoomMessage { get;}
        IMessageRepository Message { get; }
        ICommunityRepository Community { get; }
        IDonationRepository Donation { get; }
        IInquiryRepository Inquiry { get; }
        ISubscriptionRepository Subscription { get; }
        ISubscriptionTypeRepository SubscriptionType { get; }
        IApplicantRepository Applicant { get; }
        void Save();
    }
}
