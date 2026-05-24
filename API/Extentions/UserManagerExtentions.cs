using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ECommerce.Core.Entities.Identity;

namespace E_COMMERSE.API.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<User> FindByEmailWithAddressAsync(this UserManager<User> userManager,
         ClaimsPrincipal principal)
        {
            var email = principal?.Claims?.FirstOrDefault(a => a.Type == ClaimTypes.Email)?.Value;
            var user = await userManager.Users.Include(a => a.Address).FirstOrDefaultAsync(a => a.Email == email);
            return user;
        }
    }
}