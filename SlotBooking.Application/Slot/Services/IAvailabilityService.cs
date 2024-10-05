using SlotBooking.Application.Slot.Queries;

namespace SlotBooking.Application.Slot.Services
{
    public interface IAvailabilityService
    {
        GetWeeklyAvailabilityQueryResponse GetAvailableSlots(DateTime monday, GetWeeklyAvailabilityResponse weeklyAvailability);
    }
}