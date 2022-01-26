using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using WorkforceManagement.DAL.Entities;
using WorkforceManagement.Models.User;

namespace WorkforceManagement.WEB.Profiles
{
    [ExcludeFromCodeCoverage]
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, ViewUserModel>()
                .ReverseMap();

            CreateMap<User, CreateUserModel>()
                .ReverseMap();

            CreateMap<User, EditUserModel>()
                .ReverseMap();
        }
    }
}
