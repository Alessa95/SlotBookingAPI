using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace SlotBookingAPI.Model.BookingSlot
{
    public class WeekValidation: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var week = value as string;
            if (week == null)
            {
                return new ValidationResult("Week is required.");
            }

            if (!DateTime.TryParseExact(week, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                return new ValidationResult("Week must be in 'yyyyMMdd' format.");
            }

            if (date.DayOfWeek != DayOfWeek.Monday)
            {
                return new ValidationResult("The week must represent a Monday.");
            }

            return ValidationResult.Success;
        }
    }
}
