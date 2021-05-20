using System.Threading.Tasks;
using API.Entities;

namespace API.Data
{
    public interface IAuthorizationRepository
    {
         Task<bool> DoesUserExist(string username);
         Task<AppUser> Login(string username, string password);
         Task<AppUser> Register(AppUser user, string password);
    }
}