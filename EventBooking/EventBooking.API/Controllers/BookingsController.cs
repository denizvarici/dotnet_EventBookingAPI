using EventBooking.Application.Common;
using EventBooking.Application.DTOs;
using EventBooking.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
        [HttpGet("/user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            var result = await _bookingService.GetUserBookingsAsync(userId);
            var response = Result<IEnumerable<BookingDto>>.Success(result);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingDto createBookingDto)
        {
            var result = await _bookingService.CreateBookingAsync(createBookingDto);
            var response = Result<Guid>.Success(result);

            return CreatedAtAction(nameof(GetByUserId), new { userId = createBookingDto.UserId }, response);
        }
    }
}
