using Domain;
using Service.Outputs;

namespace Service.Extensions
{
    public static class UserExtensions
    {
        public static UserDetails ToUserDetails(this User user, bool includePrivate = true)
        {
            if (user == null)
                return null;
            
            return new UserDetails
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Balance = includePrivate ? user.Balance : 0
            };
        }
    }
}