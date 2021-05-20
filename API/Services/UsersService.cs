using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DataTransferObjects;
using API.Entities;
using API.Parameters;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository usersRepo;
        private readonly IOptions<CloudinarySettings> cloudinaryConfig;
        private readonly IMapper mapper;
        private readonly Cloudinary cloudinary;

        public UsersService(IUsersRepository usersRepo, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            this.mapper = mapper;
            this.usersRepo = usersRepo;
            this.cloudinaryConfig = cloudinaryConfig;
            this.mapper = mapper;
            this.usersRepo = usersRepo;

            var account = new Account(
                cloudinaryConfig.Value.CloudName,
                cloudinaryConfig.Value.ApiKey,
                cloudinaryConfig.Value.ApiSecret
            );

            this.cloudinary = new Cloudinary(account);
        }

        public async Task<object> GetSingleUser(int userId)
        {
            var user = await usersRepo.GetSingleUserAsync(userId);

            if (user == null) throw new Exception("Nie ma takiego użytkownika");

            var userToReturn = mapper.Map<DetailedUser>(user);

            return userToReturn;
        }

        public async Task<object> GetUsersToSearch(SearchParameters searchParameters)
        {
            var users = await usersRepo.GetUsersToSearchAsync(searchParameters.KeyWord.ToLower());

            var usersToReturn = mapper.Map<IEnumerable<DetailedUser>>(users);

            return usersToReturn;
        }

        public async Task<object> SendFriendRequest(int id, int myID)
        {
            // var myID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var me = await usersRepo.GetSingleUserAsync(myID);
            var friend = await usersRepo.GetSingleUserAsync(id);
            var request = await usersRepo.GetFriendRequestAsync(id, myID);
            var friendsRelation = await usersRepo.GetFriendsRelationAsync(myID, id);

            if (friend == null || me == null) throw new Exception("Nie znaleziono.");
            if (id == myID) throw new Exception("Nie możesz wysłać zaproszenia do siebie.");
            if (request != null) throw new Exception("Zaproszenie już istnieje.");

            if (friendsRelation == null)
            {
                request = new FriendRequest
                {
                    SenderId = myID,
                    RecipientId = id
                };

                await usersRepo.AddAsync(request);

                if (await usersRepo.SaveAllAsync())
                    return Task.CompletedTask;

                throw new Exception("Wystąpił błąd.");
            }

            throw new Exception("Już jesteście przyjaciółmi.");
        }

        public async Task<object> AcceptRequest(int senderId, int myID)
        {

            var me = await usersRepo.GetSingleUserAsync(myID);
            var friend = await usersRepo.GetSingleUserAsync(senderId);
            var request = await usersRepo.GetFriendRequestAsync(senderId, myID);

            if (friend == null || me == null) throw new Exception("Nie znaleziono.");
            if (senderId == myID) throw new Exception("Nie możesz zaakceptować zaproszenia do siebie.");
            if (request == null) throw new Exception("Zaproszenie nie istnieje.");

            usersRepo.Delete(request);

            if (!await usersRepo.SaveAllAsync()) throw new Exception("Wystąpił błąd.");

            var friendGotten = new Friend
            {
                UserId = myID,
                SecondUserId = senderId
            };

            var friendBeing = new Friend
            {
                UserId = senderId,
                SecondUserId = myID
            };

            await usersRepo.AddAsync(friendGotten);
            await usersRepo.AddAsync(friendBeing);

            if (await usersRepo.SaveAllAsync())
                return Task.CompletedTask;

            throw new Exception("Wystąpił błąd");
        }

        public async Task<object> DeclineRequest(int recipientId, int myID)
        {
            var me = await usersRepo.GetSingleUserAsync(myID);
            var friend = await usersRepo.GetSingleUserAsync(recipientId);
            var request = await usersRepo.GetFriendRequestAsync(recipientId, myID);

            if (friend == null || me == null) throw new Exception("Nie znaleziono.");
            if (recipientId == myID) throw new Exception("Wprowadź Id obiorcy");
            if (request == null) throw new Exception("Zaproszenie nie istnieje.");

            usersRepo.Delete(request);

            if (await usersRepo.SaveAllAsync())
                return Task.CompletedTask;

            throw new Exception("Wystąpił błąd.");
        }

        public async Task<object> DeleteFriend(int friendId, int myID)
        {
            if (friendId == myID) throw new Exception("Błąd.");

            var friendGotten = await usersRepo.GetFriendGottenAsync(friendId, myID);
            var friendBeing = await usersRepo.GetFriendBeingAsync(friendId, myID);

            if (friendGotten == null || friendBeing == null) throw new Exception("Nie znaleziono.");

            usersRepo.Delete(friendGotten);
            usersRepo.Delete(friendBeing);

            if (await usersRepo.SaveAllAsync())
                return Task.CompletedTask;

            throw new Exception("Błąd.");
        }

        public async Task<object> GetFriends(int userId)
        {
            var friends = await usersRepo.GetFriendsWithUserAsync(userId);

            var friendsToReturn = mapper.Map<IEnumerable<FriendForList>>(friends);

            return friendsToReturn;
        }

        public async Task<object> GetFriendRequests(int myID)
        {
            var requests = await usersRepo.GetFriendRequestsAsync(myID);

            var requestsToReturn = mapper.Map<IEnumerable<FriendRequestForList>>(requests);

            return requestsToReturn;
        }

        public async Task<object> GetFriendRelation(int id, int myID)
        {
            if (id == myID) throw new Exception("Marcin Najman to mój ulubiony sportowiec.");

            return await usersRepo.GetFriendGottenAsync(id, myID);
        }

        public async Task<IAsyncResult> ChangeAvatar(AvatarForChange avatarForChange)
        {
            var user = await usersRepo.GetSingleUserAsync(avatarForChange.UserId);

            var file = avatarForChange.File;

            var uploadResult = new ImageUploadResult();

            if (!string.IsNullOrEmpty(user.PhotoCloudinaryPublicId) &&
                !string.IsNullOrEmpty(user.PhotoUrl) && file.Length > 0)
            {
                var deleteParams = new DeletionParams(user.PhotoCloudinaryPublicId);
                var result = await cloudinary.DestroyAsync(deleteParams);

                if (result.Result == "ok")
                {
                    user.PhotoUrl = string.Empty;
                    user.PhotoCloudinaryPublicId = string.Empty;
                }
                else throw new Exception("Przy zmianie zdjęcia wystąpił bląd.");
            }

            var photoId = user.Id + "_main";

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
            user.PhotoUrl = uploadResult.Url.ToString();
            user.PhotoCloudinaryPublicId = uploadResult.PublicId;

            if (await usersRepo.SaveAllAsync())
                return Task.CompletedTask;

            throw new Exception("Coś poszło nie tak :(");
        }
    }
}