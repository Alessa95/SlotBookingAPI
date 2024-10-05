using FluentValidation;
using System.Globalization;

namespace SlotBooking.Application.Slot.Queries
{
    public class GetWeeklyAvailabilityQueryValidator : AbstractValidator<GetWeeklyAvailabilityQuery>
    {
        public GetWeeklyAvailabilityQueryValidator()
        {
            RuleFor(command => command.Week)
            .NotEmpty().WithMessage("Week is required.")
            .Must(BeValidWeekFormat).WithMessage("Week must be in 'yyyyMMdd' format.")
            .Must(BeMonday).WithMessage("Week must be a Monday.");
        }

        private bool BeValidWeekFormat(string week)
        {
            return DateTime.TryParseExact(week, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }

        private bool BeMonday(string week)
        {
            if (DateTime.TryParseExact(week, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                return date.DayOfWeek == DayOfWeek.Monday;
            }
            return false;
        }
    }
}
