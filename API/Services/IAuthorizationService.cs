using System.Threading.Tasks;
using API.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public interface IAuthorizationService
    {
         Task<object> RegisterUser(RegisterUser registerUser);
         Task<object> LogInUser(LogInUser loginUser);
    }
}