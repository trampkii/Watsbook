using System;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Watsbook_Android.Dtos.Responses;
using Watsbook_Android.Others;
using Watsbook_Android.Presenters;

namespace Watsbook_Android.Adapters
{
    public class PostAdapter : RecyclerView.Adapter
    {
        public event EventHandler<PostAdapterEventArgs> ReactButtonClicked;
        public event EventHandler<PostAdapterEventArgs> ItemLongClicked;
        private readonly List<PostDetailResponse> posts;
        private readonly PostPresenter presenter;

        public PostAdapter(List<PostDetailResponse> posts)
        {
            this.posts = posts;
            presenter = new PostPresenter();
        }

        public override int ItemCount => posts.Count;

        public override async void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as PostAdapterViewHolder;

            var item = posts[position];

            var isLiked = await presenter.GetLikeForPostAsync(item.Id) != null;

            viewHolder.UserNameTV.Text = item.UserName;

            if (isLiked) viewHolder.ReactButton.Text = "Nie lubię tego!";
            else viewHolder.ReactButton.Text = "Lubię to!";

            // pobieranie liczby polubień tutaj zrobić

            var numberOfLikes = await presenter.GetNumberOfLikes(item.Id);
            viewHolder.NumberOfLikesTV.Text = $"{numberOfLikes} polubień";

            ViewsHelper.GetImageFromUrl(item.PhotoUrl, viewHolder.ImageIV);

            viewHolder.ReactButton.Click += async (s, e) =>
            {
                await presenter.ReactPostAsync(item.Id);
                var isLiked = await presenter.GetLikeForPostAsync(item.Id) != null;

                viewHolder.UserNameTV.Text = item.UserName;

                if (isLiked) viewHolder.ReactButton.Text = "Nie lubię tego!";
                else viewHolder.ReactButton.Text = "Lubię to!";

                var numberOfLikes = await presenter.GetNumberOfLikes(item.Id);
                viewHolder.NumberOfLikesTV.Text = $"{numberOfLikes} polubień";
            };

        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).
                Inflate(Resource.Layout.item_post, parent, false);

            return new PostAdapterViewHolder(itemView, OnLongClick);
        }

        void OnLongClick(PostAdapterEventArgs args)
        {
            ItemLongClicked?.Invoke(this, args);
        }

        public class PostAdapterViewHolder : RecyclerView.ViewHolder
        {
            TextView userNameTV;
            ImageView imageIV;
            TextView numberOfLikesTV;
            Button reactButton;

            public PostAdapterViewHolder(View itemView, Action<PostAdapterEventArgs> clickListener) : base(itemView)
            {
                UserNameTV = itemView.FindViewById<TextView>(Resource.Id.userNameTV);
                ImageIV = itemView.FindViewById<ImageView>(Resource.Id.imageIV);
                NumberOfLikesTV = itemView.FindViewById<TextView>(Resource.Id.numberOfLikesTV);
                ReactButton = itemView.FindViewById<Button>(Resource.Id.reactButton);

                itemView.Click += (s, e) => clickListener(new PostAdapterEventArgs { Position = AdapterPosition });
            }

            public TextView UserNameTV { get => userNameTV; set => userNameTV = value; }
            public ImageView ImageIV { get => imageIV; set => imageIV = value; }
            public TextView NumberOfLikesTV { get => numberOfLikesTV; set => numberOfLikesTV = value; }
            public Button ReactButton { get => reactButton; set => reactButton = value; }
        }

        public class PostAdapterEventArgs: EventArgs
        {
            public int Position { get; set; }
        }
    }
}
