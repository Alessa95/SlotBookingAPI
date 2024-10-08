using FluentAssertions;
using SlotBooking.Application.Slot;
using SlotBooking.Application.Slot.Services;

namespace SlotBooking.ApiTests
{
    [TestFixture]
    public class AvailabilityServiceTests
    {
        private IAvailabilityService _availabilityService;

        [SetUp]
        public void Setup()
        {
            _availabilityService = new AvailabilityService();
        }

        [Test]
        public void GetAvailableSlots_ShouldReturnSlots_ExcludingBusySlots()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddDays(1); 
            startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);

            var dayAvailability = new DayAvailability
            {
                WorkPeriod = new WorkPeriod
                {
                    StartHour = 9,
                    EndHour = 17,
                    LunchStartHour = 12,
                    LunchEndHour = 13
                },
                BusySlots = new List<BusySlot>
                {
                    new BusySlot { Start = startDate.AddHours(10), End = startDate.AddHours(10).AddMinutes(30) },
                }
            };

            int slotDurationMinutes = 30;

            // Act
            var availableSlots = _availabilityService.GetAvailableSlotsPerDay(dayAvailability, slotDurationMinutes, startDate);

            // Assert
            availableSlots.Count().Should().BeGreaterThan(0);
            availableSlots.Should().NotContain("10:00", "10:30");
        }

        [Test]
        public void GetAvailableSlots_ShouldReturnSlots_ExcludingLunchPeriod()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddDays(1);
            startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            var lunchStart = startDate.AddHours(12);
            var lunchEnd = startDate.AddHours(13);   

            var dayAvailability = new DayAvailability
            {
                WorkPeriod = new WorkPeriod
                {
                    StartHour = 9,
                    EndHour = 17,
                    LunchStartHour = 12,
                    LunchEndHour = 13
                },
                BusySlots = new List<BusySlot>()
            };

            int slotDurationMinutes = 30;

            // Act
            var availableSlots = _availabilityService.GetAvailableSlotsPerDay(dayAvailability, slotDurationMinutes, startDate);

            // Assert
            availableSlots.Count().Should().BeGreaterThan(0);
            var invalidLunchTimeSlots = availableSlots.Where(slot =>
            {
                var time = TimeSpan.Parse(slot);
                return time >= TimeSpan.FromHours(12) && time < TimeSpan.FromHours(13);
            });
            invalidLunchTimeSlots.Should().BeEmpty();
        }

        [Test]
        public void GetAvailableSlots_ShouldReturnEmptyList_WhenDayIsFullyBusy()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddDays(1);
            startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);

            var dayAvailability = new DayAvailability
            {
                WorkPeriod = new WorkPeriod
                {
                    StartHour = 9,
                    EndHour = 17,
                    LunchStartHour = 12,
                    LunchEndHour = 13
                },
                BusySlots = new List<BusySlot>
            {
                new BusySlot { Start = startDate.AddHours(9), End = startDate.AddHours(17) } // Fully busy
            }
            };

            int slotDurationMinutes = 30;

            // Act
            var availableSlots = _availabilityService.GetAvailableSlotsPerDay(dayAvailability, slotDurationMinutes, startDate);

            // Assert
            availableSlots.Should().BeEmpty();
        }

        [Test]
        public void GetAvailableSlots_ShouldReturnSlots_AfterLunchAndBusySlots()
        {
            // Arrange
            var startDate = DateTime.UtcNow.AddDays(1);
            startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
            var lunchStart = startDate.AddHours(12);
            var lunchEnd = startDate.AddHours(13);

            var dayAvailability = new DayAvailability
            {
                WorkPeriod = new WorkPeriod
                {
                    StartHour = 9,
                    EndHour = 17,
                    LunchStartHour = 12,
                    LunchEndHour = 13
                },
                BusySlots = new List<BusySlot>
                {
                    new BusySlot { Start = startDate.AddHours(10), End = startDate.AddHours(10).AddMinutes(30) },
                }
            };

            int slotDurationMinutes = 30;

            // Act
            var availableSlots = _availabilityService.GetAvailableSlotsPerDay(dayAvailability, slotDurationMinutes, startDate);
            var invalidLunchTimeSlots = availableSlots.Where(slot =>
            {
                var time = TimeSpan.Parse(slot);
                return time >= TimeSpan.FromHours(12) && time < TimeSpan.FromHours(13);
            });


            // Assert
            availableSlots.Count.Should().BeGreaterThan(0);
            availableSlots.Should().NotContain("10:00", "10:30");
            invalidLunchTimeSlots.Should().BeEmpty();
        }
    }
}
