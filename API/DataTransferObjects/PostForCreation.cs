using System;
using Microsoft.AspNetCore.Http;

namespace API.DataTransferObjects
{
    public class PostForCreation
    {
        public string PhotoUrl { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public string CloudinaryPublicId { get; set; }
        public IFormFile File { get; set; }
        public int UserId { get; set; }
        public PostForCreation()
        {
            CreationDate = DateTime.UtcNow;
        }
    }
}