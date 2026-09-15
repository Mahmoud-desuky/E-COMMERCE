using System;
using ECommerce.Core.Enums;

namespace ECommerce.API.Models
{
    public class BaseResponse<T>
    {
        public T? Data { get; set; }
        public ResponseState State { get; set; }
        public string? Message { get; set; }

        public static BaseResponse<T> Success(T data,string message=""  )
        {
            return new BaseResponse<T>
            {
                Data = data,
                State = ResponseState.Success,
                Message = message
            };
        }   
        public static BaseResponse<T> Error(string message,Exception ex=null  )
        {
            return new BaseResponse<T>
            {
                Data = default,
                State = ResponseState.Error,
                Message = message
            };
        }   
        public static BaseResponse<T> NotFound(string Message)
        {
            return new BaseResponse<T>
            {
                Data =default,
                State=ResponseState.NotFound,
                Message=Message 
            };
        }

    } 
}