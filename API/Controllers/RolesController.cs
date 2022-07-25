using Application.Filters;
using Application.Interfaces.RoleService;
using Application.Models.DTO.RoleDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private IRoleService roleService;
        private IConfiguration configuration;
        private string BASE_URL;
        public RolesController(IRoleService _roleService, IConfiguration _configuration)
        {
            roleService = _roleService;
            configuration = _configuration;
            BASE_URL = configuration["BASE_URL"] + "roles";
        }

        [HttpGet]
        public IActionResult GetAllRoleDto()
        {
            var roles = roleService.GetAllRoles();
            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto role)
        {
            var result = await roleService.Create(role);
            if (result) return Ok();
            return BadRequest();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoleDto role)
        {
            if (roleService.GetRoleById(id) == null) throw new KeyNotFoundException(MessageErrors.RoleNotFound);
            var result = await roleService.Update(id, role);
            if (result) return Ok();
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await roleService.GetRoleById(id) == null) return NotFound();
            else
            {
                var result = await roleService.Delete(id);
                if (result == true)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userDTO = await roleService.GetRoleById(id);
            if(userDTO == null) return NotFound();
            return Ok(userDTO); 
        }

    }
}
