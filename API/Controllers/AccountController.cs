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
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var email=HttpContext.User?.Claims?.FirstOrDefault(a=>a.Type==ClaimTypes.Email)?.Value;

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) throw new UnAuthorizedException();
            var res=new UserDTO
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                Address= user.Address.ToString(),
                FullName = user.UserName
            };
            return Ok(BaseResponse<UserDTO>.Success(res));
        }
        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExists([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.UserName);
            if (user == null) 
                throw new UnAuthorizedException();
            
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
                 throw new UnAuthorizedException();
            
            var res = new UserDTO
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                Address= user.Address.ToString(),
                FullName = user.UserName
            };
            return Ok(BaseResponse<UserDTO>.Success(res));
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDto)
        {
            if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
                return BadRequest(new ApiResponse(400, "User with this email already exists"));
            
            var user = new User
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName
           };
            
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) 
                return BadRequest(new ApiResponse(400));
            var res = new UserDTO
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                Address= user.Address.ToString(),
                FullName = user.UserName
            };
            return Ok(BaseResponse<UserDTO>.Success(res));
        }
    }
}