using SlotBooking.Application.Slot.Queries;

namespace SlotBooking.Application.Slot.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        public GetWeeklyAvailabilityDto GetAvailableSlots(DateTime monday, DateTime currentDate, GetWeeklyAvailabilityResponse weeklyAvailability)
        {
            return new GetWeeklyAvailabilityDto
            {
                Monday = GetDayAvailability(weeklyAvailability.Monday, weeklyAvailability.SlotDurationMinutes, DayOfWeek.Monday, monday, currentDate),
                Tuesday = GetDayAvailability(weeklyAvailability.Tuesday, weeklyAvailability.SlotDurationMinutes, DayOfWeek.Tuesday, monday, currentDate),
                Wednesday = GetDayAvailability(weeklyAvailability.Wednesday, weeklyAvailability.SlotDurationMinutes, DayOfWeek.Wednesday, monday, currentDate),
                Thursday = GetDayAvailability(weeklyAvailability.Thursday, weeklyAvailability.SlotDurationMinutes, DayOfWeek.Thursday, monday, currentDate),
                Friday = GetDayAvailability(weeklyAvailability.Friday, weeklyAvailability.SlotDurationMinutes, DayOfWeek.Friday, monday, currentDate),
                Saturday = GetDayAvailability(weeklyAvailability.Saturday, weeklyAvailability.SlotDurationMinutes, DayOfWeek.Saturday, monday, currentDate),
                Sunday = GetDayAvailability(weeklyAvailability.Sunday, weeklyAvailability.SlotDurationMinutes, DayOfWeek.Sunday, monday, currentDate)
            };
        }

        public List<string> GetDayAvailability(DayAvailability dayAvailability, int slotDurationMinutes, DayOfWeek dayOfWeek, DateTime monday, DateTime currentDate)
        {
            var date = monday.AddDays((int)dayOfWeek - (int)DayOfWeek.Monday);
            if (date.Date < currentDate.Date)
                return new List<string>();

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
            startOfDay = startOfDay < day ? day : startOfDay;

            // Ensure endOfDay is not earlier than startOfDay
            if (endOfDay <= startOfDay)
                return availableSlots;

            TimeSpan slotDuration = TimeSpan.FromMinutes(slotDurationMinutes);

            for (DateTime currentSlotStart = startOfDay; currentSlotStart.Add(slotDuration) <= endOfDay; currentSlotStart = currentSlotStart.Add(slotDuration))
            {
                DateTime currentSlotEnd = currentSlotStart.Add(slotDuration);

                var isLunchTime = currentSlotStart < lunchEnd && currentSlotEnd > lunchStart;
                if (isLunchTime)
                {
                    currentSlotStart = lunchEnd;
                    continue;
                }

                bool isBusy = busySlots.Any(b => currentSlotStart < b.End && currentSlotEnd > b.Start);
                if (!isBusy)
                {
                    availableSlots.Add(currentSlotStart.ToString("HH:mm"));
                }
            }

            return availableSlots;
        }

        public DateTime GetMondayOfWeek(DateTime date)
        {
            int difference = date.DayOfWeek - DayOfWeek.Monday;

            if (difference < 0)
            {
                difference += 7;
            }

            return date.AddDays(-difference).Date;
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
