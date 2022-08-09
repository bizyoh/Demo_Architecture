using AutoMapper;
using Domain.Entities;
using Application.Models.DTO.UserDto;

namespace Infrastructure.Files.Maps
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<RegisterUserDto, User>();
            CreateMap<User, CreatedUserDto>();
            CreateMap<User, DetailUserDto>();
            CreateMap<User, AccessedUserDto>();
            CreateMap<User, AdminUpdateUserDto>();
            CreateMap<AdminUpdateUserDto, User>();
            CreateMap<User, GetUserRoleDto>();
            CreateMap<User, AccessedUserDto>();
            // CreateMap<PostAdminUpdateUserDto, User>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<PostAdminUpdateUserDto, User>();
             
        }
    }
}
