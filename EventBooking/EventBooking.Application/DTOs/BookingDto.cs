using System;
using System.Collections.Generic;
using System.Text;

namespace EventBooking.Application.DTOs
{
    public class BookingDto
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string EventTitle { get; set; }
        public Guid UserId { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
