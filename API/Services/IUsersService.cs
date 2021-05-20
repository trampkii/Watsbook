using System;
using System.Threading.Tasks;
using API.DataTransferObjects;
using API.Parameters;

namespace API.Services
{
    public interface IUsersService
    {
         Task<object> GetSingleUser(int userId);
         Task<object> GetUsersToSearch(SearchParameters searchParameters);
         Task<object> SendFriendRequest(int id, int myID);
         Task<object> AcceptRequest(int senderId, int myID);
         Task<object> DeclineRequest(int recipientId, int myID);
         Task<object> DeleteFriend(int friendId, int myID);
         Task<object> GetFriends(int userId);
         Task<object> GetFriendRequests(int myID);
         Task<object> GetFriendRelation(int id, int myID);
         Task<IAsyncResult> ChangeAvatar(AvatarForChange avatarForChange);
    }
}