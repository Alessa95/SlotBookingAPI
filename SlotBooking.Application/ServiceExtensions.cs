using Microsoft.Extensions.DependencyInjection;
using SlotBooking.Application.Slot.Services;

namespace SlotBooking.Application
{
    public static class ServiceExtensions
    {
        public static void ConfigureAplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IAvailabilityService, AvailabilityService>();
        }
    }
}
