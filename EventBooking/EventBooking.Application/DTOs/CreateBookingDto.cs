using System;
using System.Collections.Generic;
using System.Text;

namespace EventBooking.Application.DTOs
{
    public class CreateBookingDto
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; } // Şimdilik manuel alıyoruz, sonra Identity'den çekeceğiz.
    }
}
