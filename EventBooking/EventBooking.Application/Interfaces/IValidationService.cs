using System;
using System.Collections.Generic;
using System.Text;

namespace EventBooking.Application.Interfaces
{
    public interface IValidationService
    {
        Task ValidateAsync<T>(T dto);
    }
}
