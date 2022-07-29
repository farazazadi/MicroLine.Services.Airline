using Bogus;

namespace MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

public static class BaseUtcOffset
{
    public static Domain.Common.ValueObjects.BaseUtcOffset NewFake()
    {
        var faker = new Faker();

        var systemTimeZones = TimeZoneInfo.GetSystemTimeZones();
        var timeZoneInfo = faker.PickRandom(systemTimeZones, 1)
            .First();

        var hours = timeZoneInfo.BaseUtcOffset.Hours;
        var minutes = timeZoneInfo.BaseUtcOffset.Minutes;

        return Domain.Common.ValueObjects.BaseUtcOffset.Create(hours, minutes);
    }
}