using SlotBooking.Application.Slot.Queries;

namespace SlotBookingAPI.Model.BookingSlot
{
    public class SlotAvailabilityModel
    {
        public required IEnumerable<string> Monday { get; set; }
        public required IEnumerable<string> Tuesday { get; set; }
        public required IEnumerable<string> Wednesday { get; set; }
        public required IEnumerable<string> Thursday { get; set; }
        public required IEnumerable<string> Friday { get; set; }
        public required IEnumerable<string> Saturday { get; set; }
        public required IEnumerable<string> Sunday { get; set; }

        public static SlotAvailabilityModel FromDto(GetWeeklyAvailabilityDto dto)
        {
            return new SlotAvailabilityModel
            {
                Monday = dto.Monday?.Select(x => x) ?? [],
                Tuesday = dto.Tuesday?.Select(x => x) ?? [],
                Wednesday = dto.Wednesday?.Select(x => x) ?? [],
                Thursday = dto.Thursday?.Select(x => x) ?? [],
                Friday = dto.Friday?.Select(x => x) ?? [],
                Saturday = dto.Saturday?.Select(x => x) ?? [],
                Sunday = dto.Sunday?.Select(x => x) ?? [],
            };
        }
    }
}
