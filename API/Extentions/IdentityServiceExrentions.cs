using Microsoft.AspNetCore.Identity;
using ECommerce.Core.Entities.Identity;
using ECommerce.Infrastructure.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ECommerce.Infrastructure.Interface;
using ECommerce.Infrastructure.Logic;
namespace ECommerce.API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices (this IServiceCollection services,IConfiguration config)
        {
            var builder=services.AddIdentityCore<User>();
            

            builder= new IdentityBuilder(builder.UserType, builder.Services);

            //builder.AddScoped<ITokenService, TokenService>();
            builder.AddEntityFrameworkStores<ApplicationIdentityDbContext>();
            builder.AddSignInManager<SignInManager<User>>();
            var tokenKey = config.GetValue<string>("Token:Key")
                ?? config.GetValue<string>("JWT:Key");
            var issuer = config.GetValue<string>("Token:Issuer")
                ?? config.GetValue<string>("JWT:Issuer");

            if (string.IsNullOrWhiteSpace(tokenKey))
            {
                tokenKey = "DefaultDevelopmentTokenKey12345";
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                    ValidIssuer = issuer,
                    ValidateIssuer = !string.IsNullOrWhiteSpace(issuer),
                    ValidateAudience = false
                };
            });
            return services;
        }
      
    }
}