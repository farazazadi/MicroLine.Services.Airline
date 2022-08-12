namespace MicroLine.Services.Airline.Tests.Common.Extensions;

public static class DateTimeExtensions
{
    public static DateTime NextWeekDayDateTime(this DateTime baseDateTime, DayOfWeek nextDayOfWeek)
    {
        var baseDateTimeWeekDayNumber = (int)baseDateTime.DayOfWeek;
        var nextWeekDayNumber = (int)nextDayOfWeek;

        if (nextWeekDayNumber <= baseDateTimeWeekDayNumber)
            nextWeekDayNumber += 7;

        return baseDateTime.AddDays(nextWeekDayNumber - baseDateTimeWeekDayNumber);
    }
}
