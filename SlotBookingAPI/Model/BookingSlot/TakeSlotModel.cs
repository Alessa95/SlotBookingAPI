using SlotBooking.Application.Slot.Commands;
using System.ComponentModel.DataAnnotations;

namespace SlotBookingAPI.Model.BookingSlot
{
    public class TakeSlotModel : IValidatableObject
    {
        [Required(ErrorMessage = "FacilityId is required.")]
        public Guid FacilityId { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        public DateTime Start { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        public DateTime End { get; set; }

        [StringLength(500, ErrorMessage = "Comments cannot exceed 500 characters.")]
        public string? Comments { get; set; }

        [Required(ErrorMessage = "Patient information is required.")]
        public required PatientInformationModel Patient { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (End <= Start)
            {
                yield return new ValidationResult("End time must be later than Start time.", new[] { "End" });
            }
        }

        public TakeSlotDto ToDto()
        { 
            return new TakeSlotDto 
            { 
                FacilityId = FacilityId,
                Start = Start.ToString("yyyy-MM-dd HH:mm:ss"), 
                End = End.ToString("yyyy-MM-dd HH:mm:ss"), 
                Comments = Comments, 
                Patient = Patient.ToDto()
            };
        }
    }
}
