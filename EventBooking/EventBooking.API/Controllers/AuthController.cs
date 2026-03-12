using EventBooking.Application.Common;
using EventBooking.Application.DTOs;
using EventBooking.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);
            if (result.IsSuccess)
            {
                return Ok(Result<AuthResponseDto>.Success(result));
            }
            else
            {
                return BadRequest(Result<AuthResponseDto>.Failure(result,result.Message));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            if (result.IsSuccess)
            {
                return Ok(Result<AuthResponseDto>.Success(result));
            }
            else
            {
                return BadRequest(Result<AuthResponseDto>.Failure(result, result.Message));
            }
        }
    }
}
