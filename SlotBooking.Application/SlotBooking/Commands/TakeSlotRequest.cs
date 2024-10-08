using SlotBooking.Application.Slot.Commands;

namespace SlotBooking.Application.SlotBooking.Commands
{
    public class TakeSlotRequest
    {  
        public Guid FacilityId { get; set; } 
        public required string Start {  get; set; }
        public required string End { get; set; }
        public string? Comments { get; set; }
        public required PatientInformationRequest Patient { get; set; }

        public static TakeSlotRequest FromDto(TakeSlotDto takeSlotDto, Guid facilityId, int slotDurationMinutes)
        {
            return new TakeSlotRequest
            {
                Comments = takeSlotDto.Comments,
                Patient = PatientInformationRequest.FromDto(takeSlotDto.Patient),
                FacilityId = facilityId,
                Start = takeSlotDto.Slot.ToString("yyyy-MM-dd HH:mm:ss"),
                End = takeSlotDto.Slot.AddMinutes(slotDurationMinutes).ToString("yyyy-MM-dd HH:mm:ss")
            };
        }
    }
    public class PatientInformationRequest
    {
        public required string Name { get; set; }
        public required string SecondName { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }

        public static PatientInformationRequest FromDto(PatientInformationDto dto)
        {
            return new PatientInformationRequest
            {
                Email = dto.Email,
                Phone = dto.Phone,
                Name = dto.Name,
                SecondName = dto.SecondName,
            };
        }
    }
}
