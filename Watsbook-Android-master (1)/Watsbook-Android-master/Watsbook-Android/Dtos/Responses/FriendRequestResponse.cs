using System;
using Newtonsoft.Json;

namespace Watsbook_Android.Dtos.Responses
{
    public class FriendRequestResponse
    {
        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("photoUrl")]
        public string PhotoUrl { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }
    }
}
