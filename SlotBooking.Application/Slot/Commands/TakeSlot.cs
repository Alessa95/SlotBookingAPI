using MediatR;
using SlotBooking.Application.Slot.Services;
using SlotBooking.Infrastructure.HttpClients;

namespace SlotBooking.Application.Slot.Commands
{
    public readonly record struct TakeSlotDto(Guid FacilityId, DateTime Slot, string? Comments, PatientInformationDto Patient) : IRequest;
    
    public readonly record struct PatientInformationDto(string Name, string SecondName, string Email, string Phone);

    public readonly record struct TakeSlotRequest(Guid FacilityId, string Start, string End, string? Comments, PatientInformationDto Patient);
    
    public class TakeSlotHandler(IApiClient apiClient, IAvailabilityService availabilityService) : IRequestHandler<TakeSlotDto>
    {

        public async Task Handle(TakeSlotDto takeSlotDto, CancellationToken cancellationToken)
        {
            var monday = availabilityService.GetMondayOfWeek(takeSlotDto.Slot);
            string endpoint = $"GetWeeklyAvailability/{monday.ToString("yyyyMMdd")}";
            var availability = await apiClient.GetAsync<GetWeeklyAvailabilityResponse>(endpoint);
            var dayAvailability = availabilityService.GetDayAvailabilityForDate(takeSlotDto.Slot, availability);
            var availableSlots = availabilityService.GetAvailableSlotsPerDay(dayAvailability, availability.SlotDurationMinutes, takeSlotDto.Slot);
            if (!availableSlots.Any(s => s == takeSlotDto.Slot.ToString("HH:mm")))
                throw new Exception("Slot not available");
            var takeSlotRequest = new TakeSlotRequest
            {
                Comments = takeSlotDto.Comments,
                Patient = takeSlotDto.Patient,
                FacilityId = availability.Facility.FacilityId,
                Start = takeSlotDto.Slot.ToString("yyyy-MM-dd HH:mm:ss"),
                End = takeSlotDto.Slot.AddMinutes(availability.SlotDurationMinutes).ToString("yyyy-MM-dd HH:mm:ss")
            };
            await apiClient.PostAsync("TakeSlot", takeSlotRequest);
        }
    }
}
