
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
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
    [Activity(Label = "ProfileActivity")]
    public class ProfileActivity : AppCompatActivity
    {
        CircleImageView profileIV;
        TextView nameTV;
        Button inviteOrRemoveBtn;
        Button friendsBtn;
        RecyclerView postsRV;
        private ProfilePresenter presenter;
        private PostAdapter adapter;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_profile);
            presenter = new ProfilePresenter(this);
            await presenter.GetPostsAsync();
            await presenter.GetUserAsync();
            await presenter.CheckIfFriendsAsync();
            ConnectViews();
        }

        private void ConnectViews()
        {
            profileIV = FindViewById<CircleImageView>(Resource.Id.profileIV);
            nameTV = FindViewById<TextView>(Resource.Id.nameTV);
            inviteOrRemoveBtn = FindViewById<Button>(Resource.Id.inviteOrRemoveBtn);
            friendsBtn = FindViewById<Button>(Resource.Id.friendsBtn);
            postsRV = FindViewById<RecyclerView>(Resource.Id.postsRV);

            postsRV.SetLayoutManager(new LinearLayoutManager(postsRV.Context));
            adapter = new PostAdapter(presenter.Posts);
            
            if (presenter.IsMine && !presenter.AreFriends)
            {
                inviteOrRemoveBtn.Visibility = ViewStates.Gone;
                profileIV.Click += ProfileIV_Click;
                adapter.ItemLongClicked += Adapter_ItemLongClicked;
            }
            else
            {          
                inviteOrRemoveBtn.Click += InviteOrRemoveBtn_Click;
            }

            if (presenter.AreFriends)
            {
                inviteOrRemoveBtn.Visibility = ViewStates.Gone;
            }

            if (presenter.User.PhotoUrl != null)
                ViewsHelper.GetImageFromUrl(presenter.User.PhotoUrl, profileIV);

            nameTV.Text = $"{presenter.User.Name} {presenter.User.Surname}";

            postsRV.SetAdapter(adapter);

            friendsBtn.Click += (s, e) =>
            {
                var intent = new Intent(this, typeof(FriendsActivity));
                intent.PutExtra("userId", presenter.User.Id);
                StartActivity(intent);
            };

        }

        private async void InviteOrRemoveBtn_Click(object sender, EventArgs e)
        {
            await presenter.InviteAsync();
            ConnectViews();
        }

        private void Adapter_ItemLongClicked(object sender, PostAdapter.PostAdapterEventArgs e)
        {

            var dialog = new AndroidX.AppCompat.App.AlertDialog.Builder(this, Resource.Style.AppCompatAlertDialogStyle);
            dialog.SetMessage("Czy chcesz usunąć tego posta?");

            dialog.SetNegativeButton("Nie", (thisalert, args) =>
            {
            });

            dialog.SetPositiveButton("Usuń", async (thisalert, args) =>
            {
                await presenter.RemovePostAsync(presenter.Posts[e.Position].Id);
                await presenter.GetPostsAsync();

                adapter = new PostAdapter(presenter.Posts);
                postsRV.SetAdapter(adapter);
            });

            dialog.Show();
            
        }

        private async void ProfileIV_Click(object sender, EventArgs e)
        {
            await presenter.AddPhotoAsync();
            if (presenter.User.PhotoUrl != null)
                ViewsHelper.GetImageFromUrl(presenter.User.PhotoUrl, profileIV);
        }
    }
}
