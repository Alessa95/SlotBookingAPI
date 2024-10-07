using MediatR;
using SlotBooking.Application.Slot.Services;
using SlotBooking.Infrastructure.HttpClients;
using System.Globalization;

namespace SlotBooking.Application.Slot.Queries
{
    public readonly record struct AvailabilityFromDateDto(DateTime StartDay) : IRequest<GetWeeklyAvailabilityDto>;

    public readonly record struct GetWeeklyAvailabilityDto(
        IEnumerable<string> Monday,
        IEnumerable<string> Tuesday,
        IEnumerable<string> Wednesday,
        IEnumerable<string> Thursday,
        IEnumerable<string> Friday,
        IEnumerable<string> Saturday,
        IEnumerable<string> Sunday
    );
        
    public class GetWeeklyAvailabilityHandler(IApiClient apiClient, IAvailabilityService availabilityService) : IRequestHandler<AvailabilityFromDateDto, GetWeeklyAvailabilityDto>
    {
        public async Task<GetWeeklyAvailabilityDto> Handle(AvailabilityFromDateDto request, CancellationToken cancellationToken)
        {
            var monday = availabilityService.GetMondayOfWeek(request.StartDay);
            string endpoint = $"GetWeeklyAvailability/{monday.ToString("yyyyMMdd")}";
            var availability = await apiClient.GetAsync<GetWeeklyAvailabilityResponse>(endpoint);
            return availabilityService.GetAvailableSlots(monday, request.StartDay, availability);
        }
    }
}
