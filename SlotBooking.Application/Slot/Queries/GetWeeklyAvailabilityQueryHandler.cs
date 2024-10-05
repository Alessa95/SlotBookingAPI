using MediatR;
using SlotBooking.Application.Slot.Services;
using SlotBooking.Infrastructure.HttpClients;
using System.Globalization;

namespace SlotBooking.Application.Slot.Queries
{
    public class GetWeeklyAvailabilityQueryHandler(ApiClient apiClient, IAvailabilityService availabilityService) : IRequestHandler<GetWeeklyAvailabilityQuery, GetWeeklyAvailabilityQueryResponse>
    {
        public async Task<GetWeeklyAvailabilityQueryResponse> Handle(GetWeeklyAvailabilityQuery request, CancellationToken cancellationToken)
        {
            string endpoint = $"GetWeeklyAvailability/{request.Week}";
            var availability = await apiClient.GetAsync<GetWeeklyAvailabilityResponse>(endpoint);
            return availabilityService.GetAvailableSlots(DateTime.ParseExact(request.Week, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None), availability);
        }
    }
}
