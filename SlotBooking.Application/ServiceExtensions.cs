using Microsoft.Extensions.DependencyInjection;
using SlotBooking.Application.Slot.Services;
using SlotBooking.Application.Utils;

namespace SlotBooking.Application
{
    public static class ServiceExtensions
    {
        public static void ConfigureAplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IAvailabilityService, AvailabilityService>();
            services.AddTransient<IDateTimeUtils,  DateTimeUtils>();
        }
    }
}
