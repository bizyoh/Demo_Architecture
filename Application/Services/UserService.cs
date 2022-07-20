using Application.Exceptions;
using Application.Filters;
using Application.Interfaces.InvoiceService;
using Application.Interfaces.RoleService;
using Application.Interfaces.UserService;
using Application.Models.DTO.InvoiceDto;
using Application.Models.DTO.UserDto;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace Application.Services
{
    public class UserService : IUserService
    {
        private UserManager<User> userManager;
        private SignInManager<User> signManager;
        private RoleManager<Role> roleManager;
        private IRoleService roleService;
        private IMapper mapper;
        private IInvoiceService invoiceService;
        private IConfiguration configuration;
        private AppDBContext db;
        public UserService(IInvoiceService _invoiceService, IRoleService _roleService, UserManager<User> _userManager, SignInManager<User> _signInManager, RoleManager<Role> _roleManager, IMapper _mapper, IConfiguration _configuration, AppDBContext _db)
        {
            invoiceService = _invoiceService;
            userManager = _userManager;
            roleService = _roleService;
            signManager = _signInManager;
            roleManager = _roleManager;
            mapper = _mapper;
            db = _db;
            configuration = _configuration;
        }

        public async Task<AdminUpdateUserDto> AddRoleUser(int id, string role)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null) throw new AppException(MessageErrors.NotFound);
            if (string.IsNullOrEmpty(role)) throw new AppException(MessageErrors.NoRoleAdd);
            else
            {
                var roleCurrent = await roleManager.FindByNameAsync(role);
                if (roleCurrent != null && await userManager.IsInRoleAsync(user, roleCurrent.Name) == false)
                {
                    var result = await userManager.AddToRoleAsync(user, role);
                    var adminUpdateDto = await FindAdminUpdateUserDtoById(id);
                    return adminUpdateDto;
                }
            }
            return null;
        }

        public async Task<bool> ChangeStatusUser(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (await userManager.IsInRoleAsync(user, "Admin"))
            {
                throw new AppException(MessageErrors.AdminForbbiden);
            }
            if (user == null)
            {
                throw new KeyNotFoundException(MessageErrors.NotFound);
            }
            user.Status = !user.Status;
            IdentityResult res = await userManager.UpdateAsync(user);
            if (res.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async Task<CreatedUserDto> Create(RegisterUserDto registerUserDto)
        {
            var createdUserDto = new CreatedUserDto();
            if (registerUserDto is null) return null;
            else
            {
                var checkUser = await userManager.FindByNameAsync(registerUserDto.UserName);
                if (checkUser != null)
                {
                    throw new AppException(MessageErrors.UniqueUser);
                }
                var checkEmail = await userManager.FindByEmailAsync(registerUserDto.Email);
                if (checkEmail != null)
                {
                    throw new AppException(MessageErrors.UniqueEmail);
                }
                var user = new User();
                mapper.Map(registerUserDto, user);
                user.Status = true;
                var a = db.Database.CreateExecutionStrategy();
                await a.ExecuteAsync(async () =>
                {
                    using var transaction = await db.Database.BeginTransactionAsync();
                    var result = await userManager.CreateAsync(user, registerUserDto.Password);
                    if (result.Succeeded)
                    {

                        await userManager.AddToRoleAsync(user, "user");
                        await transaction.CommitAsync();
                        mapper.Map(user, createdUserDto);
                    }
                    else
                    {
                        transaction?.Rollback();
                    }
                });
            }
            return createdUserDto;
        }

        public async Task<AccessToken> CreateAccessToken(User user)
        {
            var roles = await userManager.GetRolesAsync(user);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:key"]));
            var signature = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            foreach (var userRole in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            var expirityTime = int.Parse(configuration["JWT:AccessTokenValidityInMinutes"]);
            var token = new JwtSecurityToken(
                configuration["Jwt:issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(expirityTime),
                signingCredentials: signature);
            return new AccessToken { Token = new JwtSecurityTokenHandler().WriteToken(token), ExpirityTime = DateTime.Now.AddMinutes(expirityTime) };
        }



        public async Task<DetailUserDto> FindDetailUserDtoById(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user != null)
            {
                var roles = await userManager.GetRolesAsync(user);
                var detailUserDto = mapper.Map<DetailUserDto>(user);
                var invoices = mapper.Map<List<GetAllInvoiceDto>>(invoiceService.FindInvoiceByUserId(id));
                detailUserDto.Roles = roles;
                detailUserDto.Invoices = invoices;
                return detailUserDto;
            }
            throw new KeyNotFoundException(MessageErrors.NotFound);

        }
        public async Task<AdminUpdateUserDto> FindAdminUpdateUserDtoById(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user != null)
            {
                var roles = await userManager.GetRolesAsync(user);
                var allRoles = roleService.GetAllRoles();
                var adminUpdateUserDto = mapper.Map<AdminUpdateUserDto>(user);
                adminUpdateUserDto.Roles = roles;
                adminUpdateUserDto.AllRoles = allRoles;
                return adminUpdateUserDto;
            }
            throw new KeyNotFoundException(MessageErrors.NotFound);
        }

        public async Task<User> FindUserById(int id)
        {
            return await userManager.FindByIdAsync(id.ToString());
        }

        public List<GetAllUserDto> GetAll()
        {
            var GetAllUserDtos = userManager.Users.Include(x => x.Invoices).Select(p => new GetAllUserDto
            {
                Id = p.Id,
                UserName = p.UserName,
                FirstName = p.FirstName,
                LastName = p.LastName,
                PhoneNumber = p.PhoneNumber,
                Address = p.Address,
                Status = p.Status,
                Email = p.Email,
            }).ToList();


            return GetAllUserDtos;
        }

        public async Task<AccessedUserDto> Login(LoginUserDto loginUserDto)
        {
            var user = await userManager.FindByNameAsync(loginUserDto.UserNameOrEmail);
            if (user != null && user.Status != false)
            {
                SignInResult resultLogInWithUserName = await signManager.CheckPasswordSignInAsync(user, loginUserDto.Password, false);
                if (resultLogInWithUserName.Succeeded)
                {
                    var accessedUserDto = mapper.Map<AccessedUserDto>(user);
                    var token = await CreateAccessToken(user);
                    accessedUserDto.AccessToken = token.Token;
                    user.RefreshToken = GenerateRefreshToken().Token;
                    await userManager.UpdateAsync(user);
                    return accessedUserDto;
                }
            }
            else
            {
                user = await userManager.FindByEmailAsync(loginUserDto.UserNameOrEmail);
                if (user != null && user.Status != false)
                {
                    var resultLogInWithEmail = await signManager.CheckPasswordSignInAsync(user, loginUserDto.Password, false);
                    if (resultLogInWithEmail.Succeeded)
                    {
                        var accessedUserDto = mapper.Map<AccessedUserDto>(user);
                        var token = await CreateAccessToken(user);
                        accessedUserDto.AccessToken = token.Token;
                        user.RefreshToken = GenerateRefreshToken().Token;
                        await userManager.UpdateAsync(user);
                        return accessedUserDto;
                    }
                }
            }
            throw new KeyNotFoundException(MessageErrors.Login);
        }

        public async Task<bool> RemoveRoleUser(int id, string role)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null) throw new KeyNotFoundException(MessageErrors.NotFound);
            if (string.IsNullOrEmpty(role)) throw new AppException(MessageErrors.NoRoleRemove);
            else
            {
                var result = await roleManager.RoleExistsAsync(role);
                if (result)
                {
                    if (await userManager.IsInRoleAsync(user, role))
                    {
                        IdentityResult result2 = await userManager.RemoveFromRoleAsync(user, role);
                        if (result2.Succeeded) return true;
                    }
                }
            }
            return false;
        }

        public async Task<bool> UpdateByUser(int id, UpdateUserDto userUpdateDto)
        {
            var user = await FindUserById(id);
            if (user is null) return false;
            if (!await userManager.CheckPasswordAsync(user, userUpdateDto.CurrentPassword)) return false;
            else
            {
                user = mapper.Map<User>(userUpdateDto);
                var a = db.Database.CreateExecutionStrategy();
                await a.ExecuteAsync(async () =>
                {
                    using var transaction = await db.Database.BeginTransactionAsync();
                    var result1 = await userManager.ChangePasswordAsync(user, userUpdateDto.CurrentPassword, userUpdateDto.NewPassword);
                    var result2 = await userManager.UpdateAsync(user);
                    if (result1.Succeeded && result2.Succeeded) await transaction.CommitAsync();
                    else transaction.Rollback();
                });
            }
            return false;
        }

        public async Task<bool> UpdateByAdmin(int id, PostAdminUpdateUserDto postAdminUpdateUserDto)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            var listUsers = db.Users.Where(x => x.Email == postAdminUpdateUserDto.Email && x.Email != user.Email).ToList();
            if (listUsers.Count >= 1)
            {
                throw new AppException(MessageErrors.UniqueEmail);
            }
            if (user is null) throw new AppException(MessageErrors.NotFound);
            mapper.Map(postAdminUpdateUserDto, user);

            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded) return true;
            else throw new AppException(MessageErrors.UpdateUserFail);
            return false;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            RefreshToken token = new RefreshToken();
            token.Token = Convert.ToBase64String(randomNumber);
            return token;
        }

        public GetUserRoleDto FindUserRoleDtoById(int id)
        {
            var user = db.Users.FirstOrDefault(x => x.Id == id);
            GetUserRoleDto userDto = mapper.Map<GetUserRoleDto>(user);
            List<DetailRoleDto> roleDtos = mapper.Map<List<DetailRoleDto>>(user.Roles);
            userDto.DetailRoleDtos = roleDtos;
            return userDto;
        }
    }
}
