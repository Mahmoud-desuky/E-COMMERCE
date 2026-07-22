using Microsoft.OpenApi.Models;

namespace ECommerce.API.Extension
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)  
        {
            services.AddSwaggerGen(c =>
            {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "ECommerce Application", Version = "v1" });
            });
            return services ;
        }
    
    public static IApplicationBuilder UserSwaggerDocumentation(this IApplicationBuilder application)
        {
            return application;
        }
    }
}