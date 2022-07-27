using System;
using Domain;
using Microsoft.AspNetCore.Http;
using Service;

namespace BackEnd
{
    public class CurrentUserProvider : ICurrentUserProvider
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public void SetUser(User user)
        {
            UserId = user.Id;
            User = user;
        }

        public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            LoadUser();
        }

        private void LoadUser()
        {
            _httpContextAccessor.HttpContext.Items.TryGetValue("User", out var user);
            
            if (user is not User value)
            {
                return;
            }
            
            User = value;
            UserId = value.Id;
        }
    }
}