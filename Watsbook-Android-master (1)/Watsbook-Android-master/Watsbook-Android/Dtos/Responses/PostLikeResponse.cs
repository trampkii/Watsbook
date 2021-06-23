using System;
using Newtonsoft.Json;

namespace Watsbook_Android.Dtos.Responses
{
    public class PostLikeResponse
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("postId")]
        public int PostId { get; set; }
    }
}
