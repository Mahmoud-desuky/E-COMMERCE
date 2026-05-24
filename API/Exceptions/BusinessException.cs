using System;
using System.Collections.Generic;
using System.Linq;

namespace ECommerce.API.Exceptions
{
    public class BusinessException : Exception
    {
          public string MessageCode;
        public new string Message;
        public List<object> Variables;
        public BusinessException(string messageCode, params object[] variables) : base(messageCode)
        {
            MessageCode = messageCode;
            Variables = variables.ToList();
        }

        public BusinessException(string messageCode, string message) : base(message)
        {
            MessageCode = messageCode;
            Message = message;
        }

    }
}
