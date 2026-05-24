using System;
using ECommerce.Core.Enums;

namespace ECommerce.API.Models
{
    public class BaseResponse<T>
    {
        public T Data;
        public ResponseState State;
        public string Message;

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