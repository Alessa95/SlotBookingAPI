namespace SlotBooking.Application.Slot.Queries
{
    public class GetWeeklyAvailabilityResponse
    {
        public Facility Facility { get; set; }
        public int SlotDurationMinutes { get; set; }
        public DayAvailability Monday { get; set; }
        public DayAvailability Tuesday { get; set; }
        public DayAvailability Wednesday { get; set; }
        public DayAvailability Thursday { get; set; }
        public DayAvailability Friday { get; set; }
        public DayAvailability Saturday { get; set; }
        public DayAvailability Sunday { get; set; }
    }

    public class Facility
    {
        public Guid FacilityId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class DayAvailability
    {
        public WorkPeriod WorkPeriod { get; set; }
        public List<BusySlot> BusySlots { get; set; }
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
