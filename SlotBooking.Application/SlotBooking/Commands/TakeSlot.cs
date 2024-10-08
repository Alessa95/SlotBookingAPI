using MediatR;
using SlotBooking.Application.Slot.Services;
using SlotBooking.Application.SlotBooking;
using SlotBooking.Application.SlotBooking.Commands;
using SlotBooking.Application.Utils;
using SlotBooking.Infrastructure.HttpClients;

namespace SlotBooking.Application.Slot.Commands
{
    public readonly record struct TakeSlotDto(Guid FacilityId, DateTime Slot, string? Comments, PatientInformationDto Patient) : IRequest;
    public readonly record struct PatientInformationDto(string Name, string SecondName, string Email, string Phone);

    public class TakeSlotHandler(IApiClient apiClient, IAvailabilityService availabilityService, IDateTimeUtils dateTimeUtils) : IRequestHandler<TakeSlotDto>
    {
        public async Task Handle(TakeSlotDto takeSlotDto, CancellationToken cancellationToken)
        {
            var monday = dateTimeUtils.GetMondayOfWeek(takeSlotDto.Slot);
            string endpoint = $"{ExternalApiEndPoints.GET_WEEKLY_AVAILABILITY}/{monday:yyyyMMdd}";
            var availability = await apiClient.GetAsync<GetWeeklyAvailabilityResponse>(endpoint);

            var dayAvailability = availabilityService.GetDayAvailabilityForDate(takeSlotDto.Slot, availability);
            var availableSlots = availabilityService.GetAvailableSlotsPerDay(dayAvailability, availability.SlotDurationMinutes, takeSlotDto.Slot);

            if (!availableSlots.Any(s => s == takeSlotDto.Slot.ToString("HH:mm")))
                throw new Exception("Slot not available");

            var takeSlotRequest = TakeSlotRequest.FromDto(takeSlotDto, availability.Facility.FacilityId, availability.SlotDurationMinutes);
            await apiClient.PostAsync(ExternalApiEndPoints.TAKE_SLOT, takeSlotRequest);
        }
    }
}
