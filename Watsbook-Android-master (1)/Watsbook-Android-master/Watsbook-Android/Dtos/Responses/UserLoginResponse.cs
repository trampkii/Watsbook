using System;
using Newtonsoft.Json;

namespace Watsbook_Android.Dtos.Responses
{
    public class UserLoginResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("user")]
        public UserDetailResponse User { get; set; }
    }
}
