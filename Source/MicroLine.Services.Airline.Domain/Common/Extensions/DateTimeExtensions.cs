namespace MicroLine.Services.Airline.Domain.Common.Extensions;
public static class DateTimeExtensions
{
    public static DateTime RemoveSeconds(this DateTime dateTime)
    {
        return dateTime.Second == 0 ? dateTime : dateTime.AddSeconds(dateTime.Second * -1);
    }
}
