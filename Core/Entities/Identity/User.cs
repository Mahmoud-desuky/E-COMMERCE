using Microsoft.AspNetCore.Identity;

namespace ECommerce.Core.Entities.Identity
{
    public class User : IdentityUser
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }

    }
}