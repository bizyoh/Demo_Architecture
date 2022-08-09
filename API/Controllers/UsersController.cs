using Application.Filters;
using Application.Interfaces.UserService;
using Application.Models.DTO.UserDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace WebUI.Controllers
{

    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService userService;
        private IConfiguration configuration;
        private string BASE_URL;
        public UsersController(IUserService _userService, IConfiguration _configuration)
        {
            userService = _userService;
            configuration = _configuration;
            BASE_URL = configuration["BASE_URL"] + "users";
        }

        [HttpGet]
        public IActionResult GetAllUserDto()
        {
            var users = userService.GetAll();
            
            return Ok(users);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(RegisterUserDto registerUserDto)
        {
            var createdUser = await userService.Create(registerUserDto);
            return Created(BASE_URL + "/" + createdUser.Id, createdUser);
        }

        //[HttpPost("refresh")]
        //[AllowAnonymous]
        //public async Task<IActionResult> Refresh(string accessToken)
        //{
        //   var user=await userService.Refresh(accessToken);
        //    if (user != null)
        //    {
        //        return Ok(user);
        //    }
        //    return BadRequest();
        //}


        [HttpPut]
        [Authorize(Roles ="User")]
        public async Task<IActionResult> UpdateByuser(int id, UpdateUserDto updateUserDto)
        {
            var user = await userService.FindUserById(id);
            if (user == null) return NotFound();
            else
            {
                var Result = await userService.UpdateByUser(id, updateUserDto);
                if (Result == true)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        [HttpPut("admin/{id}")]
        public async Task<IActionResult> UpdateByAdmin(int id,[FromBody] PostAdminUpdateUserDto postAdminUpdateUserDto)
        {
            var user = await userService.FindUserById(id);
            if (user == null) return NotFound(MessageErrors.NotFound);
            else
            {
                var Result = await userService.UpdateByAdmin(id, postAdminUpdateUserDto);
                if (Result == true)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }

        [HttpGet("{id}/status")]
        public async Task<IActionResult> ChangeStatusUser(int id)
        {
            var user = await userService.FindUserById(id);
            if (user == null) return NotFound();
            else
            {
                var Result = await userService.ChangeStatusUser(id);
                if (Result == true)
                {
                    return Ok();
                }
            }
            return BadRequest();
        }


        [HttpGet("admin/{id}/role")]
        public async Task<IActionResult> FindUserRoleById(int id)
        {
            var user = await userService.FindUserById(id);
            if (user == null) return NotFound();
            else
            {
                var userDto = userService.FindUserRoleDtoById(id);
                if (userDto != null)
                {
                    return Ok(userDto);
                }
            }
            return BadRequest();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userDTO = await userService.FindUserById(id);
            if (userDTO == null) return NotFound();
            return Ok(userDTO);
        }

        [HttpGet("admin/{id}/detail")]
        public async Task<IActionResult> FindDetailUserDtoById(int id)
        {
            var userDTO = await userService.FindDetailUserDtoById(id);
            if (userDTO == null) return NotFound(MessageErrors.NotFound);
            return Ok(userDTO);
        }

        [HttpGet("admin/{id}")]
        public async Task<IActionResult> FindAdminUpdateUserDtoById(int id)
        {
            var userDTO = await userService.FindAdminUpdateUserDtoById(id);
            if (userDTO == null) return NotFound(MessageErrors.NotFound);
            return Ok(userDTO);
        }

        [HttpPost("RemoveRole/{id}")]
        public async Task<IActionResult> RemoveRolesUser(int id,[FromBody] string role)
        {
            var result = await userService.RemoveRoleUser(id,role);
            if (result) return Ok();
            return BadRequest();
        }

        [HttpPost("AddRole/{id}")]
        public async Task<IActionResult> AddRolesUser(int id,[FromBody] string role)
        {
            var user = await userService.AddRoleUser(id,role);
            if (user!=null) return Ok(user);
            return BadRequest();
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserDto lgoinUserDto)
        {
            var user = await userService.Login(lgoinUserDto);
            if (user!=null) return Ok(user);
            return BadRequest();
        }
    }
}
