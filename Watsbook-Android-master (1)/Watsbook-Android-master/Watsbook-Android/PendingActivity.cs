
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
using AndroidX.RecyclerView.Widget;
using Watsbook_Android.Adapters;
using Watsbook_Android.Presenters;

namespace Watsbook_Android
{
    [Activity(Label = "PendingActivity")]
    public class PendingActivity : AppCompatActivity
    {
        RecyclerView userRV;
        private FriendRequestAdapter adapter;
        private PendingPresenter presenter;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_pending);
            presenter = new PendingPresenter(this);
            await presenter.GetPendingAsync();
            ConnectViews();
        }

        private void ConnectViews()
        {
            userRV = FindViewById<RecyclerView>(Resource.Id.userRV);
            userRV.SetLayoutManager(new LinearLayoutManager(userRV.Context));
            adapter = new FriendRequestAdapter(presenter.Requests);
            adapter.ItemClicked += Adapter_ItemClicked;
            userRV.SetAdapter(adapter);
        }

        private void Adapter_ItemClicked(object sender, FriendRequestAdapter.FriendRequestAdapterEventArgs e)
        {
            var dialog = new AndroidX.AppCompat.App.AlertDialog.Builder(this, Resource.Style.AppCompatAlertDialogStyle);
            dialog.SetMessage("Co chcesz zrobić z tym zaproszeniem?");

            dialog.SetNegativeButton("Odrzuć", async (thisalert, args) =>
            {
                await presenter.DeclineAsync(presenter.Requests[e.Position].UserId);
                await presenter.GetPendingAsync();

                adapter = new FriendRequestAdapter(presenter.Requests);
                adapter.ItemClicked += Adapter_ItemClicked;
                userRV.SetAdapter(adapter);
            });

            dialog.SetPositiveButton("Zaakcpetuj", async (thisalert, args) =>
            {
                await presenter.AcceptAsync(presenter.Requests[e.Position].UserId);
                await presenter.GetPendingAsync();

                adapter = new FriendRequestAdapter(presenter.Requests);
                adapter.ItemClicked += Adapter_ItemClicked;
                userRV.SetAdapter(adapter);
            });          

            dialog.Show();
        }
    }
}
