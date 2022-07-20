using Application.Models.DTO.RoleDto;
using Domain.Entities;

namespace Application.Models.DTO.UserDto
{
    public class AdminUpdateUserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Status { get; set; }
        public IList<string> Roles { get; set; }
        public List<GetAllRoleDto> AllRoles { get; set; }
        public List<Invoice> Invoices { get; set; }
    }
}
