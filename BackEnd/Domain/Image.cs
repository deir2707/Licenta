using System;
using Infrastructure.Mongo;

namespace Domain
{
    [BsonCollection("images")]
    public class Image : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public Guid AuctionId { get; set; }

        public Auction Auction { get; set; }

        public string ImageFileName { get; set; }

        public string FileType { get; set; }

        public byte[] DataFiles { get; set; }
    }
}