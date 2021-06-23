using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Widget;
using Newtonsoft.Json;
using Watsbook_Android.API.Services;
using Watsbook_Android.Dtos.Responses;
using Watsbook_Android.Others;

namespace Watsbook_Android.Presenters
{
    public class FriendsPresenter
    {
        private readonly FriendsActivity activity;
        private readonly UserService userService;
        private readonly int userId;

        public FriendsPresenter(FriendsActivity activity)
        {
            this.activity = activity;
            userService = new UserService();
            userId = activity.Intent.GetIntExtra("userId", 0);
        }

        public List<FriendRequestResponse> Requests;

        public async Task AcceptAsync(int id)
        {
            try
            {
                var response = await userService.RemoveFriendAsync(id);

                if (response != null)
                {
                    Toast.MakeText(activity, "Pomyślnie usunięto.", ToastLength.Long).Show();
                }
            }
            catch (Exception exception)
            {
                Toast.MakeText(activity, exception.Message, ToastLength.Short).Show();
            }
        }

        public async Task GetPendingAsync()
        {
            try
            {
                var response = await userService.GetFriendsAsync(userId);

                if (response != null)
                {
                    Requests = JsonConvert.DeserializeObject<List<FriendRequestResponse>>(response);
                }
            }
            catch (Exception exception)
            {
                Toast.MakeText(activity, exception.Message, ToastLength.Short).Show();
            }
        }

    }
}
