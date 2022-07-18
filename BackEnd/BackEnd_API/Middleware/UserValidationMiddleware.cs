using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace BackEnd.Middleware
{
    public class UserValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public UserValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, AuctionContext auctionContext)
        {
            if (context.Request.Method == "OPTIONS" ||
                context.Request.Path.Value.Contains("register") ||
                context.Request.Path.Value.Contains("login") ||
                context.Request.Path.Value.Contains("hub"))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue("User-Id", out var token))
            {
                context.Response.StatusCode = 401;
                return;
            }

            if (!int.TryParse(token, out var userId))
            {
                context.Response.StatusCode = 401;
                return;
            }

            var user = await auctionContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                context.Response.StatusCode = 401;
                return;
            }

            context.Items["User"] = user;
            await _next(context);
        }
    }
}