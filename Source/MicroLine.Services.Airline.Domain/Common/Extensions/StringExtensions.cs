using System.Text.RegularExpressions;

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

    public static bool HasValidLength(this string input, int validLength, bool trimFirst = true)
    {
        var text = trimFirst ? input.Trim() : input;

        return text.Length == validLength;
    }

    public static bool AreAllCharactersLetter(this string input) => input.All(char.IsLetter);

    public static bool AreAllCharactersEnglishLetter(this string input)
    {
        return Regex.IsMatch(input, "^[a-zA-Z]+$", RegexOptions.Compiled);
    }

    public static bool AreAllCharactersLetterOrDigit(this string input) => input.All(char.IsLetterOrDigit);

}