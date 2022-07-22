
using Bogus;

namespace MicroLine.Services.Airline.Tests.Common.Fakes.ValueObjects;

public static class BirthDate
{
    public static Domain.Common.ValueObjects.Date NewFake()
    {
        var faker = new Faker();

        var startDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-55));
        var endDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-25));

        return faker.Date.BetweenDateOnly(startDate, endDate);
    }
}
