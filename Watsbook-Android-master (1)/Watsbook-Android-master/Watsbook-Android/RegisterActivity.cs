
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
using AndroidX.AppCompat.App;
using Watsbook_Android.Presenters;

namespace Watsbook_Android
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : AppCompatActivity
    {
        EditText userNameET;
        EditText firstNameET;
        EditText lastNameET;
        EditText cityET;
        EditText passwordET;
        EditText confirmPasswordET;
        Button registerBtn;
        RegisterPresenter presenter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_register);
            ConnectViews();
            presenter = new RegisterPresenter(this);
        }

        private void ConnectViews()
        {
            userNameET = FindViewById<EditText>(Resource.Id.userNameET);
            firstNameET = FindViewById<EditText>(Resource.Id.firstNameET);
            lastNameET = FindViewById<EditText>(Resource.Id.lastNameET);
            cityET = FindViewById<EditText>(Resource.Id.cityET);
            passwordET = FindViewById<EditText>(Resource.Id.passwordET);
            confirmPasswordET = FindViewById<EditText>(Resource.Id.confirmPasswordET);
            registerBtn = FindViewById<Button>(Resource.Id.registerBtn);

            userNameET.TextChanged += UserNameET_TextChanged;
            firstNameET.TextChanged += FirstNameET_TextChanged;
            lastNameET.TextChanged += LastNameET_TextChanged;
            cityET.TextChanged += CityET_TextChanged;
            passwordET.TextChanged += PasswordET_TextChanged;
            confirmPasswordET.TextChanged += ConfirmPasswordET_TextChanged;
            registerBtn.Click += async (s, e) => { await presenter.RegisterAsync(); }; 
        }

        private void ConfirmPasswordET_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            presenter.ConfirmPassword = confirmPasswordET.Text;
        }

        private void PasswordET_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            presenter.Password = passwordET.Text;
        }

        private void CityET_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            presenter.Location = cityET.Text;
        }

        private void LastNameET_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            presenter.Surname = lastNameET.Text;
        }

        private void FirstNameET_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            presenter.Name = firstNameET.Text;
        }

        private void UserNameET_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            presenter.UserName = userNameET.Text;
        }
    }
}
