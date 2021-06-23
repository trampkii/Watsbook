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
    public class ProfilePresenter
    {
        private readonly ProfileActivity activity;
        private readonly PostService postsService;
        private readonly UserService userService;
        private readonly int userId;


        public ProfilePresenter(ProfileActivity activity)
        {
            this.activity = activity;
            postsService = new PostService();
            userService = new UserService();

            userId = activity.Intent.GetIntExtra("userId", 0);

            CheckIfImOwnerOfThisProfile();
        }

        public bool IsMine { get; private set; }
        public List<PostDetailResponse> Posts { get; private set; }
        public UserDetailResponse User { get; private set; }
        public bool AreFriends { get; private set; }

        public async Task CheckIfFriendsAsync()
        {
            try
            {
                var response = await userService.CheckIfAreFriends(userId);

                if (response != "")
                {
                    AreFriends = true;
                }
                else AreFriends = false;
            }
            catch
            {
                
            }
        }

        public async Task InviteAsync()
        {
            try
            {
                var response = await userService.InviteUserAsync(userId);

                if (response != null)
                {
                    Toast.MakeText(activity, "Wysłano zaproszenie", ToastLength.Short).Show();
                }
            }
            catch
            {
                Toast.MakeText(activity, "Zaproszenie już istnieje.", ToastLength.Short).Show();
            }
        }

        public async Task RemovePostAsync(int postId)
        {
            try
            {
                var response = await postsService.RemovePostAsync(postId);

                if (response != null)
                {
                    Toast.MakeText(activity, "Usunięto post.", ToastLength.Short).Show();
                }
            }
            catch (Exception exception)
            {
                Toast.MakeText(activity, exception.Message, ToastLength.Long).Show();
            }
        }

        public async Task AddPhotoAsync()
        {
            try
            {
                var file = await ViewsHelper.SelectPhotoAsync();
                var userId = SharedPrefManager.GetUser().Id;

                var request = new ChangePhotoRequest
                {
                    File = file,
                    UserId = userId
                };

                var response = await userService.ChangePhotoAsync(request);

                if (response != null)
                {
                    Toast.MakeText(activity, "Dodano zdjęcie.", ToastLength.Short).Show();
                    await GetUserAsync();
                    SharedPrefManager.SaveUser(User);
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
                var response = await postsService.GetPostsFromUserAsync(userId);

                if (response != null)
                {
                    Posts = JsonConvert.DeserializeObject<List<PostDetailResponse>>(response);
                }
            }
            catch (Exception exception)
            {
                Toast.MakeText(activity, exception.Message, ToastLength.Short).Show();
            }
        }

        public async Task GetUserAsync()
        {
            //var userId = SharedPrefManager.GetUser().Id;

            try
            {
                var response = await userService.GetUserAsync(userId);

                if (response != null)
                {
                    User = JsonConvert.DeserializeObject<UserDetailResponse>(response);
                }
            }
            catch (Exception exception)
            {
                Toast.MakeText(activity, exception.Message, ToastLength.Short).Show();
            }
        }

        private void CheckIfImOwnerOfThisProfile()
        {
            IsMine = userId == SharedPrefManager.GetUser().Id;
        }
    }
}
