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
        /// <param name="startDay">Retrieves the available slots of the week starting from a specified date.</param>
        /// <returns>A list of available slots for the requested week.</returns>
        /// <response code="200">Returns the available slots for the specified week, starting from a specified date.</response>
        /// <response code="400">Returns if startDay validation fails.</response>
        [HttpGet("{startDay}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SlotAvailabilityModel))]
        public async Task<IActionResult> GetAvailability([FromRoute][FutureDate]DateTime startDay)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            var query = new AvailabilityFromDateDto(startDay);
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
