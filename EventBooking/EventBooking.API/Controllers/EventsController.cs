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
           return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _eventService.GetEventByIdAsync(id);
            if (result == null) return NotFound("Event couldn't found");
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEventDto createEventDto)
        {
            // FluentValidation otomatik olarak çalışacak (Program.cs ayarı ile)
            var eventId = await _eventService.CreateEventAsync(createEventDto);

            return CreatedAtAction(nameof(GetById), new { id = eventId }, createEventDto);
        }
    }
}
