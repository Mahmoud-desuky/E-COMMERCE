using ECommerce.Common.Interface;
using ECommerce.Common.Logic;
using ECommerce.Infrastructure.Interface;
using ECommerce.Infrastructure.Logic;

using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Extections
{
    public static class ApplicationServiceExtentions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<IBasketRepository,BasketRepository>();
            services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            
            services.Configure<ApiBehaviorOptions>(options=>
            {
               /* options.InvalidModelStateResponseFactory=actionContext =>
                {
                    var error=actionContext.ModelState
                    .Where(e=>e.Value.Errors.Count>0)
                    .SelectMany(e=>e.Value.Errors)
                    .Select(e=>e.ErrorMessage).ToArray();

                   // var errorResponse=new ApiValidationErrorResponse

                
                };*/
            });
            return services;
        }

    }
}