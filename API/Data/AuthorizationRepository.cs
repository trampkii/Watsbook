using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using API.Helpers;

namespace API.Data
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        private readonly DataContxt context;
        public AuthorizationRepository(DataContxt context)
        {
            this.context = context;
        }
        public async Task<bool> DoesUserExist(string username)
        {
            if (await context.Users.AnyAsync(x => x.UserName == username))
                return true;
            
            return false;
        }

        public async Task<AppUser> Login(string username, string password)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == username);

            if (user == null) return null; // user with entered username doesn't exist

            if(!PasswordHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) return null;
                // password hash doesn't match with that in db

            return user;
        }

        public async Task<AppUser> Register(AppUser user, string password)
        {
            byte[] passwordHash, passwordSalt;
            PasswordHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await context.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }
    }
}