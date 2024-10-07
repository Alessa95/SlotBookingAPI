using SlotBooking.Application.Slot.Queries;

namespace SlotBooking.Application.Slot.Services
{
    public interface IAvailabilityService
    {
        GetWeeklyAvailabilityDto GetAvailableSlots(DateTime monday, DateTime currentDate, GetWeeklyAvailabilityResponse weeklyAvailability);
        List<string> GetAvailableSlotsPerDay(DayAvailability dayAvailability, int slotDurationMinutes, DateTime day);
        DateTime GetMondayOfWeek(DateTime date);
        DayAvailability GetDayAvailabilityForDate(DateTime date, GetWeeklyAvailabilityResponse weeklyAvailability);
    }
}