using System;
using System.Threading.Tasks;
using Android.Widget;
using Watsbook_Android.API.Services;
using Watsbook_Android.Dtos.Requests;

namespace Watsbook_Android.Presenters
{
    public class RegisterPresenter
    {
        private readonly AuthorizationService authService;
        private readonly RegisterActivity activity;

        public RegisterPresenter(RegisterActivity activity)
        {
            authService = new AuthorizationService();
            this.activity = activity;
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string Location { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public async Task RegisterAsync()
        {

            try
            {
                CheckIfPasswordsMatch();

                var request = new UserRegisterRequest
                {
                    UserName = UserName,
                    Name = Name,
                    Surname = Surname,
                    Location = Location,
                    Password = Password
                };

                var response = await authService.RegisterAsync(request);

                if (response != null)
                {
                    // Po pomyślnym utworzeniu konta przenieś do ekranu logowania
                    Toast.MakeText(activity, "Dziękujemy za rejestrację.", ToastLength.Short).Show();
                    activity.StartActivity(typeof(MainActivity));
                    activity.Finish();
                }

            }
            catch (Exception exception)
            {
                Toast.MakeText(activity, exception.Message, ToastLength.Short).Show();
            }
        }

        private void CheckIfPasswordsMatch()
        {
            if (Password != ConfirmPassword)
            {
                throw new Exception("Hasla nie sa takie same.");
            }
        }
    }
}
