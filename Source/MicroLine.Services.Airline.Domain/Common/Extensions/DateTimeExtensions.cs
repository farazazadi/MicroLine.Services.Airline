namespace MicroLine.Services.Airline.Domain.Common.Extensions;
public static class DateTimeExtensions
{
    public static DateTime RemoveSeconds(this DateTime dateTime)
    {
        return dateTime.Second == 0 ? dateTime : dateTime.AddSeconds(dateTime.Second * -1);
    }

    public static DateTime RemoveSecondsAndSmallerTimeUnites(this DateTime dateTime)
    {
        var normalizeDateTime =
            new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, dateTime.Kind);

        return normalizeDateTime;
    }
}
