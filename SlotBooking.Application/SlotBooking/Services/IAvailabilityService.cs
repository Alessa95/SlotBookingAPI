using SlotBooking.Application.Slot.Queries;

namespace SlotBooking.Application.Slot.Services
{
    public interface IAvailabilityService
    {
        GetWeeklyAvailabilityDto GetAvailableSlots(DateTime monday, DateTime currentDate, GetWeeklyAvailabilityResponse weeklyAvailability);
        IEnumerable<string> GetAvailableSlotsForDayAfterCurrentDate(DayAvailability dayAvailability, int slotDurationMinutes, DayOfWeek dayOfWeek, DateTime monday, DateTime currentDate);
        List<string> GetAvailableSlotsPerDay(DayAvailability dayAvailability, int slotDurationMinutes, DateTime day);
        DayAvailability GetDayAvailabilityForDate(DateTime date, GetWeeklyAvailabilityResponse weeklyAvailability);
    }
}