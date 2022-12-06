using Bogus;
using MicroLine.Services.Airline.Domain.Aircrafts;
using MicroLine.Services.Airline.Domain.Airports;
using MicroLine.Services.Airline.Domain.CabinCrews;
using MicroLine.Services.Airline.Domain.FlightCrews;
using MicroLine.Services.Airline.Domain.FlightPricingPolicies;
using MicroLine.Services.Airline.Domain.Flights;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Common.Fakes;
public static class FakeFlight
{
    public static Flight ScheduleNewFakeFlight(
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

        flightPricingPolicies ??= Enumerable.Empty<IFlightPricingPolicy>();

        flightNumber ??= NewFakeFlightNumber(faker);

        originAirport ??= FakeAirport.NewFake();
        destinationAirport ??= FakeAirport.NewFake();

        var aircraftManufacturer = faker.PickRandom<AircraftManufacturer>();
        aircraft ??= FakeAircraft.NewFake(aircraftManufacturer);

        scheduledUtcDateTimeOfDeparture ??= DateTime.UtcNow.AddDays(4);

        basePrices ??= NewFakeFlightPrice(faker);

        flightCrewMembers ??= FakeFlightCrew.NewFakeList(
            FlightCrewType.Pilot,
            FlightCrewType.CoPilot,
            FlightCrewType.FlightEngineer);

        cabinCrewMembers ??= FakeCabinCrew.NewFakeList(
            CabinCrewType.Purser,
            CabinCrewType.FlightAttendant,
            CabinCrewType.FlightAttendant,
            CabinCrewType.FlightAttendant,
            CabinCrewType.FlightAttendant,
            CabinCrewType.Chef
            );

        var flight = Flight.ScheduleNewFlight(
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


    public static List<Flight> ScheduleNewFakeFlights(int count)
    {
        var flights = new List<Flight>();

        for (var i = 0; i < count; i++) 
            flights.Add(ScheduleNewFakeFlight());

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