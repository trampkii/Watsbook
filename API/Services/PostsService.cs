using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.DataTransferObjects;
using API.Entities;
using API.Helpers;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class PostsService : IPostsService
    {
        private Cloudinary cloudinary;
        private readonly IOptions<CloudinarySettings> cloudinaryConfig;
        private readonly IUsersRepository usersRepo;
        private readonly IPostsRepostiory postsRepo;
        private readonly IMapper mapper;
        public PostsService(IUsersRepository usersRepo, IPostsRepostiory postsRepo,
        IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            this.cloudinaryConfig = cloudinaryConfig;
            this.mapper = mapper;
            this.postsRepo = postsRepo;
            this.usersRepo = usersRepo;

            var account = new Account(
                cloudinaryConfig.Value.CloudName,
                cloudinaryConfig.Value.ApiKey,
                cloudinaryConfig.Value.ApiSecret
            );

            this.cloudinary = new Cloudinary(account);
        }

        public async Task<object> AddPost(PostForCreation postForCreation, int myID)
        {
            //var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var user = await usersRepo.GetSingleUserAsync(myID);

            var file = postForCreation.File;

            var uploadResult = new ImageUploadResult();

            // Generowanie randomowego publicId zdjęcia do Cloudinary
            var photoId = ("user_" + user.Id + "-").GenerateRandomString();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation()
                            .Width(500).Height(500).Crop("fill").Gravity("face"),
                        PublicId = photoId
                    };

                    uploadResult = cloudinary.Upload(uploadParams);
                }
            }

            postForCreation.PhotoUrl = uploadResult.Url.ToString();
            postForCreation.CloudinaryPublicId = uploadResult.PublicId;

            var post = mapper.Map<Post>(postForCreation);
            post.User = user;
            post.UserId = myID;

            await postsRepo.AddAsync(post);

            if (await postsRepo.SaveAllAsync())
            {
                return Task.CompletedTask;
            }

            throw new Exception("Coś poszło nie tak :(");
        }

        public async Task<object> GetPostsForUser(int id)
        {
            var user = await usersRepo.GetSingleUserAsync(id);

            if (user == null) throw new Exception("Nie znaleziono.");

            var posts = await postsRepo.GetPostsForUserAsync(id);

            var postsToReturn = mapper.Map<IEnumerable<DetailedPost>>(posts);

            return postsToReturn;
        }

        public async Task<object> GetPostsFromFriends(int myID)
        {

            var posts = await postsRepo.GetPostsFromFriendsAsync(myID);

            var postsToReturn = mapper.Map<IEnumerable<DetailedPost>>(posts);

            return postsToReturn;
        }

        public async Task<object> DeletePost(int id, int myID)
        {
            var post = await postsRepo.GetPostAsync(id);

            if (post == null) throw new Exception("Nie znaleziono.");
            if (post.UserId != myID) throw new Exception("Brak autoryzacji.");

            if (!string.IsNullOrEmpty(post.CloudinaryPublicId))
            {
                // Sprawdzanko czy post posiada cloudinary public id

                var deleteParams = new DeletionParams(post.CloudinaryPublicId);

                var result = await cloudinary.DestroyAsync(deleteParams);

                if (result.Result == "ok")
                    postsRepo.Delete(post);
            }
            else postsRepo.Delete(post);

            if (await postsRepo.SaveAllAsync())
                return Task.CompletedTask;

            throw new Exception("Coś poszło nie tak");
        }

        public async Task<IAsyncResult> ReactPost(int postId, int myID)
        {
            var like = await postsRepo.GetPostLikeAsync(postId, myID);

            if(like != null)
            {
                postsRepo.Delete(like);
            }
            else 
            {
                var likeIt = new LikeIt
                {
                    UserId = myID,
                    PostId = postId
                };
            
                await postsRepo.AddAsync(likeIt);
            }

            if(await postsRepo.SaveAllAsync()) return Task.CompletedTask;

            throw new Exception("Wystąpił błąd.");
        }

        public async Task<object> GetPostLikes(int postId) 
        {
            return await postsRepo.GetPostLikesAsync(postId);
        }

        public async Task<object> GetPostLike(int postId, int myID)
        {
            return await postsRepo.GetPostLikeAsync(postId, myID);
        }

        
    }
}