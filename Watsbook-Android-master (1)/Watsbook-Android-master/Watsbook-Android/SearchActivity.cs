
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
    [Activity(Label = "SearchActivity")]
    public class SearchActivity : AppCompatActivity
    {
        EditText search;
        RecyclerView userRV;
        private SearchPresenter presenter;
        private UserAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_search);
            presenter = new SearchPresenter(this);
            ConnectViews();
            // Create your application here
        }

        private void ConnectViews()
        {
            search = FindViewById<EditText>(Resource.Id.search);
            userRV = FindViewById<RecyclerView>(Resource.Id.userRV);
            userRV.SetLayoutManager(new LinearLayoutManager(userRV.Context));

            search.TextChanged += Search_TextChanged;
        }

        private async void Search_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            presenter.KeyWord = search.Text;
            await presenter.GetUsersAsync();
            adapter = new UserAdapter(presenter.Users);
            adapter.ItemClicked += Adapter_ItemClicked;
            userRV.SetAdapter(adapter);
        }

        private void Adapter_ItemClicked(object sender, UserAdapter.UserAdapterEventArgs e)
        {
            var intent = new Intent(this, typeof(ProfileActivity));
            intent.PutExtra("userId", presenter.Users[e.Position].Id);
            StartActivity(intent);
        }
    }
}
