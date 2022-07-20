using Domain;
using Service.Outputs;

namespace Service.Extensions
{
    public static class UserExtensions
    {
        public static UserDetails ToUserDetails(this User user)
        {
            return new UserDetails
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Balance = user.Balance
            };
        }
    }
}