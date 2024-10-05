using MediatR;

namespace SlotBooking.Application.Slot.Commands
{
    public class TakeSlotCommand : IRequest
    {
        public Guid FacilityId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Comments { get; set; }
        public PatientInfo Patient { get; set; }
    }

    public class PatientInfo
    {
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
