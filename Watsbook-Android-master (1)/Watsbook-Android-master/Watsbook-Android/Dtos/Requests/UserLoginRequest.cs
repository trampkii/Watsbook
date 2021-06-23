using System;
using Newtonsoft.Json;

namespace Watsbook_Android.Dtos.Requests
{
    public class UserLoginRequest
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
