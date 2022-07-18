using Domain;

namespace Infrastructure
{
    public interface ICurrentUserProvider
    {
        public int UserId { get; set; }
        public User User { get; set; }
    }
}