using System.Linq.Expressions;
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

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Flights;

public class FlightTests
{

    [Fact]
    public async Task Flight_ShouldHaveFlightScheduledEventAndScheduledStatus_WhenScheduled()
    {
        // Given
        var flightRepository = new Mock<IFlightReadonlyRepository>().Object;


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

        var flightCrewMembers = await FakeFlightCrew.NewFakeListAsync(
            FlightCrewType.Pilot,
            FlightCrewType.CoPilot,
            FlightCrewType.FlightEngineer);


        var cabinCrewMembers = await FakeCabinCrew.NewFakeListAsync(
            CabinCrewType.Purser,
            CabinCrewType.FlightAttendant);


        // When
        var flight = await Flight.ScheduleNewFlightAsync(flightRepository, flightPricingPolicies,
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
    public async Task Flight_ShouldThrowInvalidScheduledDateTimeOfDeparture_WhenScheduledUtcDateTimeOfDepartureIsInPastTime()
    {
        // Given
        var flightRepository = new Mock<IFlightReadonlyRepository>().Object;

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

        var flightCrewMembers = await FakeFlightCrew.NewFakeListAsync(
            FlightCrewType.Pilot,
            FlightCrewType.CoPilot,
            FlightCrewType.FlightEngineer);


        var cabinCrewMembers = await FakeCabinCrew.NewFakeListAsync(
            CabinCrewType.Purser,
            CabinCrewType.FlightAttendant);


        // When
        var func = () => Flight.ScheduleNewFlightAsync(flightRepository, flightPricingPolicies,
            flightNumber,
            originAirport,
            destinationAirport,
            aircraft,
            scheduledUtcDateTimeOfDeparture,
            basePrices,
            flightCrewMembers,
            cabinCrewMembers);



        // Then
        (await func.Should().ThrowExactlyAsync<InvalidScheduledDateTimeOfDeparture>())
            .And.Code.Should().Be(nameof(InvalidScheduledDateTimeOfDeparture));
    }



    public static TheoryData<List<FlightCrew>> IncompleteFlightCrewMembers = new()
    {
        Task.Run(()=> FakeFlightCrew.NewFakeListAsync(FlightCrewType.Pilot, FlightCrewType.FlightEngineer)).GetAwaiter().GetResult(),

        Task.Run(()=> FakeFlightCrew.NewFakeListAsync(FlightCrewType.CoPilot, FlightCrewType.FlightEngineer)).GetAwaiter().GetResult(),

        Task.Run(()=> FakeFlightCrew.NewFakeListAsync(FlightCrewType.Navigator, FlightCrewType.FlightEngineer)).GetAwaiter().GetResult(),

        Task.Run(()=> FakeFlightCrew.NewFakeListAsync(FlightCrewType.Navigator)).GetAwaiter().GetResult()
    };

    [Theory, MemberData(nameof(IncompleteFlightCrewMembers))]
    public async Task Flight_ShouldIncompleteFlightCrewMembersException_WhenFlightCrewDoesNotContainAtLeast1PilotAnd1CoPilotOr2Pilot(
        List<FlightCrew> flightCrewMembers)
    {
        // Given
        var flightRepository = new Mock<IFlightReadonlyRepository>().Object;

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

        var cabinCrewMembers = await FakeCabinCrew.NewFakeListAsync(
            CabinCrewType.Purser,
            CabinCrewType.FlightAttendant);


        // When
        var func = () => Flight.ScheduleNewFlightAsync(flightRepository, flightPricingPolicies,
            flightNumber,
            originAirport,
            destinationAirport,
            aircraft,
            scheduledUtcDateTimeOfDeparture,
            basePrices,
            flightCrewMembers,
            cabinCrewMembers);



        // Then
        (await func.Should().ThrowExactlyAsync<IncompleteFlightCrewMembersException>())
            .And.Code.Should().Be(nameof(IncompleteFlightCrewMembersException));
    }


    public static TheoryData<DateTime, decimal, decimal, decimal, decimal, decimal, decimal> WeekDaysPricingData = new()
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
    public async Task Flight_ShouldHaveExpectedPrices_WhenWeekDayFlightPricingPolicyApplied(
        DateTime scheduledUtcDateTimeOfDeparture,
        decimal baseEconomyClassPrice, decimal baseBusinessClassPrice, decimal baseFirstClassPrice,
        decimal expectedEconomyClassPrice, decimal expectedBusinessClassPrice, decimal expectedFirstClassPrice
        )
    {
        // Given
        var flightRepository = new Mock<IFlightReadonlyRepository>().Object;

        var flightPricingPolicies = new List<IFlightPricingPolicy> { WeekDayFlightPricingPolicy.Create() };


        var flightNumber = FlightNumber.Create("UAL870");

        var originAirport = FakeAirport.NewFake();
        var destinationAirport = FakeAirport.NewFake();

        var aircraft = FakeAircraft.NewFake(AircraftManufacturer.Boeing);


        var economyClassPrice = Money.Of(baseEconomyClassPrice, Money.CurrencyType.UnitedStatesDollar);
        var businessClassPrice = Money.Of(baseBusinessClassPrice, Money.CurrencyType.UnitedStatesDollar);
        var firstClassPrice = Money.Of(baseFirstClassPrice, Money.CurrencyType.UnitedStatesDollar);

        var basePrices = FlightPrice.Create(economyClassPrice, businessClassPrice, firstClassPrice);

        var flightCrewMembers = await FakeFlightCrew.NewFakeListAsync(
            FlightCrewType.Pilot,
            FlightCrewType.CoPilot,
            FlightCrewType.FlightEngineer);


        var cabinCrewMembers = await FakeCabinCrew.NewFakeListAsync(
            CabinCrewType.Purser,
            CabinCrewType.FlightAttendant);


        // When
        var flight = await Flight.ScheduleNewFlightAsync(flightRepository, flightPricingPolicies,
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


    [Fact]
    public async Task Flight_ShouldThrowOverlapFlightResourcesException_WhenFlightOverlapsWithAnotherFlightOfAircraft()
    {
        // Given
        var aircraft = FakeAircraft.NewFake(AircraftManufacturer.Airbus);

        var overlappedFlight = await FakeFlight.ScheduleNewFakeFlightAsync(
            aircraft: aircraft,
            scheduledUtcDateTimeOfDeparture: DateTime.UtcNow.AddDays(1)
            );



        var repository = new Mock<IFlightReadonlyRepository>();

        repository.Setup(repo =>
                repo.GetAsync(It.IsAny<Expression<Func<Flight, bool>>>(), CancellationToken.None).Result)
            .Returns(overlappedFlight);


        // When
        var func = () => FakeFlight.ScheduleNewFakeFlightAsync(
            flightReadonlyRepository: repository.Object,
            aircraft: aircraft,
            scheduledUtcDateTimeOfDeparture: overlappedFlight.ScheduledUtcDateTimeOfArrival.AddHours(-1)
        );


        // Then
        (await func.Should().ThrowExactlyAsync<OverlapFlightResourcesException>())
            .And.Code.Should().Be(nameof(OverlapFlightResourcesException));
    }


    [Fact]
    public async Task Flight_ShouldThrowOverlapFlightResourcesException_WhenFlightCrewMemberOverlapsWithAnotherFlight()
    {

        // Given
        var flightCrewMembers = await FakeFlightCrew.NewFakeListAsync(
            FlightCrewType.CoPilot,
            FlightCrewType.FlightEngineer
        );

        var pilot = await FakeFlightCrew.NewFakeAsync(FlightCrewType.Pilot);
        flightCrewMembers.Add(pilot);

        var overlappedFlight = await FakeFlight.ScheduleNewFakeFlightAsync(
            flightCrewMembers: flightCrewMembers,
            scheduledUtcDateTimeOfDeparture: DateTime.UtcNow.AddDays(1)
        );


        var repository = new Mock<IFlightReadonlyRepository>();

        repository.Setup(repo =>
                repo.GetAllAsync(It.IsAny<Expression<Func<Flight, bool>>>(), CancellationToken.None).Result)
            .Returns(new List<Flight> {overlappedFlight});


        var newFlightCrewMembers = await FakeFlightCrew.NewFakeListAsync(
            FlightCrewType.CoPilot,
            FlightCrewType.FlightEngineer
        );

        newFlightCrewMembers.Add(pilot);

        // When
        var func = () => FakeFlight.ScheduleNewFakeFlightAsync(
            flightReadonlyRepository: repository.Object,
            flightCrewMembers: newFlightCrewMembers,
            scheduledUtcDateTimeOfDeparture: overlappedFlight.ScheduledUtcDateTimeOfArrival.AddHours(-1)
        );


        // Then
        (await func.Should().ThrowExactlyAsync<OverlapFlightResourcesException>())
            .And.Code.Should().Be(nameof(OverlapFlightResourcesException));
    }


    [Fact]
    public async Task Flight_ShouldThrowOverlapFlightResourcesException_WhenCabinCrewMemberOverlapsWithAnotherFlight()
    {

        // Given
        var cabinCrewMembers = await FakeCabinCrew.NewFakeListAsync(
            CabinCrewType.FlightAttendant,
            CabinCrewType.FlightAttendant
        );

        var purser = await FakeCabinCrew.NewFakeAsync(CabinCrewType.Purser);
        cabinCrewMembers.Add(purser);

        var overlappedFlight = await FakeFlight.ScheduleNewFakeFlightAsync(
            cabinCrewMembers: cabinCrewMembers,
            scheduledUtcDateTimeOfDeparture: DateTime.UtcNow.AddDays(1)
        );


        var repository = new Mock<IFlightReadonlyRepository>();

        repository.SetupSequence(repo =>
                repo.GetAllAsync(It.IsAny<Expression<Func<Flight, bool>>>(), CancellationToken.None).Result)
            .Returns(new List<Flight>())
            .Returns(new List<Flight> { overlappedFlight });


        var newCabinCrewMembers = await FakeCabinCrew.NewFakeListAsync(
            CabinCrewType.FlightAttendant,
            CabinCrewType.FlightAttendant
        );

        newCabinCrewMembers.Add(purser);

        // When
        var func = () => FakeFlight.ScheduleNewFakeFlightAsync(
            flightReadonlyRepository: repository.Object,
            cabinCrewMembers: newCabinCrewMembers,
            scheduledUtcDateTimeOfDeparture: overlappedFlight.ScheduledUtcDateTimeOfArrival.AddHours(-1)
        );


        // Then
        (await func.Should().ThrowExactlyAsync<OverlapFlightResourcesException>())
            .And.Code.Should().Be(nameof(OverlapFlightResourcesException));
    }
}