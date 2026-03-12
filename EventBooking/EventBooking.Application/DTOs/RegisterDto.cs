using System;
using System.Collections.Generic;
using System.Text;

namespace EventBooking.Application.DTOs
{
    public record RegisterDto(string FullName,string Email, string Password);
}
