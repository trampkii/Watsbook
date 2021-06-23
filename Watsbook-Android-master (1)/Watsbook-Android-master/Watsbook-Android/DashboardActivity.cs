
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using Refractored.Controls;
using Watsbook_Android.Adapters;
using Watsbook_Android.Others;
using Watsbook_Android.Presenters;

namespace Watsbook_Android
{
    [Activity(Label = "DashboardActivity")]
    public class DashboardActivity : AppCompatActivity
    {
        ImageView invitationsIV;
        ImageView logout;
        ImageView searchIV;
        ImageView attached;
        EditText content;
        Button addBtn;
        CircleImageView userIV;
        RecyclerView postsRV;
        RelativeLayout bottomBarRL;
        PostAdapter adapter;
        DashboardPresenter presenter;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_dashboard);
            presenter = new DashboardPresenter(this);
            await presenter.GetPostsAsync();
            ConnectViews();
            
        }

        protected override async void OnResume()
        {
            base.OnResume();
            presenter.User = SharedPrefManager.GetUser();
            await presenter.GetPostsAsync();
            if (presenter.User.PhotoUrl != null)
                ViewsHelper.GetImageFromUrl(presenter.User.PhotoUrl, userIV);
        }

        protected override async void OnRestart()
        {
            base.OnRestart();
            presenter.User = SharedPrefManager.GetUser();
            await presenter.GetPostsAsync();
            if (presenter.User.PhotoUrl != null)
                ViewsHelper.GetImageFromUrl(presenter.User.PhotoUrl, userIV);
        }



        private void ConnectViews()
        {
            postsRV = FindViewById<RecyclerView>(Resource.Id.postsRV);
            bottomBarRL = FindViewById<RelativeLayout>(Resource.Id.bottomBarRL);
            invitationsIV = FindViewById<ImageView>(Resource.Id.invitationsIV);
            searchIV = FindViewById<ImageView>(Resource.Id.searchIV);
            userIV = FindViewById<CircleImageView>(Resource.Id.userIV);
            logout = FindViewById<ImageView>(Resource.Id.logout);
            addBtn = FindViewById<Button>(Resource.Id.addBtn);
            attached = FindViewById<ImageView>(Resource.Id.attached);
            content = FindViewById<EditText>(Resource.Id.content);

            postsRV.SetLayoutManager(new LinearLayoutManager(postsRV.Context));
            adapter = new PostAdapter(presenter.Responses);
            postsRV.SetAdapter(adapter);

            if (presenter.User.PhotoUrl != null)
                ViewsHelper.GetImageFromUrl(presenter.User.PhotoUrl, userIV);

            userIV.Click += UserIV_Click;
            searchIV.Click += SearchIV_Click;
            invitationsIV.Click += InvitationsIV_Click;
            logout.Click += Logout_Click;
            attached.Click += Attached_Click;
            content.TextChanged += Content_TextChanged;
            addBtn.Click += AddBtn_Click;
        }

        private async void AddBtn_Click(object sender, EventArgs e)
        {
            if (presenter.File != null)
            {
                await presenter.AddPhotoAsync();
                attached.SetImageResource(Resource.Drawable.placeholder);
                content.Text = "";
            }
            else Toast.MakeText(this, "Wybierz zdjęcie.", ToastLength.Long).Show();
            
        }

        private void Content_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            presenter.Content = content.Text;
        }

        private async void Attached_Click(object sender, EventArgs e)
        {
            try
            {
                presenter.File = await ViewsHelper.SelectPhotoAsync();
                Bitmap bmp = await BitmapFactory.DecodeByteArrayAsync(presenter.File, 0, presenter.File.Length);
                attached.SetImageBitmap(Bitmap.CreateScaledBitmap(bmp, attached.Width, attached.Height, false));
            }
            catch
            {
                Toast.MakeText(this, "Plik nie istnieje.", ToastLength.Long).Show();
            }                        
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            
            var dialog = new AndroidX.AppCompat.App.AlertDialog.Builder(this, Resource.Style.AppCompatAlertDialogStyle);
            dialog.SetMessage("Czy na pewno chcesz się wylogować?");

            dialog.SetNegativeButton("Nie", (thisalert, args) =>
            {
                
            });

            dialog.SetPositiveButton("Wyloguj", (thisalert, args) =>
            {
                presenter.Logout();
            });

            dialog.Show();
        }

        private void InvitationsIV_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(PendingActivity));
        }

        private void SearchIV_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(SearchActivity));
        }

        private void UserIV_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(ProfileActivity));
            intent.PutExtra("userId", presenter.User.Id);
            StartActivity(intent);
        }
    }
}
