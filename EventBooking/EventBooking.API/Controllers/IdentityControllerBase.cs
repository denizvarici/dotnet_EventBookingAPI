using EventBooking.Application.Common;
using EventBooking.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventBooking.API.Controllers
{
    public class IdentityControllerBase : ControllerBase
    {
        protected Guid CurrentUserId
        {
            get
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    throw new AuthException("User identity not found or token is invalid.");
                }

                return Guid.Parse(userIdClaim);
            }
        }
    }
}
