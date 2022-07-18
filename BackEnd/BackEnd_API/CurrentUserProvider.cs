using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Http;

namespace BackEnd
{
    public class CurrentUserProvider : ICurrentUserProvider
    {
        public int UserId { get; set; }
        public User User { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            LoadUser();
        }

        private void LoadUser()
        {
            _httpContextAccessor.HttpContext.Items.TryGetValue("User", out var user);

            if (user is not User value) return;
            
            User = value;
            UserId = value.Id;
        }
    }
}