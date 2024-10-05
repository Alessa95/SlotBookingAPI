using SlotBooking.Application.Slot.Queries;

namespace SlotBookingAPI.Model.BookingSlot
{
    public class SlotAvailabilityModel
    {
        public required IEnumerable<SlotModel> Monday { get; set; }
        public required IEnumerable<SlotModel> Tuesday { get; set; }
        public required IEnumerable<SlotModel> Wednesday { get; set; }
        public required IEnumerable<SlotModel> Thursday { get; set; }
        public required IEnumerable<SlotModel> Friday { get; set; }
        public required IEnumerable<SlotModel> Saturday { get; set; }
        public required IEnumerable<SlotModel> Sunday { get; set; }

        public static SlotAvailabilityModel FromDto(GetWeeklyAvailabilityDto dto)
        {
            return new SlotAvailabilityModel
            {
                Monday = dto.Monday?.Select(SlotModel.FromDto) ?? [],
                Tuesday = dto.Tuesday?.Select(SlotModel.FromDto) ?? [],
                Wednesday = dto.Wednesday?.Select(SlotModel.FromDto) ?? [],
                Thursday = dto.Thursday?.Select(SlotModel.FromDto) ?? [],
                Friday = dto.Friday?.Select(SlotModel.FromDto) ?? [],
                Saturday = dto.Saturday?.Select(SlotModel.FromDto) ?? [],
                Sunday = dto.Sunday?.Select(SlotModel.FromDto) ?? [],
            };
        }
    }
}
