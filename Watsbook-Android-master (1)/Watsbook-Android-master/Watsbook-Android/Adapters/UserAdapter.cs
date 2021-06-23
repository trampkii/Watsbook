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
    public class UserAdapter : RecyclerView.Adapter
    {
        public event EventHandler<UserAdapterEventArgs> ItemClicked;
        private readonly List<UserDetailResponse> users;

        public UserAdapter(List<UserDetailResponse> users)
        {
            this.users = users; 
        }

        public override int ItemCount => users.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as UserAdapterViewHolder;

            var item = users[position];

            viewHolder.UserNameTV.Text = $"{item.Name} {item.Surname}";
            ViewsHelper.GetImageFromUrl(item.PhotoUrl, viewHolder.ImageIV);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).
                Inflate(Resource.Layout.item_user, parent, false);

            return new UserAdapterViewHolder(itemView, OnClick);
        }

        private void OnClick(UserAdapterEventArgs args)
        {
            ItemClicked?.Invoke(this, new UserAdapterEventArgs { Position = args.Position });
        }

        public class UserAdapterViewHolder : RecyclerView.ViewHolder
        {
            TextView nameTV;
            CircleImageView profileIV;

            public UserAdapterViewHolder(View itemView, Action<UserAdapterEventArgs> clickListener) : base(itemView)
            {
                UserNameTV = itemView.FindViewById<TextView>(Resource.Id.nameTV);
                ImageIV = itemView.FindViewById<CircleImageView>(Resource.Id.profileIV);
                itemView.Click += (s, e) => clickListener(new UserAdapterEventArgs { Position = AdapterPosition });
            }

            public TextView UserNameTV { get => nameTV; set => nameTV = value; }
            public CircleImageView ImageIV { get => profileIV; set => profileIV = value; }

        }

        public class UserAdapterEventArgs : EventArgs
        {
            public int Position { get; set; }
        }
    }
}
