using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SlotBooking.Infrastructure.HttpClients;

namespace SlotBooking.Application.Slot.Commands
{
    public class TakeSlotCommandHandler(ApiClient apiClient) : IRequestHandler<TakeSlotCommand>
    {
        public async Task Handle(TakeSlotCommand request, CancellationToken cancellationToken)
        {
            var slot = new Slot
            {
                Start = request.Start,
                End = request.End,
                Comments = request.Comments,
                Patient = request.Patient,
                FacilityId = request.FacilityId,
            };
            await apiClient.PostAsync("TakeSlot", slot);
        }
    }

    public class Slot
    {
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime Start { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime End { get; set; }

        public string Comments { get; set; }
        public PatientInfo Patient { get; set; }

        public Guid FacilityId { get; set; }
    }

    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }
    }
}
