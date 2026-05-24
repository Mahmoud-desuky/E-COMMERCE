using ECommerce.Core.Entities.Identity;

namespace ECommerce.Infrastructure.Interface
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}