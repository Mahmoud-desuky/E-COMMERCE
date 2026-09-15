using Microsoft.AspNetCore.Identity;
using ECommerce.Core.Entities.Identity;
using ECommerce.Infrastructure.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
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
            var tokenKey = config["Token:Key"];
            var issuer = config["Token:Issuer"];

            if (string.IsNullOrWhiteSpace(tokenKey))
                throw new InvalidOperationException("Token:Key is missing.");

            if (string.IsNullOrWhiteSpace(issuer))
                throw new InvalidOperationException("Token:Issuer is missing.");


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

                options.Events=new JwtBearerEvents
                {
                    OnTokenValidated=async context=>
                    {
                        var userManager =context.HttpContext.RequestServices.GetRequiredService<UserManager<User>>();
                        
                        var userId= context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
                        
                        if(!int.TryParse(userId, out var id))
                        {
                            context.Fail("Invalid User Id");
                            return;
                        }

                         var user = await userManager.FindByIdAsync(id.ToString());

                        if (user == null)
                        {
                            context.Fail("User not found.");
                            return;
                        }

                        var tokenStamp = context.Principal?
                            .FindFirst("security_stamp")?.Value;

                        if (tokenStamp != user.SecurityStamp)
                        {
                            context.Fail("Token is no longer valid.");
                        }


                    }
                };
            }
        
            );
            return services;
        }
      
    }
}