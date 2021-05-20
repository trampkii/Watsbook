using Microsoft.AspNetCore.Http;

namespace API.DataTransferObjects
{
    public class AvatarForChange
    {
        public int UserId { get; set; } 
        public IFormFile File { get; set; }
    }
}