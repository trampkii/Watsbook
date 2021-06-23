using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Widget;
using Newtonsoft.Json;
using Watsbook_Android.API.Services;
using Watsbook_Android.Dtos.Responses;

namespace Watsbook_Android.Presenters
{
    public class PendingPresenter
    {
        private readonly PendingActivity activity;
        private readonly UserService userService;

        public PendingPresenter(PendingActivity activity)
        {
            this.activity = activity;
            userService = new UserService();
        }

        public List<FriendRequestResponse> Requests;

        public async Task AcceptAsync(int id)
        {
            try
            {
                var response = await userService.AcceptRequestAsync(id);

                if (response != null)
                {
                    Toast.MakeText(activity, "Pomyślnie zaakcpetowano.", ToastLength.Long).Show();  
                }
            }
            catch (Exception exception)
            {
                Toast.MakeText(activity, exception.Message, ToastLength.Short).Show();
            }
        }

        public async Task DeclineAsync(int id)
        {
            try
            {
                var response = await userService.DeclineAsync(id);

                if (response != null)
                {
                    Toast.MakeText(activity, "Pomyślnie odrzucono.", ToastLength.Long).Show();
                }

            }
            catch (Exception exception)
            {
                Toast.MakeText(activity, exception.Message, ToastLength.Short).Show();
            }
        }

        public async Task GetPendingAsync()
        {
            //var userId = SharedPrefManager.GetUser().Id;

            try
            {
                var response = await userService.GetPendingRequestsAsync();

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
