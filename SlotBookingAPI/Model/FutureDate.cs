using System.ComponentModel.DataAnnotations;

namespace SlotBooking.API.Model
{
    public class FutureDate : ValidationAttribute
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
