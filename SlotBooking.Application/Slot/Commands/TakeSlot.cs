using MediatR;
using SlotBooking.Infrastructure.HttpClients;

namespace SlotBooking.Application.Slot.Commands
{
    public readonly record struct TakeSlotDto(Guid FacilityId, string Start, string End, string? Comments, PatientInformationDto Patient) : IRequest;
    
    public readonly record struct PatientInformationDto(string Name, string SecondName, string Email, string Phone);
    
    public class TakeSlotHandler(IApiClient apiClient) : IRequestHandler<TakeSlotDto>
    {

        public async Task Handle(TakeSlotDto request, CancellationToken cancellationToken)
        {
            await apiClient.PostAsync("TakeSlot", request);
        }
    }
}
