using Graduation.DataAccess.Data;
using Graduation.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IUserRepository User { get; set; }
        public IChatRoomRepository ChatRoom { get; }
        public IChatRoomMessageRepository ChatRoomMessage { get; set; }
        public IMessageRepository Message { get; }
        public ICommunityRepository Community { get; set; }
        public IDonationRepository Donation { get; set; }
        public IInquiryRepository Inquiry { get; set; }
        public ISubscriptionRepository Subscription { get; set; }
        public ISubscriptionTypeRepository SubscriptionType { get; set; }
        public IApplicantRepository Applicant { get; set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            User = new UserRepository(_db);
            ChatRoom = new ChatRoomRepository(_db);
            ChatRoomMessage = new ChatRoomMessageRepository(_db);
            Message = new MessageRepository(_db);
            Community = new CommunityRepository(_db);
            Donation = new DonationRepository(_db);
            Inquiry = new InquiryRepository(_db);
            Subscription = new SubscriptionRepository(_db);
            SubscriptionType = new SubscriptionTypeRepository(_db);
            Applicant = new ApplicantRepository(_db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
