using EventBooking.Application.Common;
using EventBooking.Application.DTOs;
using EventBooking.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventBooking.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : IdentityControllerBase
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
        [HttpGet("/my")]
        public async Task<IActionResult> GetByUserSelf()
        {
            var result = await _bookingService.GetUserBookingsAsync(CurrentUserId);
            var response = Result<IEnumerable<BookingDto>>.Success(result);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingDto createBookingDto)
        {
            var result = await _bookingService.CreateBookingAsync(createBookingDto, CurrentUserId);
            var response = Result<Guid>.Success(result);

            return CreatedAtAction(nameof(GetByUserId), new { userId = CurrentUserId }, response);
        }
    }
}
