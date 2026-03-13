using EventBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBooking.Infrastructure.Persistence.Configurations
{
    internal class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.Property(e => e.RowVersion)
                .IsRowVersion();


            //builder.HasData(
            //    new Event()
            //    {
            //        Id = Guid.Parse("445368C7-23A4-4A8E-9549-9A0E3C62985D"),
            //        Title = "Futbol Maci",
            //        Description = "BJK - GS",
            //        Date = new DateTime(2026, 8, 28),
            //        Capacity = 50,
            //        RemainingCapacity = 50,
            //        Location = "Istanbul/Stadyum"
            //    },
            //    new Event()
            //    {
            //        Id = Guid.Parse("445368C7-23A4-4A8E-9549-9A0E3C62985E"),
            //        Title = "Araba Fuari",
            //        Description = "Lüx araclarin bulustugu fuar",
            //        Date = new DateTime(2026,7,20),
            //        Capacity = 5,
            //        RemainingCapacity = 5,
            //        Location = "Istanbul/Tuyap"
            //    },
            //    new Event()
            //    {
            //        Id = Guid.Parse("445368C7-23A4-4A8E-9549-9A0E3C629852"),
            //        Title = "Tekirdag Namik Kemal Muhendisleri Mezuniyet Toreni",
            //        Description = "Mezun olan muhendislik fakultesi ogrencileri icin kutlama",
            //        Date = new DateTime(2027, 3, 10),
            //        Capacity = 3,
            //        RemainingCapacity = 3,
            //        Location = "Tekirdag/Muh Fakultesi"
            //    },
            //    new Event()
            //    {
            //        Id = Guid.Parse("425368C7-23A4-4A8E-9549-9A0E3C629852"),
            //        Title = "Concurrency Test Event",
            //        Description = "Concurrency Test Event",
            //        Date = new DateTime(2027, 3, 10),
            //        Capacity = 1,
            //        RemainingCapacity = 1,
            //        Location = "Test"
            //    }
            //    );
        }
    }
}
