using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Widget;
using Newtonsoft.Json;
using Watsbook_Android.API.Services;
using Watsbook_Android.Dtos.Responses;

namespace Watsbook_Android.Presenters
{
    public class SearchPresenter
    {
        private readonly UserService userService;
        private readonly SearchActivity activity;

        public SearchPresenter(SearchActivity activity)
        {
            this.activity = activity;
            userService = new UserService();           
        }

        public string KeyWord { get; set; }
        public List<UserDetailResponse> Users { get; private set; }

        public async Task GetUsersAsync()
        {
            if (KeyWord.Length < 3)
            {
                Users = new List<UserDetailResponse>();
                return;
            }
            try
            {
                var response = await userService.GetSearchUsersAsync(KeyWord);

                if (response != null)
                {
                    Users = JsonConvert.DeserializeObject<List<UserDetailResponse>>(response);
                }
            }
            catch (Exception exception)
            {
                Toast.MakeText(activity, exception.Message, ToastLength.Short).Show();
            }
        }
    }
}
