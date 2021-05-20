using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DataTransferObjects;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAuthorizationRepository authRepo;
        private readonly IMapper mapper;
        private readonly IConfiguration config;
        public AuthorizationService(IAuthorizationRepository authRepo, IMapper mapper, 
            IConfiguration config)
        {
            this.config = config;
            this.mapper = mapper;
            this.authRepo = authRepo;
        }

        public async Task<object> RegisterUser(RegisterUser registerUser)
        {
            registerUser.UserName = registerUser.UserName.ToLower();

            if (await authRepo.DoesUserExist(registerUser.UserName))
                throw new Exception();

            var userToBeCreated = mapper.Map<AppUser>(registerUser);

            var createdUser = await authRepo.Register(userToBeCreated, registerUser.Password);

            var userToReturn = mapper.Map<DetailedUser>(createdUser);

            return userToReturn;
        }

        public async Task<object> LogInUser(LogInUser loginUser)
        {
             var userToLogin = await authRepo.Login(loginUser.UserName.ToLower(), loginUser.Password);

            if (userToLogin == null)
                throw new Exception();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userToLogin.Id.ToString()),
                new Claim(ClaimTypes.Name, userToLogin.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var user = mapper.Map<DetailedUser>(userToLogin);

            return new
            {
                token = tokenHandler.WriteToken(token),
                user
            };
        }
    }
}