using FluentValidation;

namespace SlotBooking.Application.Slot.Commands
{
    public class TakeSlotCommandValidator : AbstractValidator<TakeSlotCommand>
    {
        public TakeSlotCommandValidator()
        {
            RuleFor(command => command.FacilityId)
                .NotEmpty().WithMessage("FacilityId is required.");

            RuleFor(command => command.Start)
                .GreaterThan(DateTime.UtcNow).WithMessage("Start time must be in the future.");

            RuleFor(command => command.End)
                .GreaterThan(command => command.Start).WithMessage("End time must be after the start time.");

            RuleFor(command => command.Comments)
                .MaximumLength(500).WithMessage("Comments should not exceed 500 characters.")
                .When(command => !string.IsNullOrEmpty(command.Comments));

            // Validate nested PatientInfo object
            RuleFor(command => command.Patient)
                .NotNull().WithMessage("Patient information is required.")
                .SetValidator(new PatientInfoValidator());
        }
    }

    public class PatientInfoValidator : AbstractValidator<PatientInfo>
    {
        public PatientInfoValidator()
        {
            RuleFor(patient => patient.Name)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(100).WithMessage("First name should not exceed 100 characters.");

            RuleFor(patient => patient.SecondName)
                .NotEmpty().WithMessage("Second name is required.")
                .MaximumLength(100).WithMessage("Second name should not exceed 100 characters.");

            RuleFor(patient => patient.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(patient => patient.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?\d+$").WithMessage("Phone number must contain only numbers and optional leading +.")
                .MinimumLength(10).WithMessage("Phone number must be at least 10 digits long.")
                .MaximumLength(15).WithMessage("Phone number must not exceed 15 digits.");
        }
    }
}
