using MediatR;
using SlotBooking.Infrastructure.HttpClients;

namespace SlotBooking.Application.Slot.Queries
{
    public class GetWeeklyAvailabilityQueryHandler(ApiClient apiClient) : IRequestHandler<GetWeeklyAvailabilityQuery, GetWeeklyAvailabilityResponse>
    {
        public async Task<GetWeeklyAvailabilityResponse> Handle(GetWeeklyAvailabilityQuery request, CancellationToken cancellationToken)
        {
            string endpoint = $"GetWeeklyAvailability/{request.Week:yyyyMMdd}";
            var availability = await apiClient.GetAsync<GetWeeklyAvailabilityResponse>(endpoint);
            return availability;
        }
    }
}
