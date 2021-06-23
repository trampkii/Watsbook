
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Watsbook_Android.Others;

namespace Watsbook_Android
{
    [Activity(Label = "SplashActivity", MainLauncher = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_splash);
            Redirect();
        }

        protected override void OnResume()
        {
            base.OnResume();
            Redirect();
        }

        private void Redirect()
        {
            FinishAffinity();

            if (IsLoggedIn())
            {
                StartActivity(typeof(DashboardActivity));

                return;
            }

            StartActivity(typeof(MainActivity));
        }

        private bool IsLoggedIn()
        {
            return SharedPrefManager.GetUser() != null;
        }
    }
}
