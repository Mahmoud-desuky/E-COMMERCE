using ECommerce.API.Controllers;
using ECommerce.API.Errors;
using ECommerce.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Common.DTOs;
using ECommerce.API.Exceptions;
using ECommerce.API.Models;
using ECommerce.Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ECommerce.Common.DTOs.UserAccount;
namespace E_COMMERSE.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : BaseApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        public AccountController(UserManager<User> userManager,
         SignInManager<User> signInManager,
         ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }
        [Authorize]
        [HttpGet]
        public async Task<BaseResponse<UserDTO>> GetCurrentUser()
        {
            var email=HttpContext.User?.Claims?.FirstOrDefault(a=>a.Type==ClaimTypes.Email)?.Value;

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new UnAuthorizedException();
            var res=new UserDTO
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                Address= user.Address?.ToString()??"",
                FullName = user.UserName
            };
            return BaseResponse<UserDTO>.Success(res);
        }
        [HttpGet("emailexists")]
        public async Task<BaseResponse<bool>> CheckEmailExists([FromQuery] string email)
        {
            return BaseResponse<bool>.Success(await _userManager.FindByEmailAsync(email) != null);
        }


        [HttpPost("login")]
        public async Task<BaseResponse<UserDTO>> Login(LoginDTO loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) 
                return BaseResponse<UserDTO>.Error("Invalid email or password");            
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
               return BaseResponse<UserDTO>.Error("Invalid email or password");            
            
            var res = new UserDTO
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                Address= user.Address?.ToString()??"",
                FullName = user.UserName
            };
            return BaseResponse<UserDTO>.Success(res);
        }
        [HttpPost("register")]
        public async Task<BaseResponse<UserDTO>> Register(RegisterDTO registerDto)
        {
            if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
                return BaseResponse<UserDTO>.Error("User with this email already exists");
            
            var user = new User
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName
           };
            
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) 
                return BaseResponse<UserDTO>.Error(result.Errors.FirstOrDefault()?.Description ?? "User creation failed");
            var res = new UserDTO
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                Address= user.Address?.ToString()??"",
                FullName = user.UserName
            };
            return BaseResponse<UserDTO>.Success(res);
        }

        [HttpPatch()]
        public async Task<BaseResponse<UserDTO>>ChangePassword(ChangePasswordDto changePasswordDto)
        {

            var userId = GetCustomerId();
            var user = await _userManager.FindByIdAsync(userId);
            var findUserByEmail=(changePasswordDto.Email!=null)? await _userManager.FindByEmailAsync(changePasswordDto.Email):null;
            if (user == null&& findUserByEmail==null)
                return BaseResponse<UserDTO>.Error("User not found");

            var result = await _userManager.ChangePasswordAsync(user??findUserByEmail, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            if (!result.Succeeded)
                return BaseResponse<UserDTO>.Error(result.Errors.FirstOrDefault()?.Description ?? "Password change failed");

            var res = new UserDTO
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                Address= user.Address?.ToString()??"",
                FullName = user.UserName
            };
            return BaseResponse<UserDTO>.Success(res);
            
        }
    }
}