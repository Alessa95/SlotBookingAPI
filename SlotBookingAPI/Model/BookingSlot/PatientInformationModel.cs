using SlotBooking.Application.Slot.Commands;
using System.ComponentModel.DataAnnotations;

namespace SlotBookingAPI.Model.BookingSlot
{
    public class PatientInformationModel
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Second name is required.")]
        [StringLength(50, ErrorMessage = "Second name cannot exceed 50 characters.")]
        public required string SecondName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        public required string Phone { get; set; }

        public PatientInformationDto ToDto()
        {
            return new PatientInformationDto 
            {
                Name = Name,
                SecondName = SecondName,
                Email = Email,
                Phone = Phone
            };
        }
    }
}
