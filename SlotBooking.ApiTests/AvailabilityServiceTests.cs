using SlotBooking.Application.Slot.Queries;
using SlotBooking.Application.Slot.Services;

namespace SlotBooking.ApiTests
{
    [TestFixture]
    public class AvailabilityServiceTests
    {
        private AvailabilityService _availabilityService;

        [SetUp]
        public void Setup()
        {
            _availabilityService = new AvailabilityService();
        }

        [Test]
        public void GetAvailableSlots_ShouldReturnSlots_ExcludingBusySlots()
        {
            // Arrange
            var monday = new DateTime(2024, 10, 7); // A Monday

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
                    new BusySlot { Start = monday.AddHours(10), End = monday.AddHours(10).AddMinutes(30) },
                }
            };

            int slotDurationMinutes = 30;

            // Act
            var availableSlots = _availabilityService.GetAvailableSlotsPerDay(dayAvailability, slotDurationMinutes, monday);

            // Assert
            // Ensure slots are created, and the busy slot is excluded
            Assert.That(availableSlots.Count, Is.GreaterThan(0));
            Assert.IsFalse(availableSlots.Any(slot => slot.Start.Hour == 10 && slot.End.Hour == 10 && slot.End.Minute == 30)); // Busy slot must not appear
        }

        [Test]
        public void GetAvailableSlots_ShouldReturnSlots_ExcludingLunchPeriod()
        {
            // Arrange
            var monday = new DateTime(2024, 10, 7); // A Monday
            var lunchStart = monday.AddHours(12);
            var lunchEnd = monday.AddHours(13);   

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
            var availableSlots = _availabilityService.GetAvailableSlotsPerDay(dayAvailability, slotDurationMinutes, monday);

            // Assert
            // Ensure slots are created and lunch break is skipped
            Assert.That(availableSlots.Count, Is.GreaterThan(0));
            Assert.IsFalse(availableSlots.Any(slot => slot.Start < lunchEnd && slot.End > lunchStart)); // Lunch hour must not appear
        }

        [Test]
        public void GetAvailableSlots_ShouldReturnEmptyList_WhenDayIsFullyBusy()
        {
            // Arrange
            var monday = new DateTime(2024, 10, 7); // A Monday

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
                new BusySlot { Start = monday.AddHours(9), End = monday.AddHours(17) } // Fully busy
            }
            };

            int slotDurationMinutes = 30;

            // Act
            var availableSlots = _availabilityService.GetAvailableSlotsPerDay(dayAvailability, slotDurationMinutes, monday);

            // Assert
            // Ensure no available slots when the entire workday is busy
            Assert.That(availableSlots.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetAvailableSlots_ShouldReturnSlots_AfterLunchAndBusySlots()
        {
            // Arrange
            var monday = new DateTime(2024, 10, 7); // A Monday
            var lunchStart = monday.AddHours(12);
            var lunchEnd = monday.AddHours(13);

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
                    new BusySlot { Start = monday.AddHours(10), End = monday.AddHours(10).AddMinutes(30) },
                }
            };

            int slotDurationMinutes = 30;

            // Act
            var availableSlots = _availabilityService.GetAvailableSlotsPerDay(dayAvailability, slotDurationMinutes, monday);

            // Assert
            // Ensure that the available slots exclude busy slots and lunch break
            Assert.That(availableSlots.Count, Is.GreaterThan(0));
            Assert.IsFalse(availableSlots.Any(slot => slot.Start.Hour == 10 && slot.End.Hour == 10 && slot.End.Minute == 30)); // Busy slot must not appear
            Assert.IsFalse(availableSlots.Any(slot => slot.Start < lunchEnd && slot.End > lunchStart)); // Lunch hour must not appear
        }
    }
}
