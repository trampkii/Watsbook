using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DataTransferObjects;
using API.Entities;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService service;
        public AuthorizationController(IAuthorizationService service)
        {
            this.service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterUser registerUser)
        {
            try 
            {
                return Ok(await service.RegisterUser(registerUser));
            }
            catch
            {
                return BadRequest("User already exists. Please, change your username");
            }
            
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogInUser(LogInUser loginUser)
        {
            try
            {
                return Ok(await service.LogInUser(loginUser));
            }
            catch (System.Exception)
            {
                return Unauthorized();
            }

            
        }
    }
}