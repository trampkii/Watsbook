using System;
namespace Watsbook_Android.Dtos.Requests
{
    public class ChangePhotoRequest
    {
        public byte[] File { get; set; }
        public int UserId { get; set; }
    }
}
