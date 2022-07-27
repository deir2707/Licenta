using System;
using Infrastructure.Mongo;

namespace Domain
{
    [BsonCollection("addresses")]
    public class Address : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
    }
}