namespace MicroLine.Services.Airline.Tests.Common.Extensions;
public static class StringExtensions
{
    public static string Truncate(this string value, int maxLength)
    {
        return value?.Length > maxLength ? value.Substring(0, maxLength) : value;
    }
}
