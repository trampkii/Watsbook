using System;
using Newtonsoft.Json;

namespace Watsbook_Android.Dtos.Requests
{
    public class UserRegisterRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        public UserRegisterRequest()
        {
            DateOfBirth = DateTime.UtcNow.AddYears(-18);
        }
    }
}
