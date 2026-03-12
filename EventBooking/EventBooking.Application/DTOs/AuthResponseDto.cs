using System;
using System.Collections.Generic;
using System.Text;

namespace EventBooking.Application.DTOs
{
    public record AuthResponseDto(bool IsSuccess, string Token, string Message);
}
