namespace SlotBooking.Application.Utils
{
    public class DateTimeUtils : IDateTimeUtils
    {
        public DateTime GetMondayOfWeek(DateTime date)
        {
            int daysToSubtract = ((int)date.DayOfWeek + 6) % 7;
            return date.AddDays(-daysToSubtract).Date;
        }
    }
}
