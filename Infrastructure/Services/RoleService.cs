using AutoMapper;
using Microsoft.AspNetCore.Identity;

using Domain.Entities;
using Application.Models.DTO.RoleDto;
using Application.Interfaces.RoleService;
using Application.Exceptions;
using WebUI.Filters;

namespace Infrastructure.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private RoleManager<Role> roleManager;
        private IMapper mapper;
        public RoleService(RoleManager<Role> _roleManager, IMapper _mapper)
        {
            roleManager = _roleManager;
            mapper = _mapper;
        }

        public async Task<bool> Create(CreateRoleDto roleDto)
        {
            var role = mapper.Map<Role>(roleDto);
            if(await roleManager.RoleExistsAsync(role.Name)) throw new AppException(MessageErrors.UniqueRole);
            var result = await roleManager.CreateAsync(role);
            if (result.Succeeded) return true;
            return false;
        }

        public async Task<bool> Delete(int id)
        {
            var role = await roleManager.FindByIdAsync(id.ToString());
            if (role != null)
            {
                var result = await roleManager.DeleteAsync(role);
                if (result.Succeeded) return true;
            }
            return false;
        } 

        public async Task<bool> Delete(string name)
        {
            var role = await roleManager.FindByNameAsync(name);
            if (role != null)
            {
                var result = await roleManager.DeleteAsync(role);
                if (result.Succeeded) return true;
            }
            return false;
        }

        public List<GetAllRoleDto> GetAllRoles()
        {
            return roleManager.Roles.Select(p => new GetAllRoleDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
            }).ToList();
        }

        public async Task<DetailRoleDto> GetRoleById(int id)
        {
            var role = await roleManager.FindByIdAsync(id.ToString());
            if (role == null) throw new KeyNotFoundException(MessageErrors.RoleNotFound);
            else
            {
                var detailRoleDto = mapper.Map<DetailRoleDto>(role);
                return detailRoleDto;
            } 
            return null;
        }

        public async Task<Role> GetRoleByName(string roleName)
        {
            var role = await roleManager.FindByIdAsync(roleName);
            if (role != null) return role;
            return null;
        }

        public async Task<bool> Update(int id, UpdateRoleDto updateRoleDto)
        {
            var currentRole = await roleManager.FindByIdAsync(id.ToString());
            if(currentRole is null) throw new KeyNotFoundException(MessageErrors.RoleNotFound);
            else 
            {
                mapper.Map(updateRoleDto,currentRole);
                if(await roleManager.RoleExistsAsync(currentRole.Name)) throw new AppException(MessageErrors.RoleNameExist);
                IdentityResult result =await roleManager.UpdateAsync(currentRole);
                if (result.Succeeded) return true;
            }
            return false;
        }

     
    }
}
