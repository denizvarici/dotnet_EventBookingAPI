using EventBooking.Application.Interfaces;
using FluentEmail.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBooking.Infrastructure.Services
{
    public class EmailService : IEMailService
    {
        private readonly IFluentEmail _fluentEmail;

        public EmailService(IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail;
        }

        public async Task SendEmailAync(string to, string subject, string body)
        {
            await _fluentEmail
            .To(to)
            .Subject(subject)
            .Body(body, isHtml: true)
            .SendAsync();
        }
    }
}
