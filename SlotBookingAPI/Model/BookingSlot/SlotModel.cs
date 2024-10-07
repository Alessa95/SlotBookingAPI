using SlotBooking.Application.Slot.Queries;

namespace SlotBookingAPI.Model.BookingSlot
{
    public class SlotModel
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        //public static SlotModel FromDto(AvailableSlotDto dto)
        //{
        //    return new SlotModel
        //    {
        //        Start = dto.Start,
        //        End = dto.End,
        //    };
        //}
    }
}
