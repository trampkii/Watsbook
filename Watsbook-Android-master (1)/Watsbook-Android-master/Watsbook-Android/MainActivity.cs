using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Android.Widget;
using Watsbook_Android.Presenters;

namespace Watsbook_Android
{
    [Activity(Label = "@string/app_name")]
    public class MainActivity : AppCompatActivity
    {
        TextView registerTV;
        EditText userNameET;
        EditText passwordET;
        Button loginBtn;
        LoginPresenter presenter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            ConnectViews();
            presenter = new LoginPresenter(this);
        }

        private void ConnectViews()
        {
            registerTV = FindViewById<TextView>(Resource.Id.registerTV);
            userNameET = FindViewById<EditText>(Resource.Id.userNameET);
            passwordET = FindViewById<EditText>(Resource.Id.passwordET);
            loginBtn = FindViewById<Button>(Resource.Id.loginBtn);

            registerTV.Click += (s, e) => { StartActivity(typeof(RegisterActivity)); };
            loginBtn.Click += async (s, e) => { await presenter.LoginUserAsync(); };

            userNameET.TextChanged += UserNameET_TextChanged;
            passwordET.TextChanged += PasswordET_TextChanged;
        }

        private void PasswordET_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            presenter.Password = passwordET.Text;
        }

        private void UserNameET_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            presenter.UserName = userNameET.Text;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        

    }
}
