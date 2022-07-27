using System;
using System.Collections.Generic;
using Domain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Repository
{
    public class AuctionContext : DbContext
    {
        public AuctionContext(DbContextOptions<AuctionContext> options) : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auction>().HasKey(a => a.Id);
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Bid>().HasKey(b => b.Id);
            modelBuilder.Entity<Address>().HasKey(a => a.Id);
            modelBuilder.Entity<Image>().HasKey(i => i.Id);
            
            modelBuilder.Entity<Image>()
                .HasOne(i => i.Auction)
                .WithMany(a => a.Images)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Bid>()
                .HasOne(i => i.Auction)
                .WithMany(a => a.Bids)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Bid>()
                .HasOne(b=>b.Bidder)
                .WithMany(u=>u.Bids)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Auction>()
                .HasOne(a => a.Seller)
                .WithMany(u => u.Auctions)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Auction>()
                .Property(a => a.OtherDetails)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<IDictionary<string, string>>(v));
            
            modelBuilder.Entity<User>()
                .HasData(new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                    Email = "user1@email.com",
                    Password = "password",
                    FullName = "User1"
                },
                new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    Email = "user2@email.com",
                    Password = "password2",
                    FullName = "User2"
                });
        }
    }
}