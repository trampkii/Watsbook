using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContxt : DbContext
    {
        public DataContxt(DbContextOptions<DataContxt> options) : base(options) { }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<LikeIt> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<FriendRequest>()
                .HasKey(k => new { k.SenderId, k.RecipientId });

            builder.Entity<FriendRequest>()
                .HasOne(s => s.Sender)
                .WithMany(r => r.FriendRequestsSent)
                .HasForeignKey(s => s.SenderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<FriendRequest>()
                .HasOne(r => r.Recipient)
                .WithMany(s => s.FriendRequestsGotten)
                .HasForeignKey(r => r.RecipientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Friend>()
                .HasKey(k => new { k.UserId, k.SecondUserId });

            builder.Entity<Friend>()
                .HasOne(f => f.User)
                .WithMany(f => f.FriendsGotten)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Friend>()
                .HasOne(f => f.SecondUser)
                .WithMany(f => f.FriendsBeing)
                .HasForeignKey(f => f.SecondUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<LikeIt>().HasKey(k => new { k.UserId, k.PostId });

            builder.Entity<LikeIt>()
                .HasOne(u => u.User)
                .WithMany(l => l.LikedPosts)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<LikeIt>()
                .HasOne(p => p.Post)
                .WithMany(l => l.Likes)
                .HasForeignKey(p => p.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}