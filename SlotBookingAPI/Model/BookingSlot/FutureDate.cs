using System.ComponentModel.DataAnnotations;

namespace SlotBookingAPI.Model.BookingSlot
{
    public class FutureDate: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var date = (DateTime)value;
            if (date < DateTime.UtcNow)
            {
                return new ValidationResult("Date must be in the future.");
            }

            return ValidationResult.Success;
        }
    }
}
