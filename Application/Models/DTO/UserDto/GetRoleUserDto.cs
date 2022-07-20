
using Application.Models.DTO.RoleDto;

namespace Application.Models.DTO.UserDto
{
    public class GetUserRoleDto
    {
        public int Id { get; set; }
        public List<DetailRoleDto> DetailRoleDtos { get; set; }
    }
}
