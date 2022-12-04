using Bogus;
using MicroLine.Services.Airline.Domain.Aircrafts;
using MicroLine.Services.Airline.Domain.Airports;
using MicroLine.Services.Airline.Domain.CabinCrews;
using MicroLine.Services.Airline.Domain.FlightCrews;
using MicroLine.Services.Airline.Domain.FlightPricingPolicies;
using MicroLine.Services.Airline.Domain.Flights;
using Moq;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Common.Fakes;
public static class FakeFlight
{
    public static async Task<Flight> ScheduleNewFakeFlightAsync(
        IFlightReadonlyRepository flightReadonlyRepository = null,
        IEnumerable<IFlightPricingPolicy> flightPricingPolicies = null,
        FlightNumber flightNumber = null,
        Airport originAirport = null,
        Airport destinationAirport = null,
        Aircraft aircraft = null,
        DateTime? scheduledUtcDateTimeOfDeparture = null,
        FlightPrice basePrices = null,
        List<FlightCrew> flightCrewMembers = null,
        List<CabinCrew> cabinCrewMembers = null)
    {
        var faker = new Faker();

        flightReadonlyRepository ??= Mock.Of<IFlightReadonlyRepository>();
        flightPricingPolicies ??= Enumerable.Empty<IFlightPricingPolicy>();

        flightNumber ??= NewFakeFlightNumber(faker);

        originAirport ??= FakeAirport.NewFake();
        destinationAirport ??= FakeAirport.NewFake();

        var aircraftManufacturer = faker.PickRandom<AircraftManufacturer>();
        aircraft ??= await FakeAircraft.NewFakeAsync(aircraftManufacturer);

        scheduledUtcDateTimeOfDeparture ??= DateTime.UtcNow.AddDays(4);

        basePrices ??= NewFakeFlightPrice(faker);

        flightCrewMembers ??= await FakeFlightCrew.NewFakeListAsync(
            FlightCrewType.Pilot,
            FlightCrewType.CoPilot,
            FlightCrewType.FlightEngineer);

        cabinCrewMembers ??= await FakeCabinCrew.NewFakeListAsync(
            CabinCrewType.Purser,
            CabinCrewType.FlightAttendant,
            CabinCrewType.FlightAttendant,
            CabinCrewType.FlightAttendant,
            CabinCrewType.FlightAttendant,
            CabinCrewType.Chef
            );

        var flight = await Flight.ScheduleNewFlightAsync(
            flightReadonlyRepository,
            flightPricingPolicies,
            flightNumber,
            originAirport,
            destinationAirport,
            aircraft,
            scheduledUtcDateTimeOfDeparture.Value,
            basePrices,
            flightCrewMembers,
            cabinCrewMembers
        );

        return flight;
    }


    public static async Task<List<Flight>> ScheduleNewFakeFlightsAsync(int count)
    {
        var flights = new List<Flight>();

        for (var i = 0; i < count; i++)
        {
            var flight = await ScheduleNewFakeFlightAsync();
            flights.Add(flight);
        }

        return flights;
    }


    private static FlightNumber NewFakeFlightNumber(Faker faker)
    {
        var flightNumber = faker.Random.String2(3, RandomSelectionAllowedCharacters.UpperCaseLetters)
            + faker.Random.UInt(101,999);

        return flightNumber;
    }

    private static FlightPrice NewFakeFlightPrice(Faker faker)
    {
        var flightPrice = FlightPrice.Create(
            Money.Of(faker.Random.UInt(100,199), Money.CurrencyType.UnitedStatesDollar),
            Money.Of(faker.Random.UInt(200, 299), Money.CurrencyType.UnitedStatesDollar),
            Money.Of(faker.Random.UInt(300, 399), Money.CurrencyType.UnitedStatesDollar)
        );

        return flightPrice;
    }
}