using System;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Http;
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

        public async Task Invoke(HttpContext context, IRepository<User> userRepository)
        {
            var pathValue = context.Request.Path.Value;
            if (pathValue == null)
            {
                context.Response.StatusCode = 401;
                return;
            }

            if (context.Request.Method == "OPTIONS" ||
                pathValue.Contains("register") ||
                pathValue.Contains("login") ||
                pathValue.Contains("get-all") ||
                pathValue.Contains("hub"))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue("User-Id", out var token))
            {
                context.Response.StatusCode = 401;
                return;
            }

            if (!Guid.TryParse(token, out var userId))
            {
                context.Response.StatusCode = 401;
                return;
            }

            var user = await userRepository.FindByIdAsync(userId);

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