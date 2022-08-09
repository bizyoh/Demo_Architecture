using System.Security.Claims;
using Domain.Entities;
using Application.Models.DTO.UserDto;

namespace Application.Interfaces.UserService
{
    public interface IUserService
    {
        public Task<CreatedUserDto> Create(RegisterUserDto registerUserDto);
        public Task<bool> ChangeStatusUser(int id);
        public Task<bool> UpdateByUser(int id, UpdateUserDto userUpdateDto);
        public Task<bool> UpdateByAdmin(int id, PostAdminUpdateUserDto postAdminUpdateUserDto);
        public Task<User> FindUserById(int id);
        public List<GetAllUserDto>GetAll();
        public Task<AdminUpdateUserDto> AddRoleUser(int id,string role);
        public Task<bool> RemoveRoleUser(int id,string role);
        public Task<DetailUserDto> FindDetailUserDtoById(int id);
        public Task<AdminUpdateUserDto> FindAdminUpdateUserDtoById(int id);
        public Task<AccessToken> CreateAccessToken(User user);
        public Task<AccessedUserDto> Login(LoginUserDto loginUserDto);
        public GetUserRoleDto FindUserRoleDtoById(int id);
        public Task<AccessedUserDto> Refresh(string accessToken);

    }
}
