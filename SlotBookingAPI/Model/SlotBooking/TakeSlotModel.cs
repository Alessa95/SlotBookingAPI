using SlotBooking.API.Model;
using SlotBooking.Application.Slot.Commands;
using System.ComponentModel.DataAnnotations;

namespace SlotBookingAPI.Model.BookingSlot
{
    public class TakeSlotModel
    {
        [Required(ErrorMessage = "Slot is required.")]
        [FutureDate]
        public required DateTime Slot { get; set; }

        [StringLength(500, ErrorMessage = "Comments cannot exceed 500 characters.")]
        public string? Comments { get; set; }

        [Required(ErrorMessage = "Patient information is required.")]
        public required PatientInformationModel Patient { get; set; }

        public TakeSlotDto ToDto()
        { 
            return new TakeSlotDto 
            { 
                Slot = Slot,
                Comments = Comments, 
                Patient = Patient.ToDto()
            };
        }
    }
}
