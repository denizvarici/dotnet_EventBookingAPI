using System;
using System.Collections.Generic;
using System.Text;

namespace EventBooking.Application.Interfaces
{
    public interface IEMailService
    {
        Task SendEmailAync(string to, string subject, string body);
    }
}
