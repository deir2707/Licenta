using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BackEnd.Middleware
{
    public class TimerMiddleware
    {
        private readonly RequestDelegate _next;
    
        public TimerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
    
        public async Task Invoke(HttpContext context)
        {
            var start = DateTime.Now;
            await _next(context);
            var end = DateTime.Now;
            var time = end - start;
            Console.WriteLine("Request time: " + time);
        }
    }
}