using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DataContxt context;
        public UsersRepository(DataContxt context)
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

        public async Task<AppUser> GetSingleUserAsync(int userId)
        {
            return await context.Users.FindAsync(userId);
        }

        public async Task<IEnumerable<AppUser>> GetUsersToSearchAsync(string keyWord)
        {
            return await context.Users.Where(u => u.UserName.ToLower().Contains(keyWord) 
                || u.Name.ToLower().Contains(keyWord) 
                || u.Surname.ToLower().Contains(keyWord)).ToListAsync();
        }

        
        public async Task<FriendRequest> GetFriendRequestAsync(int firstId, int secondId)
        {
            return await context.FriendRequests
                    .FirstOrDefaultAsync(x => (x.SenderId == firstId && x.RecipientId == secondId) 
                    || (x.SenderId == secondId && x.RecipientId == firstId));
        }

        public async Task<Friend> GetFriendsRelationAsync(int firstId, int secondId)
        {
            return await context.Friends
                    .FirstOrDefaultAsync(x => (x.UserId == firstId && x.SecondUserId == secondId) 
                    || (x.UserId == secondId && x.SecondUserId == firstId));
        }

        public async Task<IEnumerable<FriendRequest>> GetFriendRequestsAsync(int userId)
        {
            return await context.FriendRequests
                .Where(x => x.RecipientId == userId)
                .Include(x => x.Sender).ToListAsync();
        }

        public async Task<Friend> GetFriendGottenAsync(int id, int myID)
        {   // my friends
            return await context.Friends.FirstOrDefaultAsync(x => x.SecondUserId == id && x.UserId == myID);    
        }

        public async Task<Friend> GetFriendBeingAsync(int id, int myID)
        {
            // I'm being someone's friend
            return await context.Friends.FirstOrDefaultAsync(x => x.UserId == id && x.SecondUserId == myID);     
        }

        public async Task<IEnumerable<Friend>> GetFriendsWithUserAsync(int id)
        {
            // returns friend entity with user included so can be mapped afterwards
            return await context.Friends.Where(x => x.UserId == id)
                .Include(x => x.SecondUser)
                .Include(x => x.User).ToListAsync();
        }
        
    }
}