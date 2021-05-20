using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DataTransferObjects;
using API.Entities;
using API.Parameters;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUsersService usersService;
        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetSingleUser(int userId)
        {
            try
            {
                return Ok(await usersService.GetSingleUser(userId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersToSearch([FromQuery] SearchParameters searchParameters)
        {
            try
            {
                return Ok(await usersService.GetUsersToSearch(searchParameters));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("invite/{id}")]
        public async Task<IActionResult> SendFriendRequest(int id)
        {
            try
            {
                var myID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                await usersService.SendFriendRequest(id, myID);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("invite/{senderId}/accept")]
        public async Task<IActionResult> AcceptRequest(int senderId)
        {
            try
            {
                var myID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                await usersService.AcceptRequest(senderId, myID);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpDelete("invite/{recipientId}/decline")]
        public async Task<IActionResult> DeclineRequest(int recipientId)
        {
            try
            {
                var myID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                await usersService.DeclineRequest(recipientId, myID);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("friends/{friendId}")]
        public async Task<IActionResult> DeleteFriend(int friendId)
        {
            try
            {
                var myID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                await usersService.DeleteFriend(friendId, myID);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{userId}/friends")]
        public async Task<IActionResult> GetFriends(int userId)
        {
            try
            {
                return Ok(await usersService.GetFriends(userId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("requests")]
        public async Task<IActionResult> GetFriendRequests()
        {
            try
            {
                var myID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                return Ok(await usersService.GetFriendRequests(myID));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("relation/{id}")]
        public async Task<IActionResult> GetFriendRelation(int id)
        {
            try
            {
                var myID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                return Ok(await usersService.GetFriendRelation(id, myID));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeAvatar([FromForm] AvatarForChange avatarForChange)
        {
            try
            {
                var myID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                avatarForChange.UserId = myID;
                
                await usersService.ChangeAvatar(avatarForChange);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}