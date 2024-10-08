namespace SlotBooking.Application.Slot
{
    public class GetWeeklyAvailabilityResponse
    {
        public required Facility Facility { get; set; }
        public int SlotDurationMinutes { get; set; }
        public required DayAvailability Monday { get; set; }
        public required DayAvailability Tuesday { get; set; }
        public required DayAvailability Wednesday { get; set; }
        public required DayAvailability Thursday { get; set; }
        public required DayAvailability Friday { get; set; }
        public required DayAvailability Saturday { get; set; }
        public required DayAvailability Sunday { get; set; }
    }

    public class Facility
    {
        public Guid FacilityId { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
    }

    public class DayAvailability
    {
        public required WorkPeriod WorkPeriod { get; set; }
        public required List<BusySlot> BusySlots { get; set; }
    }

    public class WorkPeriod
    {
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public int LunchStartHour { get; set; }
        public int LunchEndHour { get; set; }
    }

    public class BusySlot
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
