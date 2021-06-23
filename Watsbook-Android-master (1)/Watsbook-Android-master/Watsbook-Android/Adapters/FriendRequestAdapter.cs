using System;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Refractored.Controls;
using Watsbook_Android.Dtos.Responses;
using Watsbook_Android.Others;

namespace Watsbook_Android.Adapters
{
    public class FriendRequestAdapter : RecyclerView.Adapter
    {
        public event EventHandler<FriendRequestAdapterEventArgs> ItemClicked;
        public event EventHandler<FriendRequestAdapterEventArgs> ItemLongClicked;
        private readonly List<FriendRequestResponse> users;

        public FriendRequestAdapter(List<FriendRequestResponse> users)
        {
            this.users = users;
        }

        public override int ItemCount => users.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as FriendRequestAdapterViewHolder;

            var item = users[position];

            viewHolder.UserNameTV.Text = $"{item.Name} {item.Surname}";
            ViewsHelper.GetImageFromUrl(item.PhotoUrl, viewHolder.ImageIV);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).
                Inflate(Resource.Layout.item_user, parent, false);

            return new FriendRequestAdapterViewHolder(itemView, OnClick, OnLongClick);
        }

        private void OnClick(FriendRequestAdapterEventArgs args)
        {
            ItemClicked?.Invoke(this, new FriendRequestAdapterEventArgs { Position = args.Position });
        }

        private void OnLongClick(FriendRequestAdapterEventArgs args)
        {
            ItemLongClicked?.Invoke(this, new FriendRequestAdapterEventArgs { Position = args.Position });
        }

        public class FriendRequestAdapterViewHolder : RecyclerView.ViewHolder
        {
            TextView nameTV;
            CircleImageView profileIV;

            public FriendRequestAdapterViewHolder(View itemView, Action<FriendRequestAdapterEventArgs> clickListener,
                Action<FriendRequestAdapterEventArgs> longClickListener) : base(itemView)
            {
                UserNameTV = itemView.FindViewById<TextView>(Resource.Id.nameTV);
                ImageIV = itemView.FindViewById<CircleImageView>(Resource.Id.profileIV);
                itemView.Click += (s, e) => clickListener(new FriendRequestAdapterEventArgs { Position = AdapterPosition });
                itemView.LongClick += (s, e) => longClickListener(new FriendRequestAdapterEventArgs { Position = AdapterPosition });
            }

            public TextView UserNameTV { get => nameTV; set => nameTV = value; }
            public CircleImageView ImageIV { get => profileIV; set => profileIV = value; }

        }

        public class FriendRequestAdapterEventArgs : EventArgs
        {
            public int Position { get; set; }
        }
    }
}
