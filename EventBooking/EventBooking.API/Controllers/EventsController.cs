using EventBooking.Application.Common;
using EventBooking.Application.DTOs;
using EventBooking.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
           var result = await _eventService.GetAllEventsAsync();
           return Ok(Result<IEnumerable<EventDto>>.Success(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _eventService.GetEventByIdAsync(id);
            if (result == null) return NotFound(Result<Guid>.Failure(id,"event not found")); ;
            return Ok(Result<EventDto>.Success(result));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEventDto createEventDto)
        {
            var eventId = await _eventService.CreateEventAsync(createEventDto);

            var response = Result<Guid>.Success(eventId);

            return CreatedAtAction(nameof(GetById), new { id = eventId }, response);
        }
    }
}
