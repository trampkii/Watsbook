using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class PostsRepository : IPostsRepostiory
    {
        private readonly DataContxt context;
        public PostsRepository(DataContxt context)
        {
            this.context = context;

        }
        public async Task AddAsync<T>(T entity) where T : class
        {
            await context.AddAsync(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            context.Remove(entity);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Post>> GetPostsForUserAsync(int userId)
        {
            var posts = await context.Posts.Where(p => p.UserId == userId).ToListAsync();
            return posts.OrderByDescending(x => x.CreationDate);
        }

        public async Task<IEnumerable<Post>> GetPostsFromFriendsAsync(int userId)
        {
            var friendBeings = await context.Friends.Where(x => x.SecondUserId == userId)
                .Select(x => x.UserId).ToListAsync();

            return await context.Posts.Where(x => friendBeings.Contains(x.UserId)).Include(x => x.User)
                .OrderByDescending(x => x.CreationDate).ToListAsync();
        }

        public async Task<Post> GetPostAsync(int postId)
        {
            return await context.Posts.FirstOrDefaultAsync(x => x.Id == postId);
        }

        public async Task<IEnumerable<LikeIt>> GetPostLikesAsync(int postId)
        {
            return await context.Likes.Where(x => x.PostId == postId).ToListAsync();
        }

        public async Task<LikeIt> GetPostLikeAsync(int postId, int myID)
        {
            return await context.Likes
                .FirstOrDefaultAsync(x => x.PostId == postId && x.UserId == myID);
        }

    }
}