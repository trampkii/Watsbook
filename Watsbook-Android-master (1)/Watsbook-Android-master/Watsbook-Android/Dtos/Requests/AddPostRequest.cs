using System;
namespace Watsbook_Android.Dtos.Requests
{
    public class AddPostRequest
    {
        public string Content { get; set; }
        public int UserId { get; set; }
        public byte[] File { get; set; }
    }
}
