using SlotBooking.Application.Slot.Queries;

namespace SlotBooking.Application.Slot.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        public GetWeeklyAvailabilityQueryResponse GetAvailableSlots(DateTime monday, GetWeeklyAvailabilityResponse weeklyAvailability)
        {
            var availability = new GetWeeklyAvailabilityQueryResponse
            {
                Monday = GetAvailableSlotsForDay(weeklyAvailability.Monday, weeklyAvailability.SlotDurationMinutes, monday),
                Tuesday = GetAvailableSlotsForDay(weeklyAvailability.Tuesday, weeklyAvailability.SlotDurationMinutes, monday.AddDays(1)),
                Wednesday = GetAvailableSlotsForDay(weeklyAvailability.Wednesday, weeklyAvailability.SlotDurationMinutes, monday.AddDays(2)),
                Thursday = GetAvailableSlotsForDay(weeklyAvailability.Thursday, weeklyAvailability.SlotDurationMinutes, monday.AddDays(3)),
                Friday = GetAvailableSlotsForDay(weeklyAvailability.Friday, weeklyAvailability.SlotDurationMinutes, monday.AddDays(4)),
                Saturday = GetAvailableSlotsForDay(weeklyAvailability.Saturday, weeklyAvailability.SlotDurationMinutes, monday.AddDays(5)),
                Sunday = GetAvailableSlotsForDay(weeklyAvailability.Sunday, weeklyAvailability.SlotDurationMinutes, monday.AddDays(6))
            };

            return availability;
        }

        private List<AvailableSlot> GetAvailableSlotsForDay(DayAvailability dayAvailability, int slotDurationMinutes, DateTime day)
        {
            if (dayAvailability == null || dayAvailability.WorkPeriod == null)
                return new List<AvailableSlot>();

            var availableSlots = new List<AvailableSlot>();
            var busySlots = dayAvailability.BusySlots ?? new List<BusySlot>();

            busySlots = busySlots.OrderBy(b => b.Start).ToList();

            DateTime startOfDay = day.AddHours(dayAvailability.WorkPeriod.StartHour);
            DateTime endOfDay = day.AddHours(dayAvailability.WorkPeriod.EndHour);

            DateTime lunchStart = day.AddHours(dayAvailability.WorkPeriod.LunchStartHour);
            DateTime lunchEnd = day.AddHours(dayAvailability.WorkPeriod.LunchEndHour);

            while (startOfDay.AddMinutes(slotDurationMinutes) <= endOfDay)
            {
                var slotEndTime = startOfDay.AddMinutes(slotDurationMinutes);

                if (startOfDay >= lunchStart && slotEndTime <= lunchEnd)
                {
                    startOfDay = lunchEnd;
                    continue;
                }

                bool isBusy = busySlots.Any(b => startOfDay < b.End && slotEndTime > b.Start);

                if (!isBusy)
                {
                    availableSlots.Add(new AvailableSlot
                    {
                        Start = startOfDay,
                        End = slotEndTime
                    });
                }

                startOfDay = slotEndTime;
            }

            return availableSlots;
        }
    }
}
