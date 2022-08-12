using MicroLine.Services.Airline.Domain.Aircrafts;
using MicroLine.Services.Airline.Domain.CabinCrews;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Domain.FlightCrews;
using MicroLine.Services.Airline.Domain.FlightPricingPolicies;
using MicroLine.Services.Airline.Domain.Flights;
using MicroLine.Services.Airline.Domain.Flights.Events;
using MicroLine.Services.Airline.Domain.Flights.Exceptions;
using MicroLine.Services.Airline.Tests.Common.Extensions;
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


    public static TheoryData<DateTime,  decimal, decimal, decimal,  decimal, decimal, decimal> WeekDaysPricingData = new()
    {
        // Weekday                                              BasePrices(EC,BC,FC)        BasePrices after applying weekdays ratios
        {DateTime.UtcNow.NextWeekDayDateTime(DayOfWeek.Monday) , 300.00m, 400.00m, 500.00m,  312.00m, 416.00m, 520.00m},
        {DateTime.UtcNow.NextWeekDayDateTime(DayOfWeek.Tuesday) , 300.00m, 400.00m, 500.00m,  273.00m, 364.00m, 455.00m},
        {DateTime.UtcNow.NextWeekDayDateTime(DayOfWeek.Wednesday) , 300.00m, 400.00m, 500.00m, 279.00m, 372.00m, 465.00m},
        {DateTime.UtcNow.NextWeekDayDateTime(DayOfWeek.Thursday) , 300.00m, 400.00m, 500.00m,  318.00m, 424.00m, 530.00m},
        {DateTime.UtcNow.NextWeekDayDateTime(DayOfWeek.Friday) , 300.00m, 400.00m, 500.00m,  321.00m, 428.00m, 535.00m},
        {DateTime.UtcNow.NextWeekDayDateTime(DayOfWeek.Saturday) , 300.00m, 400.00m, 500.00m,  309.00m, 412.00m, 515.00m},
        {DateTime.UtcNow.NextWeekDayDateTime(DayOfWeek.Sunday) , 300.00m, 400.00m, 500.00m,   360.00m, 480.00m, 600.00m},
    };

    [Theory, MemberData(nameof(WeekDaysPricingData))]
    public void Flight_ShouldHaveExpectedPrices_WhenWeekDayFlightPricingPolicyApplied(
        DateTime scheduledUtcDateTimeOfDeparture,
        decimal baseEconomyClassPrice, decimal baseBusinessClassPrice, decimal baseFirstClassPrice,
        decimal expectedEconomyClassPrice, decimal expectedBusinessClassPrice, decimal expectedFirstClassPrice
        )
    {
        // Given
        var flightRepository = new Mock<IFlightRepository>().Object;

        var flightPricingPolicies = new List<IFlightPricingPolicy> {WeekDayFlightPricingPolicy.Create()};


        var flightNumber = FlightNumber.Create("UAL870");

        var originAirport = FakeAirport.NewFake();
        var destinationAirport = FakeAirport.NewFake();

        var aircraft = FakeAircraft.NewFake(AircraftManufacturer.Boeing);


        var economyClassPrice = Money.Of(baseEconomyClassPrice, Money.CurrencyType.UnitedStatesDollar);
        var businessClassPrice = Money.Of(baseBusinessClassPrice, Money.CurrencyType.UnitedStatesDollar);
        var firstClassPrice = Money.Of(baseFirstClassPrice, Money.CurrencyType.UnitedStatesDollar);

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
        flight.Prices.EconomyClass.Amount.Should().Be(expectedEconomyClassPrice);
        flight.Prices.BusinessClass.Amount.Should().Be(expectedBusinessClassPrice);
        flight.Prices.FirstClass.Amount.Should().Be(expectedFirstClassPrice);

    }

}