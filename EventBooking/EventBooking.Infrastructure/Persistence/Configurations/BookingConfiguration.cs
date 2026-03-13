using EventBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBooking.Infrastructure.Persistence.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            //global query filter for booking. Cancelled booking data will never be included in system. It is only for to see %percentage of cancelling bookings.
            builder.HasQueryFilter(b => !b.IsCancelled);
        }
    }
}
