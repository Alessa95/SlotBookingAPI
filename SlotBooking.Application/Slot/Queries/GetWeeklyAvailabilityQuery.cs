using MediatR;

namespace SlotBooking.Application.Slot.Queries
{
    public class GetWeeklyAvailabilityQuery(string week) : IRequest<GetWeeklyAvailabilityResponse>
    {
        public string Week {  get; private set; } = week;
    }
}
