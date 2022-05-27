using System;
using System.Text.Json;

namespace Infrastructure.Models
{
    public class AuctionException: Exception
    {
        public AuctionException(ErrorCode errorCode, string message) : base(
            JsonSerializer.Serialize(new
            {
                StatusCode = (uint) errorCode,
                Message = message
            }))
        {
        }
    }
}