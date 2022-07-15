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

    public static bool HasValidLength(this string input, int minLength, int maxLength, bool trimFirst = true)
    {
        var text = trimFirst ? input.Trim() : input;

        return text.Length >= minLength && text.Length <= maxLength;
    }

    public static bool AreAllCharactersLetter(this string input) => input.All(char.IsLetter);

    public static bool AreAllCharactersLetterOrDigit(this string input) => input.All(char.IsLetterOrDigit);

}