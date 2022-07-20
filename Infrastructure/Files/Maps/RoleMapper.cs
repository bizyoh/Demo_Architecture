using AutoMapper;
using Domain.Entities;
using Application.Models.DTO.RoleDto;

namespace Infrastructure.Files.Maps
{
    public class RoleMapper : Profile
    {
        public RoleMapper()
        {
            CreateMap<CreateRoleDto, Role>();
            CreateMap<UpdateRoleDto, Role>();
            CreateMap<DetailRoleDto, Role>();
            CreateMap<Role,DetailRoleDto > ();
        }
    }
}
