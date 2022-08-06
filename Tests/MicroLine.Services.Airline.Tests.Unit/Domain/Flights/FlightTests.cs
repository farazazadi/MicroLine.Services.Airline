using MicroLine.Services.Airline.Domain.Aircrafts;
using MicroLine.Services.Airline.Domain.CabinCrews;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Domain.FlightCrews;
using MicroLine.Services.Airline.Domain.FlightPricingPolicies;
using MicroLine.Services.Airline.Domain.Flights;
using MicroLine.Services.Airline.Domain.Flights.Events;
using MicroLine.Services.Airline.Domain.Flights.Exceptions;
using MicroLine.Services.Airline.Tests.Common.Fakes;
using Moq;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Flights;

public class FlightTests
{

    [Fact]
    public void Flight_ShouldHaveFlightScheduledEventAndScheduledStatus_WhenScheduled()
    {
        // Given
        var flightRepository = new Mock<IFlightRepository>().Object;


        var flightPricingPolicies = Enumerable.Empty<IFlightPricingPolicy>();


        var flightNumber = FlightNumber.Create("UAL870");

        var originAirport = FakeAirport.NewFake();
        var destinationAirport = FakeAirport.NewFake();

        var aircraft = FakeAircraft.NewFake(AircraftManufacturer.Boeing);



        var scheduledUtcDateTimeOfDeparture = DateTime.UtcNow.AddMonths(1);

        var economyClassPrice = Money.Of(50, Money.CurrencyType.UnitedStatesDollar);
        var businessClassPrice = Money.Of(50, Money.CurrencyType.UnitedStatesDollar);
        var firstClassPrice = Money.Of(50, Money.CurrencyType.UnitedStatesDollar);

        var basePrices = FlightPrice.Create(economyClassPrice, businessClassPrice, firstClassPrice);

        var flightCrewMembers = FakeFlightCrew.NewFakeList(
            FlightCrewType.Pilot,
            FlightCrewType.CoPilot,
            FlightCrewType.FlightEngineer);


        var cabinCrewMembers = FakeCabinCrew.NewFakeList(
            CabinCrewType.Purser,
            CabinCrewType.FlightAttendant);


        // When
        var flight = Flight.Scheduler.ScheduleNewFlight(flightRepository, flightPricingPolicies,
                flightNumber,
                originAirport,
                destinationAirport,
                aircraft,
                scheduledUtcDateTimeOfDeparture,
                basePrices,
                flightCrewMembers,
                cabinCrewMembers);



        // Then
        flight.DomainEvents.OfType<FlightScheduledEvent>().Any().Should().BeTrue();
        flight.Status.Should().Be(FlightStatus.Scheduled);

    }

    [Fact]
    public void Flight_ShouldThrowInvalidScheduledDateTimeOfDeparture_WhenScheduledUtcDateTimeOfDepartureIsInPastTime()
    {
        // Given
        var flightRepository = new Mock<IFlightRepository>().Object;

        var flightPricingPolicies = Enumerable.Empty<IFlightPricingPolicy>();


        var flightNumber = FlightNumber.Create("UAL870");

        var originAirport = FakeAirport.NewFake();
        var destinationAirport = FakeAirport.NewFake();

        var aircraft = FakeAircraft.NewFake(AircraftManufacturer.Boeing);

        var scheduledUtcDateTimeOfDeparture = DateTime.UtcNow.AddHours(-1);

        var economyClassPrice = Money.Of(50, Money.CurrencyType.UnitedStatesDollar);
        var businessClassPrice = Money.Of(50, Money.CurrencyType.UnitedStatesDollar);
        var firstClassPrice = Money.Of(50, Money.CurrencyType.UnitedStatesDollar);

        var basePrices = FlightPrice.Create(economyClassPrice, businessClassPrice, firstClassPrice);

        var flightCrewMembers = FakeFlightCrew.NewFakeList(
            FlightCrewType.Pilot,
            FlightCrewType.CoPilot,
            FlightCrewType.FlightEngineer);


        var cabinCrewMembers = FakeCabinCrew.NewFakeList(
            CabinCrewType.Purser,
            CabinCrewType.FlightAttendant);


        // When
        var func = () => Flight.Scheduler.ScheduleNewFlight(flightRepository, flightPricingPolicies,
            flightNumber,
            originAirport,
            destinationAirport,
            aircraft,
            scheduledUtcDateTimeOfDeparture,
            basePrices,
            flightCrewMembers,
            cabinCrewMembers);



        // Then
        func.Should().ThrowExactly<InvalidScheduledDateTimeOfDeparture>()
            .And.Code.Should().Be(nameof(InvalidScheduledDateTimeOfDeparture));
    }



    public static TheoryData<List<FlightCrew>> IncompleteFlightCrewMembers = new()
    {
        FakeFlightCrew.NewFakeList(FlightCrewType.Pilot, FlightCrewType.FlightEngineer),

        FakeFlightCrew.NewFakeList(FlightCrewType.CoPilot, FlightCrewType.FlightEngineer),

        FakeFlightCrew.NewFakeList(FlightCrewType.Navigator, FlightCrewType.FlightEngineer),

        FakeFlightCrew.NewFakeList(FlightCrewType.Navigator)
    };

    [Theory, MemberData(nameof(IncompleteFlightCrewMembers))]
    public void Flight_ShouldIncompleteFlightCrewMembersException_WhenFlightCrewDoesNotContainAtLeast1PilotAnd1CoPilotOr2Pilot(
        List<FlightCrew> flightCrewMembers)
    {
        // Given
        var flightRepository = new Mock<IFlightRepository>().Object;

        var flightPricingPolicies = Enumerable.Empty<IFlightPricingPolicy>();


        var flightNumber = FlightNumber.Create("UAL870");

        var originAirport = FakeAirport.NewFake();
        var destinationAirport = FakeAirport.NewFake();

        var aircraft = FakeAircraft.NewFake(AircraftManufacturer.Boeing);

        var scheduledUtcDateTimeOfDeparture = DateTime.UtcNow.AddHours(5);

        var economyClassPrice = Money.Of(50, Money.CurrencyType.UnitedStatesDollar);
        var businessClassPrice = Money.Of(50, Money.CurrencyType.UnitedStatesDollar);
        var firstClassPrice = Money.Of(50, Money.CurrencyType.UnitedStatesDollar);

        var basePrices = FlightPrice.Create(economyClassPrice, businessClassPrice, firstClassPrice);

        var cabinCrewMembers = FakeCabinCrew.NewFakeList(
            CabinCrewType.Purser,
            CabinCrewType.FlightAttendant);


        // When
        var func = () => Flight.Scheduler.ScheduleNewFlight(flightRepository, flightPricingPolicies,
            flightNumber,
            originAirport,
            destinationAirport,
            aircraft,
            scheduledUtcDateTimeOfDeparture,
            basePrices,
            flightCrewMembers,
            cabinCrewMembers);



        // Then
        func.Should().ThrowExactly<IncompleteFlightCrewMembersException>()
            .And.Code.Should().Be(nameof(IncompleteFlightCrewMembersException));
    }

}