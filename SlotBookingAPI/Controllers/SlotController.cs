using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlotBooking.Application.Slot.Queries;
using SlotBookingAPI.Model.BookingSlot;

namespace SlotBookingAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SlotController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Retrieves the available slots for a specific week.
        /// </summary>
        /// <param name="week">The week identifier in 'yyyyMMdd' format, representing a Monday.</param>
        /// <returns>A list of available slots for the requested week.</returns>
        /// <response code="200">Returns the available slots for the specified week.</response>
        /// <response code="400">Returns if the week format is invalid or validation fails.</response>
        [HttpGet("{week}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SlotAvailabilityModel))]
        public async Task<IActionResult> GetAvailability([FromRoute][WeekValidation]string week)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            var query = new WeekDto(week);
            var result = await mediator.Send(query);
            return Ok(SlotAvailabilityModel.FromDto(result));
        }

        /// <summary>
        /// Takes a slot for booking.
        /// </summary>
        /// <param name="request">The slot booking request containing slot details and patient information.</param>
        /// <returns>An HTTP 200 status code if the slot is successfully booked.</returns>
        /// <response code="200">Returns if the slot is successfully booked.</response>
        /// <response code="400">Returns if the request model validation fails.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> TakeSlot([FromBody] TakeSlotModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await mediator.Send(request.ToDto());
            return Ok();
        }
    }
}
