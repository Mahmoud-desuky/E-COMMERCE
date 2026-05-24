using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerce.Core.Entities.Identity;
using ECommerce.Infrastructure.Interface;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Infrastructure.Logic
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;

        private static string GetRequiredNonEmptyConfigValue(IConfiguration config, string key)
        {
            if (config is IConfigurationRoot root)
            {
                foreach (var provider in root.Providers.Reverse())
                {
                    if (provider.TryGet(key, out var value) && !string.IsNullOrWhiteSpace(value))
                    {
                        return value;
                    }
                }
            }

            var fallback = config[key];
            if (!string.IsNullOrWhiteSpace(fallback))
            {
                return fallback;
            }

            throw new InvalidOperationException($"Required configuration value '{key}' is missing or empty.");
        }

        public TokenService(IConfiguration config)
        {
            _config = config;
            var tokenKey = GetRequiredNonEmptyConfigValue(_config, "Token:Key");
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        }
        public string CreateToken(User user)
        {
            var Claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            };

            var credintionals = new SigningCredentials(_key,SecurityAlgorithms.HmacSha512Signature);
        
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = credintionals,
                Issuer = GetRequiredNonEmptyConfigValue(_config, "Token:Issuer"),
                Audience = _config["Token:Audience"],
            };
            var tokenHandler=new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}