using MicroLine.Services.Airline.Domain.Common.Exceptions;

namespace MicroLine.Services.Airline.Domain.Common.ValueObjects;

public class BaseUtcOffset : ValueObject
{
    public int Hours { get; }
    public int Minutes { get; }

    private BaseUtcOffset(int hours, int minutes)
    {
        Hours = hours;
        Minutes = minutes;
    }

    public static BaseUtcOffset Create(int hours, int minutes)
    {
        Validate(hours, minutes);

        return new BaseUtcOffset(hours, minutes);
    }

    private static void Validate(int hours, int minutes)
    {
        var timeZones = TimeZoneInfo.GetSystemTimeZones();

        if (!timeZones.Any(t => t.BaseUtcOffset.Hours == hours &&
                                                   t.BaseUtcOffset.Minutes == minutes))
            throw new InvalidBaseUtcOffsetException();
    }


    public static implicit operator TimeSpan (BaseUtcOffset offset) =>
            new TimeSpan (offset.Hours, offset.Minutes, 0);

    public override string ToString() =>
        $"UTC{(Hours < 0 ? "-" : "+")}{Math.Abs(Hours):D2}:{Math.Abs(Minutes):D2}";

}
