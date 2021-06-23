using System;
using Newtonsoft.Json;

namespace Watsbook_Android.Dtos.Responses
{
    public class PostDetailResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("userPhotoUrl")]
        public string UserPhotoUrl { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("photoUrl")]
        public string PhotoUrl { get; set; } = string.Empty;

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("creationDate")]
        public DateTime CreationDate { get; set; }

        [JsonProperty("cloudinaryPublicId")]
        public string CloudinaryPublicId { get; set; }
    }
}
