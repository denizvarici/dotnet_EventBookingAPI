using System;
using System.Collections.Generic;
using System.Text;

namespace EventBooking.Domain.Entities
{
    public class Booking
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
        public DateTime BookingDate { get; set; }
        public bool IsCancelled { get; set; }


        
    }
}
