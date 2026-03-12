using System;
using System.Collections.Generic;
using System.Text;

namespace EventBooking.Domain.Entities
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public int RemainingCapacity { get; set; }

        public ICollection<Booking> Bookings { get; set; }


        public byte[] RowVersion { get; set; }
    }
}
