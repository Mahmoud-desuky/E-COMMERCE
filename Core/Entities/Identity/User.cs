using Microsoft.AspNetCore.Identity;

namespace ECommerce.Core.Entities.Identity
{
    public class User : IdentityUser<int>
    {
        public Address Address { get; set; }

    }
}