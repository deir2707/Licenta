using System;
using Domain;

namespace Service
{
    public interface ICurrentUserProvider
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        void SetUser(User user);
    }
}