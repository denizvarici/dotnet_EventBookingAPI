using EventBooking.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBooking.Application.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<EventDto>> GetAllEventsAsync();
        Task<EventDto> GetEventByIdAsync(Guid id);
        Task<Guid> CreateEventAsync(CreateEventDto createEventDto);
    }
}
