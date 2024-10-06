using MediatR;
using SlotBooking.Application.Slot.Services;
using SlotBooking.Infrastructure.HttpClients;
using System.Globalization;

namespace SlotBooking.Application.Slot.Queries
{
    public readonly record struct WeekDto(string Week) : IRequest<GetWeeklyAvailabilityDto>;

    public readonly record struct GetWeeklyAvailabilityDto(
        IEnumerable<AvailableSlotDto> Monday,
        IEnumerable<AvailableSlotDto> Tuesday,
        IEnumerable<AvailableSlotDto> Wednesday,
        IEnumerable<AvailableSlotDto> Thursday,
        IEnumerable<AvailableSlotDto> Friday,
        IEnumerable<AvailableSlotDto> Saturday,
        IEnumerable<AvailableSlotDto> Sunday
    );
        
    public readonly record struct AvailableSlotDto(DateTime Start, DateTime End);

    public class GetWeeklyAvailabilityHandler(IApiClient apiClient, IAvailabilityService availabilityService) : IRequestHandler<WeekDto, GetWeeklyAvailabilityDto>
    {
        public async Task<GetWeeklyAvailabilityDto> Handle(WeekDto request, CancellationToken cancellationToken)
        {
            string endpoint = $"GetWeeklyAvailability/{request.Week}";
            var availability = await apiClient.GetAsync<GetWeeklyAvailabilityResponse>(endpoint);
            var date = DateTime.ParseExact(request.Week, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
            return availabilityService.GetAvailableSlots(date, availability);
        }
    }
}
