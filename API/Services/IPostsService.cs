using System;
using System.Threading.Tasks;
using API.DataTransferObjects;

namespace API.Services
{
    public interface IPostsService
    {
         Task<object> AddPost(PostForCreation postForCreation, int myID);
         Task<object> GetPostsForUser(int id);
         Task<object> GetPostsFromFriends(int myID);
         Task<object> DeletePost(int id, int myID);
         Task<IAsyncResult> ReactPost(int postId, int myID);
         Task<object> GetPostLikes(int postId);
         Task<object> GetPostLike(int postId, int myID);
    }
}