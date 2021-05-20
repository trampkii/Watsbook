using System;
using System.Collections.Generic;

namespace API.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }  
        public string CloudinaryPublicId { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public ICollection<LikeIt> Likes { get; set; }
    }
}