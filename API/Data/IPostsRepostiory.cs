using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Data
{
    public interface IPostsRepostiory
    {
         Task AddAsync<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAllAsync();
         Task<IEnumerable<Post>> GetPostsForUserAsync(int userId);
         Task<IEnumerable<Post>> GetPostsFromFriendsAsync(int userId);
         Task<Post> GetPostAsync(int postId);     
         Task<IEnumerable<LikeIt>> GetPostLikesAsync(int postId);
         Task<LikeIt> GetPostLikeAsync(int postId, int myID);
         
    }
}