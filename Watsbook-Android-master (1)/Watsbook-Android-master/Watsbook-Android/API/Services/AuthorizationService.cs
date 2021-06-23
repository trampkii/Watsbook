using System;
using System.Net.Http;
using System.Threading.Tasks;
using Watsbook_Android.API.Helpers;
using Watsbook_Android.Dtos.Requests;

namespace Watsbook_Android.API.Services
{
    public class AuthorizationService
    {
        private readonly string BaseAddress = $"{ApiUrls.BaseURL}{ApiUrls.Auth}";

        public async Task<string> LoginAsync(UserLoginRequest request)
        {
            var data = RequestHelper.CreateJSONStringContent(request);

            var response = await SendRequestAsync(data, "/login");

            return RequestHelper.GetResponseContent(response);
        }

        public async Task<string> RegisterAsync(UserRegisterRequest request)
        {
            var data = RequestHelper.CreateJSONStringContent(request);

            var response = await SendRequestAsync(data, "/register");

            return RequestHelper.GetResponseContent(response);
        }

        private async Task<HttpResponseMessage> SendRequestAsync(StringContent data, string endpoint)
        {
            using var client = new HttpClient();

            var response = await client.PostAsync($"{BaseAddress}{endpoint}", data);

            return response;
        }
    }
}
