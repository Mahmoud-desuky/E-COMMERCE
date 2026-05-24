using System.Net;
using System.Text.Json;
using Azure.Core;
using ECommerce.API.Models;
using ECommerce.Core.Enums;
using ECommerce.API.Exceptions;
namespace ECommerce.API.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        
    private const string JsonContentType = "application/json";
    private readonly RequestDelegate _request;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _request = next;
    }
    public Task Invoke(HttpContext context) => InvokeAsync(context);

    async Task InvokeAsync(HttpContext context)
    {
        BaseResponse<object> response = new BaseResponse<object>();
        int statusCode = 500;
        try
        {
            await _request(context);
        }
        catch (BusinessException ex)
        {
            statusCode = (int)HttpStatusCode.UnprocessableContent;
            response.State = ResponseState.ValidationError;
            response.Message = string.IsNullOrEmpty(ex.Message) ? ex.MessageCode : ex.Message;
            await Return(context, statusCode, response);
        }

        catch (UnAuthorizedException ex)
        {
            statusCode = (int)HttpStatusCode.Unauthorized;
            response.State = ResponseState.UnAuthorized;
            response.Message = string.IsNullOrEmpty(ex.Message) ? "UnAuthorized" : ex.Message;
            await Return(context, statusCode, response);
        }

        catch (NotFoundException ex)
        {
            statusCode = (int)HttpStatusCode.NotFound;
            response.State = ResponseState.NotFound;
            response.Message = !string.IsNullOrEmpty(ex.Message) ? ex.Message : ex.Message;
            await Return(context, statusCode, response);
        }
        catch (Exception exception)
        {
            statusCode = (int)HttpStatusCode.InternalServerError;
            response.State = ResponseState.Error;
            response.Message = exception.Message;
            await Return(context, statusCode, response);
        }

    }
    private async Task Return(HttpContext context, int statusCode, BaseResponse<object> response)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = JsonContentType;
        await context.Response.WriteAsJsonAsync(response);
    }

    }
}