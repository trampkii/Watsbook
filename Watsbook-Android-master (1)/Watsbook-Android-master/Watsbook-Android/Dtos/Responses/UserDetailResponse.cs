using System;
using Newtonsoft.Json;

namespace Watsbook_Android.Dtos.Responses
{
    public class UserDetailResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("photoUrl")]
        public string PhotoUrl { get; set; }
    }
}
