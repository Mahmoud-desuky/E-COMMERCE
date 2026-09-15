using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    
     protected string GetCustomerId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}