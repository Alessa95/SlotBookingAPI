namespace SlotBooking.Application.Slot.Queries
{
    public class GetWeeklyAvailabilityQueryResponse
    {
        public IEnumerable<AvailableSlot> Monday { get; set; }
        public IEnumerable<AvailableSlot> Tuesday { get; set; }
        public IEnumerable<AvailableSlot> Wednesday { get; set; }
        public IEnumerable<AvailableSlot> Thursday { get; set; }
        public IEnumerable<AvailableSlot> Friday { get; set; }
        public IEnumerable<AvailableSlot> Saturday { get; set; }
        public IEnumerable<AvailableSlot> Sunday { get; set; }
    }

    public class AvailableSlot
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
