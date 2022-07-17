using Domain;
using Service.Outputs;

namespace Service.Extensions
{
    public static class UserExtensions
    {
        public static UserDTO ToUserDto(this User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Balance = user.Balance
            };
        }
    }
}