using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using Newtonsoft.Json;
using Watsbook_Android.API.Services;
using Watsbook_Android.Dtos.Responses;

namespace Watsbook_Android.Presenters
{
    public class PostPresenter
    {
        private readonly PostService postService;

        public PostPresenter()
        {
            postService = new PostService();
        }

        public async Task ReactPostAsync(int postId)
        {
            try
            {

                await postService.ReactPostAsync(postId);

            }
            catch (Exception exception)
            {
                Toast.MakeText(Application.Context, exception.Message, ToastLength.Short).Show();
            }
        }

        public async Task<PostLikeResponse> GetLikeForPostAsync(int postId)
        {
            try
            {

                var response = await postService.GetSingleLikeForPhotoAsync(postId);

                if (response != null)
                {
                    // Zapisuje zalogowanego użytkownika do pamięci aplikacji
                    var likeResponse = JsonConvert.DeserializeObject<PostLikeResponse>(response);
                    return likeResponse;
                }

                return null;

            }
            catch
            {
                return null;
            }
        }

        public async Task<int> GetNumberOfLikes(int postId)
        {
            try
            {

                var response = await postService.GetLikesForPhotoAsync(postId);

                if (response != null)
                {
                    // Zapisuje zalogowanego użytkownika do pamięci aplikacji
                    var likeResponse = JsonConvert.DeserializeObject<List<PostLikeResponse>>(response);
                    return likeResponse.Count;
                }

                return 0;

            }
            catch
            {
                return 0;
            }
        }
    }
}
