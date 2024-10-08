using FluentAssertions;
using SlotBooking.Application.Utils;

namespace SlotBooking.ApiTests
{
    [TestFixture]
    public class DateTimeUtilsTests
    {
        private IDateTimeUtils _dateTimeUtils;

        [SetUp]
        public void SetUp()
        {
            _dateTimeUtils = new DateTimeUtils();
        }

        [TestCase("2024-10-07", "2024-10-07")] // Monday
        [TestCase("2024-10-08", "2024-10-07")] // Tuesday
        [TestCase("2024-10-09", "2024-10-07")] // Wednesday
        [TestCase("2024-10-10", "2024-10-07")] // Thursday
        [TestCase("2024-10-11", "2024-10-07")] // Friday
        [TestCase("2024-10-12", "2024-10-07")] // Saturday
        [TestCase("2024-10-13", "2024-10-07")] // Sunday
        public void GetMondayOfWeek_ShouldReturnCorrectMonday_ForVariousDays(string inputDateStr, string expectedMondayStr)
        {
            // Arrange
            var inputDate = DateTime.Parse(inputDateStr);
            var expectedMonday = DateTime.Parse(expectedMondayStr);

            // Act
            var result = _dateTimeUtils.GetMondayOfWeek(inputDate);

            // Assert
            result.Date.Should().Be(expectedMonday);
        }
    }
}
