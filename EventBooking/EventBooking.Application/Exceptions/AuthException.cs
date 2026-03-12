using System;
using System.Collections.Generic;
using System.Text;

namespace EventBooking.Application.Exceptions
{
    public class AuthException : Exception
    {
        public AuthException(string message) : base(message)
        {
            
        }
    }
}
