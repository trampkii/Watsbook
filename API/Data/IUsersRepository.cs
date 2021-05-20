using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Data
{
    
    public interface IUsersRepository
    {
        Task AddAsync<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersToSearchAsync(string keyWord);
        Task<AppUser> GetSingleUserAsync(int userId);
        Task<FriendRequest> GetFriendRequestAsync(int firstId, int secondId);
        Task<IEnumerable<FriendRequest>> GetFriendRequestsAsync(int userId);
        Task<Friend> GetFriendsRelationAsync(int firstId, int secondId);
        Task<Friend> GetFriendGottenAsync(int id, int myID);
        Task<Friend> GetFriendBeingAsync(int id, int myID);
        Task<IEnumerable<Friend>> GetFriendsWithUserAsync(int id);
    }
}