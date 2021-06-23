using System;
using System.Threading.Tasks;
using Android.Widget;
using Newtonsoft.Json;
using Watsbook_Android.API.Services;
using Watsbook_Android.Dtos.Requests;
using Watsbook_Android.Dtos.Responses;
using Watsbook_Android.Others;

namespace Watsbook_Android.Presenters
{
    public class LoginPresenter
    {
        private readonly AuthorizationService authService;
        private readonly MainActivity activity;

        public LoginPresenter(MainActivity activity)
        {
            authService = new AuthorizationService();
            this.activity = activity;
        }

        public string UserName { get; set; }
        public string Password { get; set; }

        public async Task LoginUserAsync()
        {
            try
            {
                var request = new UserLoginRequest
                {
                    UserName = UserName,
                    Password = Password
                };

                var response = await authService.LoginAsync(request);

                if (response != null)
                {
                    // Zapisuje zalogowanego użytkownika do pamięci aplikacji
                    var loginResponse = JsonConvert.DeserializeObject<UserLoginResponse>(response);
                    SharedPrefManager.SaveUser(loginResponse.User);
                    SharedPrefManager.SaveToken(loginResponse.Token);
                    activity.StartActivity(typeof(DashboardActivity));
                    activity.FinishAffinity();

                    Toast.MakeText(activity, "Zalogowano", ToastLength.Short).Show();
                }

            }
            catch (Exception exception)
            {
                Toast.MakeText(activity, exception.Message, ToastLength.Short).Show();
            }
        }
    }
}
