using SlotBooking.Application.Slot.Queries;

namespace SlotBooking.Application.Slot.Services
{
    public interface IAvailabilityService
    {
        GetWeeklyAvailabilityDto GetAvailableSlots(DateTime monday, GetWeeklyAvailabilityResponse weeklyAvailability);
        List<AvailableSlotDto> GetAvailableSlotsPerDay(DayAvailability dayAvailability, int slotDurationMinutes, DateTime day);
    }
}