using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Widget;
using Newtonsoft.Json;
using Watsbook_Android.API.Services;
using Watsbook_Android.Dtos.Requests;
using Watsbook_Android.Dtos.Responses;
using Watsbook_Android.Others;

namespace Watsbook_Android.Presenters
{
    public class DashboardPresenter
    {
        private readonly PostService postsService;
        private readonly DashboardActivity activity;

        public DashboardPresenter(DashboardActivity activity)
        {
            this.activity = activity;
            postsService = new PostService();
            User = SharedPrefManager.GetUser();
        }

        public List<PostDetailResponse> Responses { get; private set; }
        public UserDetailResponse User { get; set; }
        public string Content { get; set; } = string.Empty;
        public byte[] File { get; set; }

        public void Logout()
        {
            SharedPrefManager.ClearPreferences();
            activity.StartActivity(typeof(MainActivity));
            activity.FinishAffinity();
        }

        public async Task AddPhotoAsync()
        {
            try
            {                
                var userId = SharedPrefManager.GetUser().Id;

                var request = new AddPostRequest
                {
                    File = File,
                    UserId = userId,
                    Content = Content
                };

                var response = await postsService.SendPostAsync(request);

                if (response != null)
                {
                    Toast.MakeText(activity, "Dodano.", ToastLength.Short).Show();
                }
            }
            catch (Exception exception)
            {
                Toast.MakeText(activity, exception.Message, ToastLength.Long).Show();
            }

        }

        public async Task GetPostsAsync()
        {
            //var userId = SharedPrefManager.GetUser().Id;

            try
            {
                var response = await postsService.GetPostsForDashboardAsync();

                if (response != null)
                {
                    Responses = JsonConvert.DeserializeObject<List<PostDetailResponse>>(response);
                }
            }
            catch (Exception exception)
            {
                Toast.MakeText(activity, exception.Message, ToastLength.Short).Show();
            }
        }
    }
}
