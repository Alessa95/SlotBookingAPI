using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlotBooking.Application.Slot.Queries;
using SlotBookingAPI.Model.BookingSlot;
using System.Globalization;

namespace SlotBookingAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SlotController(IMediator mediator) : ControllerBase
    {

        [HttpGet("{week}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SlotAvailabilityModel))]
        public async Task<IActionResult> GetAvailability([FromRoute][WeekValidation]string week)
        {
            // Date validation was passed
            var query = new WeekDto(week);
            var result = await mediator.Send(query);
            return Ok(SlotAvailabilityModel.FromDto(result));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> TakeSlot([FromBody] TakeSlotModel request)
        {
            await mediator.Send(request.ToDto());
            return Ok();
        }
    }
}
