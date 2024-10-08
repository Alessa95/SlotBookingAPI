using SlotBooking.Application.Slot.Queries;

namespace SlotBooking.Application.Slot.Services
{
    public class AvailabilityService() : IAvailabilityService
    {
        public GetWeeklyAvailabilityDto GetAvailableSlots(DateTime monday, DateTime currentDate, GetWeeklyAvailabilityResponse weeklyAvailability)
        {
            return new GetWeeklyAvailabilityDto
            {
                Monday = GetAvailableSlotsForDayAfterCurrentDate(weeklyAvailability.Monday, weeklyAvailability.SlotDurationMinutes, DayOfWeek.Monday, monday, currentDate),
                Tuesday = GetAvailableSlotsForDayAfterCurrentDate(weeklyAvailability.Tuesday, weeklyAvailability.SlotDurationMinutes, DayOfWeek.Tuesday, monday, currentDate),
                Wednesday = GetAvailableSlotsForDayAfterCurrentDate(weeklyAvailability.Wednesday, weeklyAvailability.SlotDurationMinutes, DayOfWeek.Wednesday, monday, currentDate),
                Thursday = GetAvailableSlotsForDayAfterCurrentDate(weeklyAvailability.Thursday, weeklyAvailability.SlotDurationMinutes,DayOfWeek.Thursday, monday, currentDate),
                Friday = GetAvailableSlotsForDayAfterCurrentDate(weeklyAvailability.Friday, weeklyAvailability.SlotDurationMinutes, DayOfWeek.Friday, monday, currentDate),
                Saturday = GetAvailableSlotsForDayAfterCurrentDate(weeklyAvailability.Saturday, weeklyAvailability.SlotDurationMinutes, DayOfWeek.Saturday, monday, currentDate),
                Sunday = GetAvailableSlotsForDayAfterCurrentDate(weeklyAvailability.Sunday, weeklyAvailability.SlotDurationMinutes, DayOfWeek.Sunday, monday, currentDate)
            };
        }

        public IEnumerable<string> GetAvailableSlotsForDayAfterCurrentDate(DayAvailability dayAvailability, int slotDurationMinutes, DayOfWeek dayOfWeek, DateTime monday, DateTime currentDate)
        {
            var date = monday.AddDays((int)dayOfWeek - (int)DayOfWeek.Monday);
            if (date.Date < currentDate.Date)
                return [];

            return GetAvailableSlotsPerDay(
                dayAvailability,
                slotDurationMinutes,
                date.Date == currentDate.Date ? currentDate : date.Date
            );
        }

        public List<string> GetAvailableSlotsPerDay(DayAvailability dayAvailability, int slotDurationMinutes, DateTime day)
        {
            var availableSlots = new List<string>();

            if (dayAvailability?.WorkPeriod == null || slotDurationMinutes <= 0)
                return availableSlots;

            var workPeriod = dayAvailability.WorkPeriod;
            var busySlots = dayAvailability.BusySlots?.OrderBy(b => b.Start).ToList() ?? new List<BusySlot>();

            DateTime startOfDay = day.Date.AddHours(workPeriod.StartHour);
            DateTime endOfDay = day.Date.AddHours(workPeriod.EndHour);
            DateTime lunchStart = day.Date.AddHours(workPeriod.LunchStartHour);
            DateTime lunchEnd = day.Date.AddHours(workPeriod.LunchEndHour);

            // Adjust startOfDay if the given day starts later than the work period start
            if(startOfDay < day)
                startOfDay = day;

            if (endOfDay <= startOfDay)
                return availableSlots;

            TimeSpan slotDuration = TimeSpan.FromMinutes(slotDurationMinutes);
            DateTime slotStartTime = startOfDay;

            while (slotStartTime.Add(slotDuration) <= endOfDay)
            {
                DateTime slotEndTime = slotStartTime.Add(slotDuration);

                var isLunchTime = slotStartTime < lunchEnd && slotEndTime > lunchStart;
                if (isLunchTime)
                {
                    slotStartTime = lunchEnd;
                    continue;
                }

                bool isBusy = busySlots.Any(b => slotStartTime < b.End && slotEndTime > b.Start);
                if (!isBusy)
                {
                    availableSlots.Add(slotStartTime.ToString("HH:mm"));
                }

                slotStartTime = slotStartTime.Add(slotDuration);
            }
            return availableSlots;
        }

        public DayAvailability GetDayAvailabilityForDate(DateTime date, GetWeeklyAvailabilityResponse weeklyAvailability)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return weeklyAvailability.Monday;
                case DayOfWeek.Tuesday:
                    return weeklyAvailability.Tuesday;
                case DayOfWeek.Wednesday:
                    return weeklyAvailability.Wednesday;
                case DayOfWeek.Thursday:
                    return weeklyAvailability.Thursday;
                case DayOfWeek.Friday:
                    return weeklyAvailability.Friday;
                case DayOfWeek.Saturday:
                    return weeklyAvailability.Saturday;
                case DayOfWeek.Sunday:
                    return weeklyAvailability.Sunday;
                default:
                    throw new ArgumentException("Invalid day of the week.");
            }
        }
    }
}
