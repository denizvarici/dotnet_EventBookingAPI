using EventBooking.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBooking.Application.Interfaces
{
    public interface IBookingService
    {
        Task<Guid> CreateBookingAsync(CreateBookingDto createBookingDto,Guid userId);
        Task<IEnumerable<BookingDto>> GetUserBookingsAsync(Guid userId);
        Task CancelBookingAsync(Guid bookingId, Guid userId);
    }
}
