using ECommerce.Infrastructure.Interface;
using ECommerce.Infrastructure.Logic;
using ECommerce.Common.Interface;
using ECommerce.Common.Logic;
using Microsoft.AspNetCore.Identity;
using ECommerce.Core.Entities.Identity;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ECommerce.Infrastructure.Identity;
using StackExchange.Redis;
namespace ECommerce.Common.Registration
{
    public static class CommonRegistration
    {
        public static IServiceCollection RegisterCommonServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IBasketRepository, BasketRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductTypeService, ProductTypeService>();
            services.AddScoped<UserManager<User>>();
            // Add services to the container.
                        services.AddDbContext<StoreDbContext>(options =>
                        {
                                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                        });
            
            services.AddDbContext<ApplicationIdentityDbContext>(x =>
                x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
            var _Configuration = ConfigurationOptions.Parse(Configuration.GetConnectionString("Redis"), true);
            return ConnectionMultiplexer.Connect(_Configuration);
            });
                        return services;
                    }
                }
            }