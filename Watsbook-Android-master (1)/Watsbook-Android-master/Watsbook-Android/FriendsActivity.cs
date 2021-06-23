
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
using AndroidX.RecyclerView.Widget;
using Watsbook_Android.Adapters;
using Watsbook_Android.Presenters;

namespace Watsbook_Android
{
    [Activity(Label = "FriendsActivity")]
    public class FriendsActivity : Activity
    {
        RecyclerView userRV;
        private FriendRequestAdapter adapter;
        private FriendsPresenter presenter;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_pending);
            presenter = new FriendsPresenter(this);
            await presenter.GetPendingAsync();
            ConnectViews();
        }

        private void ConnectViews()
        {
            userRV = FindViewById<RecyclerView>(Resource.Id.userRV);
            userRV.SetLayoutManager(new LinearLayoutManager(userRV.Context));
            adapter = new FriendRequestAdapter(presenter.Requests);
            adapter.ItemClicked += Adapter_ItemClicked;
            adapter.ItemLongClicked += Adapter_ItemLongClicked;
            userRV.SetAdapter(adapter);
        }

        private void Adapter_ItemLongClicked(object sender, FriendRequestAdapter.FriendRequestAdapterEventArgs e)
        {
            var dialog = new AndroidX.AppCompat.App.AlertDialog.Builder(this, Resource.Style.AppCompatAlertDialogStyle);
            dialog.SetMessage("Co chcesz usunąć tę znajomość?");

            dialog.SetNegativeButton("Nie", (thisalert, args) =>
            {
            });

            dialog.SetPositiveButton("Usuń", async (thisalert, args) =>
            {
                await presenter.AcceptAsync(presenter.Requests[e.Position].UserId);
                await presenter.GetPendingAsync();

                adapter = new FriendRequestAdapter(presenter.Requests);
                adapter.ItemClicked += Adapter_ItemClicked;
                adapter.ItemLongClicked += this.Adapter_ItemLongClicked;
                userRV.SetAdapter(adapter);
            });

            dialog.Show();
        }

        private void Adapter_ItemClicked(object sender, FriendRequestAdapter.FriendRequestAdapterEventArgs e)
        {
            var userId = presenter.Requests[e.Position].UserId;
            var intent = new Intent(this, typeof(ProfileActivity));
            intent.PutExtra("userId", userId);
            StartActivity(intent);
            Finish();
        }
    }
}
