using System.Text.Json;

namespace MicroLine.Services.Airline.Infrastructure.Extensions;
internal static class JsonExtensions
{
    public static string ToJsonString<TValue>(this TValue value)
    {
        return JsonSerializer.Serialize(value);
    }
}