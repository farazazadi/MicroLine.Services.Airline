using Bogus;
using MicroLine.Services.Airline.Domain.Airports;

namespace MicroLine.Services.Airline.Tests.Common.Fakes;

public static class FakeAirport
{
    public static Airport NewFake()
    {
        var faker = new Faker();

        var icaoCode = NewFakeIcaoCode(faker);
        var iataCode = NewFakeIataCode(faker);

        var airportLocation = NewFakeAirportLocation(faker);
        var airportName = NewFakeAirportName(faker, airportLocation.City);

        var baseUtcOffset = ValueObjects.FakeBaseUtcOffset.NewFake();

        return Airport.Create(icaoCode, iataCode, airportName, baseUtcOffset, airportLocation);
    }


    private static IcaoCode NewFakeIcaoCode(Faker faker)
    {
        var icaoCode = faker.Random.String2(4, 4, RandomSelectionAllowedCharacters.UpperCaseLetters);
        return IcaoCode.Create(icaoCode);
    }

    private static IataCode NewFakeIataCode(Faker faker)
    {
        var iataCode = faker.Random.String2(3, 3, RandomSelectionAllowedCharacters.UpperCaseLetters);

        return IataCode.Create(iataCode);

    }

    private static AirportName NewFakeAirportName(Faker faker, string city = null)
    {
        city ??= faker.Address.City();
        return AirportName.Create($"{city} International Airport");
    }

    private static AirportLocation NewFakeAirportLocation(Faker faker)
    {
        var continent = faker.PickRandom<Continent>();
        var country = faker.Address.Country();
        var region = faker.Address.State();
        var city = faker.Address.City();
        var latitude = faker.Random.Decimal(-90, 90);
        var longitude = faker.Random.Decimal(-180, 180);


        return AirportLocation.Create(continent ,country ,region ,city ,latitude ,longitude);
    }

}