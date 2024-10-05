using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlotBooking.Application.Slot.Commands;
using SlotBooking.Application.Slot.Queries;

namespace SlotBookingAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SlotController(IMediator mediator) : ControllerBase
    {

        [HttpGet("{week}")]
        public async Task<IActionResult> GetAvailability(string week)
        {
            var query = new GetWeeklyAvailabilityQuery(week);
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> TakeSlot([FromBody] TakeSlotCommand command)
        {
            await mediator.Send(command);
            return Ok();
        }
    }
}
