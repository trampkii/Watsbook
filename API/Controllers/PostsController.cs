using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DataTransferObjects;
using API.Entities;
using API.Helpers;
using API.Services;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostsService postsService;
        public PostsController(IPostsService postsService)
        {
            this.postsService = postsService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPost([FromForm] PostForCreation postForCreation)
        {

            try
            {
                var myID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                await postsService.AddPost(postForCreation, myID);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostsForUser(int id)
        {
            try
            {
                return Ok(await postsService.GetPostsForUser(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetPostsFromFriends()
        {

            try
            {
                var myID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                return Ok(await postsService.GetPostsFromFriends(myID));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                var myID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                await postsService.DeletePost(id, myID);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("{postId}/like")]
        public async Task<IActionResult> LikePost(int postId)
        {
            try
            {
                var myID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                await postsService.ReactPost(postId, myID);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{postId}/likes")]
        public async Task<IActionResult> GetPostLikes(int postId)
        {
            try
            {
                return Ok(await postsService.GetPostLikes(postId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{postId}/like")]
        public async Task<IActionResult> GetPostLike(int postId)
        {
            try
            {
                var myID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                return Ok(await postsService.GetPostLike(postId, myID));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}