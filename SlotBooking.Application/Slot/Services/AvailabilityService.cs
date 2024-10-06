﻿using SlotBooking.Application.Slot.Queries;

namespace SlotBooking.Application.Slot.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        public GetWeeklyAvailabilityDto GetAvailableSlots(DateTime monday, GetWeeklyAvailabilityResponse weeklyAvailability)
        {
            var availability = new GetWeeklyAvailabilityDto
            {
                Monday = GetAvailableSlotsPerDay(weeklyAvailability.Monday, weeklyAvailability.SlotDurationMinutes, monday),
                Tuesday = GetAvailableSlotsPerDay(weeklyAvailability.Tuesday, weeklyAvailability.SlotDurationMinutes, monday.AddDays(1)),
                Wednesday = GetAvailableSlotsPerDay(weeklyAvailability.Wednesday, weeklyAvailability.SlotDurationMinutes, monday.AddDays(2)),
                Thursday = GetAvailableSlotsPerDay(weeklyAvailability.Thursday, weeklyAvailability.SlotDurationMinutes, monday.AddDays(3)),
                Friday = GetAvailableSlotsPerDay(weeklyAvailability.Friday, weeklyAvailability.SlotDurationMinutes, monday.AddDays(4)),
                Saturday = GetAvailableSlotsPerDay(weeklyAvailability.Saturday, weeklyAvailability.SlotDurationMinutes, monday.AddDays(5)),
                Sunday = GetAvailableSlotsPerDay(weeklyAvailability.Sunday, weeklyAvailability.SlotDurationMinutes, monday.AddDays(6))
            };

            return availability;
        }

        public List<AvailableSlotDto> GetAvailableSlotsPerDay(DayAvailability dayAvailability, int slotDurationMinutes, DateTime day)
        {
            if (dayAvailability == null || dayAvailability.WorkPeriod == null)
                return new List<AvailableSlotDto>();

            var availableSlots = new List<AvailableSlotDto>();
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
                    availableSlots.Add(new AvailableSlotDto
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
