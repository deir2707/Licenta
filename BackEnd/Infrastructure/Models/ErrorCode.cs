namespace Infrastructure.Models
{
    public enum ErrorCode
    {
        UserNotFound = 100,
        UserAlreadyExists = 101,
        InsufficientBalance = 102,
        AuctionNotFound = 200,
        AuctionEnded = 201,
        BidTooSmall = 300,
        BidOnOwnAuction = 301,
    }
    
}