using System;

namespace API.DataTransferObjects
{
    public class DetailedPost
    {
        // Do zmapowania od AppUsera
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string UserPhotoUrl { get; set; }

        // Samo się zmapuje od Post
        public int Id { get; set; } 
        public string PhotoUrl { get; set; } = string.Empty; // Będzie dodane, jak utworzymy zdjęcia
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public string CloudinaryPublicId { get; set; }
    }
}