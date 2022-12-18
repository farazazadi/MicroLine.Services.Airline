using System.Text;

namespace MicroLine.Services.Airline.Infrastructure.Extensions;

internal static class StringExtensions
{
    public static byte[] ToByteArray(this string value)
    {
        return Encoding.UTF8.GetBytes(value);
    }
}