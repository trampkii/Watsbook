using System;
using System.Collections.Generic;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string Location { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string PhotoUrl { get; set; }
        public string PhotoCloudinaryPublicId { get; set; }
        public ICollection<FriendRequest> FriendRequestsSent { get; set; }
        public ICollection<FriendRequest> FriendRequestsGotten { get; set; }
        public ICollection<Friend> FriendsBeing { get; set;}
        public ICollection<Friend> FriendsGotten { get; set; }
        public ICollection<Post> PostsCreated { get; set; }
        public ICollection<LikeIt> LikedPosts { get; set; }
    }
}