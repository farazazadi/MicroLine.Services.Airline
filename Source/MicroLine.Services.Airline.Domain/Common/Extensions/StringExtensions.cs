namespace MicroLine.Services.Airline.Domain.Common.Extensions;

public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string text, bool trimFirst = true)
    {
        if (text is null)
            return true;

        if(trimFirst)
            text = text.Trim();

        return string.IsNullOrEmpty(text);
    }
}