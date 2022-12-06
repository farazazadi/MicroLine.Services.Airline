using Bogus;
using MicroLine.Services.Airline.Domain.Airports;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Common.Fakes;

public static class FakeAirport
{
    public static Airport NewFake(
        IcaoCode icaoCode = null,
        IataCode iataCode = null,
        AirportName airportName = null,
        BaseUtcOffset baseUtcOffset = null,
        AirportLocation airportLocation = null
    )
    {
        var faker = new Faker();

        icaoCode ??= NewFakeIcaoCode(faker);
        iataCode ??= NewFakeIataCode(faker);

        baseUtcOffset ??= ValueObjects.FakeBaseUtcOffset.NewFake();
        airportLocation ??= NewFakeAirportLocation(faker);
        airportName ??= NewFakeAirportName(faker, airportLocation.City);


        return Airport.Create(
            icaoCode, iataCode, airportName, baseUtcOffset, airportLocation);
    }

    public static Airport NewFake(double latitude, double longitude)
    {
        var faker = new Faker();

        var airportLocation = NewFakeAirportLocation(faker, latitude, longitude);
        var airportName = NewFakeAirportName(faker, airportLocation.City);

        return NewFake(airportName: airportName, airportLocation: airportLocation);
    }

    public static List<Airport> NewFakeList(int count)
    {
        var airports = new List<Airport>();

        for (var i = 0; i < count; i++)
        {
            var airport = NewFake();
            airports.Add(airport);
        }

        return airports;
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

    private static AirportLocation NewFakeAirportLocation(Faker faker, double? latitude  = null, double? longitude = null)
    {
        var continent = faker.PickRandom<Continent>();
        var country = faker.Address.Country();
        var region = faker.Address.State();
        var city = faker.Address.City();
        latitude ??= faker.Random.Double(-90, 90);
        longitude ??= faker.Random.Double(-180, 180);


        return AirportLocation.Create(continent ,country ,region ,city ,latitude.Value ,longitude.Value);
    }

}