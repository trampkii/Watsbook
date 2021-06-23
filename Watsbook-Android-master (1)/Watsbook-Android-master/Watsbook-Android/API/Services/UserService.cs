using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Watsbook_Android.API.Helpers;
using Watsbook_Android.Dtos.Requests;

namespace Watsbook_Android.API.Services
{
    public class UserService
    {
        private readonly string BaseAddress = $"{ApiUrls.BaseURL}{ApiUrls.Users}";

        public async Task<string> CheckIfAreFriends(int userId)
        {
            var response = await SendGetFriendEntity(userId);

            return RequestHelper.GetResponseContent(response);
        }

        public async Task<string> RemoveFriendAsync(int id)
        {
            var response = await SendRemoveFriendAsync(id);

            return RequestHelper.GetResponseContent(response);
        }

        public async Task<string> GetFriendsAsync(int id)
        {
            var response = await SendGetFriendsAsync(id);

            return RequestHelper.GetResponseContent(response);
        }

        public async Task<string> AcceptRequestAsync(int id)
        {
            var response = await SendAcceptRequestAsync(id);

            return RequestHelper.GetResponseContent(response);
        }

        public async Task<string> DeclineAsync(int id)
        {
            var response = await SendDeclineRequestAsync(id);

            return RequestHelper.GetResponseContent(response);
        }

        public async Task<string> GetPendingRequestsAsync()
        {
            var response = await SendGetPendingAsync();

            return RequestHelper.GetResponseContent(response);
        }

        public async Task<string> InviteUserAsync(int userId)
        {
            var response = await SendInviteUserAsync(userId);

            return RequestHelper.GetResponseContent(response);
        }

        public async Task<string> ChangePhotoAsync(ChangePhotoRequest request)
        {
            var response = await SendAddPhotoRequest(request);

            return RequestHelper.GetResponseContent(response);
        }

        public async Task<string> GetUserAsync(int userId)
        {
            var response = await SendGetUserAsync(userId);

            return RequestHelper.GetResponseContent(response);
        }

        public async Task<string> GetSearchUsersAsync(string keyWord)
        {
            var response = await SendWithQueryAsync(keyWord);

            return RequestHelper.GetResponseContent(response);
        }

        private async Task<HttpResponseMessage> SendGetFriendEntity(int id)
        {
            using var client = new HttpClient();

            RequestHelper.AddAuthorizationHeader(client);

            var response = await client.GetAsync($"{BaseAddress}/relation/{id}");

            return response;
        }

        private async Task<HttpResponseMessage> SendRemoveFriendAsync(int id)
        {
            using var client = new HttpClient();

            RequestHelper.AddAuthorizationHeader(client);

            var response = await client.DeleteAsync($"{BaseAddress}/friends/{id}");

            return response;
        }

        private async Task<HttpResponseMessage> SendGetFriendsAsync(int id)
        {
            using var client = new HttpClient();

            RequestHelper.AddAuthorizationHeader(client);

            var response = await client.GetAsync($"{BaseAddress}/{id}/friends");

            return response;
        }

        private async Task<HttpResponseMessage> SendAcceptRequestAsync(int id)
        {
            using var client = new HttpClient();

            RequestHelper.AddAuthorizationHeader(client);

            var response = await client.PostAsync($"{BaseAddress}/invite/{id}/accept", null);

            return response;
        }

        private async Task<HttpResponseMessage> SendDeclineRequestAsync(int id)
        {
            using var client = new HttpClient();

            RequestHelper.AddAuthorizationHeader(client);

            var response = await client.DeleteAsync($"{BaseAddress}/invite/{id}/decline");

            return response;
        }

        private async Task<HttpResponseMessage> SendGetPendingAsync()
        {
            using var client = new HttpClient();

            RequestHelper.AddAuthorizationHeader(client);

            var response = await client.GetAsync($"{BaseAddress}/requests");

            return response;
        }

        private async Task<HttpResponseMessage> SendInviteUserAsync(int endpoint)
        {
            using var client = new HttpClient();

            RequestHelper.AddAuthorizationHeader(client);

            var response = await client.PostAsync($"{BaseAddress}/invite/{endpoint}", null);

            return response;
        }

        private async Task<HttpResponseMessage> SendAddPhotoRequest(ChangePhotoRequest request)
        {
            using var client = new HttpClient();
            using var content = new MultipartFormDataContent();

            RequestHelper.AddAuthorizationHeader(client);

            var imageContent = new ByteArrayContent(request.File);
            imageContent.Headers.ContentType =
                MediaTypeHeaderValue.Parse("image/jpeg");

            content.Add(imageContent, "File", "image.jpg");

            var response = await client.PostAsync($"{BaseAddress}", content);

            return response;
        }

        private async Task<HttpResponseMessage> SendWithQueryAsync(string endpoint)
        {
            using var client = new HttpClient();

            RequestHelper.AddAuthorizationHeader(client);

            var response = await client.GetAsync($"{BaseAddress}?KeyWord={endpoint}");

            return response;
        }

        private async Task<HttpResponseMessage> SendGetUserAsync(int endpoint)
        {
            using var client = new HttpClient();

            RequestHelper.AddAuthorizationHeader(client);

            var response = await client.GetAsync($"{BaseAddress}/{endpoint}");

            return response;
        }
    }
}
