using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Watsbook_Android.API.Helpers;
using Watsbook_Android.Dtos.Requests;

namespace Watsbook_Android.API.Services
{
    public class PostService
    {
        private readonly string BaseAddress = $"{ApiUrls.BaseURL}{ApiUrls.Posts}";

        public async Task<string> SendPostAsync(AddPostRequest request)
        {
            var response = await SendAddPostRequest(request);

            return RequestHelper.GetResponseContent(response);
        }

        public async Task<string> RemovePostAsync(int postId)
        {
            var response = await SendRemovePostAsync(postId);

            return RequestHelper.GetResponseContent(response);
        }

        public async Task<string> GetSingleLikeForPhotoAsync(int postId)
        {
            var response = await SendGetSingleLikeForPhotoAsync(postId);

            return RequestHelper.GetResponseContent(response);
        }

        public async Task<string> GetLikesForPhotoAsync(int postId)
        {
            var response = await SendGetLikesForPhotoAsync(postId);

            return RequestHelper.GetResponseContent(response);
        }

        public async Task<string> ReactPostAsync(int postId)
        {
            var response = await SendLikePostAsync(postId);

            return RequestHelper.GetResponseContent(response);
        }

        public async Task<string> GetPostsFromUserAsync(int userId)
        {
            var response = await SendGetPostsRequestAsync(userId);

            return RequestHelper.GetResponseContent(response);
        }

        public async Task<string> GetPostsForDashboardAsync()
        {
            var response = await SendGetPostsRequestAsyncWithoutId();

            return RequestHelper.GetResponseContent(response);
        }

        private async Task<HttpResponseMessage> SendAddPostRequest(AddPostRequest request)
        {
            using var client = new HttpClient();
            using var content = new MultipartFormDataContent();

            RequestHelper.AddAuthorizationHeader(client);

            var imageContent = new ByteArrayContent(request.File);
            imageContent.Headers.ContentType =
                MediaTypeHeaderValue.Parse("image/jpeg");

            content.Add(imageContent, "File", "image.jpg");
            content.Add(new StringContent(request.Content), "Content");
            content.Add(new StringContent(request.UserId.ToString()), "UserId");

            var response = await client.PostAsync($"{BaseAddress}", content);

            return response;
        }

        private async Task<HttpResponseMessage> SendRemovePostAsync(int endpoint)
        {
            using var client = new HttpClient();

            RequestHelper.AddAuthorizationHeader(client);

            var response = await client.DeleteAsync($"{BaseAddress}/{endpoint}");

            return response;
        }

        private async Task<HttpResponseMessage> SendGetPostsRequestAsync(int endpoint)
        {
            using var client = new HttpClient();

            RequestHelper.AddAuthorizationHeader(client);

            var response = await client.GetAsync($"{BaseAddress}/{endpoint}");

            return response;
        }

        private async Task<HttpResponseMessage> SendGetPostsRequestAsyncWithoutId()
        {
            using var client = new HttpClient();

            RequestHelper.AddAuthorizationHeader(client);

            var response = await client.GetAsync($"{BaseAddress}");

            return response;
        }

        private async Task<HttpResponseMessage> SendLikePostAsync(int postId)
        {
            using var client = new HttpClient();

            RequestHelper.AddAuthorizationHeader(client);

            var response = await client.PostAsync($"{BaseAddress}/{postId}/like", null);

            return response;
        }

        private async Task<HttpResponseMessage> SendGetLikesForPhotoAsync(int postId)
        {
            using var client = new HttpClient();

            RequestHelper.AddAuthorizationHeader(client);

            var response = await client.GetAsync($"{BaseAddress}/{postId}/likes");

            return response;
        }

        private async Task<HttpResponseMessage> SendGetSingleLikeForPhotoAsync(int postId)
        {
            using var client = new HttpClient();

            RequestHelper.AddAuthorizationHeader(client);

            var response = await client.GetAsync($"{BaseAddress}/{postId}/like");

            return response;
        }
    }
}
