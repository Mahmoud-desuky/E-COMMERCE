using System.Reflection;
using ECommerce.API.Extensions;
using ECommerce.API.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using ECommerce.Common.Registration;
var builder = WebApplication.CreateBuilder(args);



builder.Services.RegisterCommonServices(builder.Configuration);



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
