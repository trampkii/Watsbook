using API.DataTransferObjects;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {  
            CreateMap<AppUser, RegisterUser>().ReverseMap();

            CreateMap<AppUser, DetailedUser>()
                .ForMember(dest => dest.Age, src => src.MapFrom(u => u.DateOfBirth.CalculateAge())); // add main photo url mapping

            CreateMap<Friend, FriendForList>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.SecondUserId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SecondUser.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.SecondUser.Surname))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.SecondUser.UserName))
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.SecondUser.PhotoUrl));

            CreateMap<FriendRequest, FriendRequestForList>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.SenderId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Sender.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Sender.Surname))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Sender.UserName))
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Sender.PhotoUrl));
            
            CreateMap<PostForCreation, Post>();
            
            CreateMap<Post, DetailedPost>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.User.Surname))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.UserPhotoUrl, opt => opt.MapFrom(src => src.User.PhotoUrl));
        }
    }
}