using System.Reflection;
using ECommerse.API.Extensions;
using ECommerse.API.Middleware;
using ECommerse.Infrastracture.Data;
using ECommerse.Infrastracture.Interface;
using ECommerse.Infrastracture.Logic;
using ECommerse.Infrastructure.Identity;
using ECommerse.Common.Interface;
using ECommerse.Common.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using ECommerse.Core.Entities.Identity;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductTypeService,ProductTypeService>();



// Add Scope of GenaricRepository
builder.Services.AddScoped(typeof(IGenaricRepository<>),typeof(GenaricRepository<>));


// Add services to the container.
builder.Services.AddDbContext<StoreContext>(x=>
    x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity

builder.Services.AddDbContext<ApplicationIdentityDbContext>(x =>
    x.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
{
   var Configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"), true);
   return ConnectionMultiplexer.Connect(Configuration);
});
builder.Services.AddScoped<UserManager<User>>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentityServices(builder.Configuration);


 builder.Services.Configure<ApiBehaviorOptions>(options=>
    {
         options.InvalidModelStateResponseFactory=ActionContext =>
          {
             var errors=ActionContext.ModelState
                    .Where(e=>e.Value.Errors.Count>0)
                    .SelectMany(x=>x.Value.Errors)
                    .Select(x=>x.ErrorMessage).ToArray();

           /*  var errorResponse=new ApiValidationErrorResponse
                   {
                          Errors=errors
                   };
*/
             return new BadRequestObjectResult(errors);
         };

    });
 builder.Services.AddSwaggerGen(c =>
    {
      c.SwaggerDoc("v1", new OpenApiInfo { Title = "ECommerce Application", Version = "v1" });
    });
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());



var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   app.UseDeveloperExceptionPage();
}
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
