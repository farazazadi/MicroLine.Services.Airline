using MicroLine.Services.Airline.Application.Common.Exceptions;
using MicroLine.Services.Airline.Application.Flights.Commands.ScheduleFlight;
using MicroLine.Services.Airline.Application.Flights.DataTransferObjects;
using MicroLine.Services.Airline.Domain.CabinCrews;
using MicroLine.Services.Airline.Domain.Common;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Domain.FlightCrews;
using MicroLine.Services.Airline.Infrastructure.Integration.Flights;
using MicroLine.Services.Airline.Tests.Common.Extensions;
using MicroLine.Services.Airline.Tests.Common.Fakes;
using MicroLine.Services.Airline.Tests.Integration.Common;

namespace MicroLine.Services.Airline.Tests.Integration.Flights;

public class FlightTests : IntegrationTestBase
{
    public FlightTests(AirlineWebApplicationFactory airlineWebApplicationFactory) : base(airlineWebApplicationFactory)
    {
    }

    [Fact]
    public async Task Flight_ShouldBeScheduled_WhenRequestIsValid()
    {
        // Given
        var flight = FakeFlight.ScheduleNewFakeFlight();

        var entities = new AggregateRoot[]
        {
            flight.OriginAirport,
            flight.DestinationAirport,
            flight.Aircraft
        }
            .Concat(flight.FlightCrewMembers)
            .Concat(flight.CabinCrewMembers);

        await SaveAsync(entities);


        var scheduleFlightCommand = Mapper.Map<ScheduleFlightCommand>(flight);

        var expected = Mapper.Map<FlightDto>(flight);


        // When
        var response = await Client.PostAsJsonAsync("api/flights", scheduleFlightCommand);


        // Then
        var flightDto = await response.Content.ReadFromJsonAsync<FlightDto>();

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        response.Headers.Location!.ToString().Should().Be($"api/flights/{flightDto?.Id}");


        flightDto.Should().BeEquivalentTo(expected, options =>
        {
            options.Excluding(dto => dto.Id);
            options.Excluding(dto => dto.AuditingDetails);
            options.Excluding(dto => dto.Prices);

            return options;
        });

        
        var publishedEvent =
             GetEventFromRabbitMq<FlightScheduledIntegrationEvent>(@event => @event.FlightId == flightDto?.Id);

        publishedEvent.Should().NotBeNull();
    }

    [Fact]
    public async Task Flight_ShouldBeReturnedAsExpected_WhenIdIsValid()
    {
        // Given
        var flight = FakeFlight.ScheduleNewFakeFlight();
        await SaveAsync(flight);

        var expected = Mapper.Map<FlightDto>(flight);


        // When
        var response = await Client.GetAsync($"api/flights/{flight.Id}");


        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var flightDto = await response.Content.ReadFromJsonAsync<FlightDto>();

        flightDto.Should().BeEquivalentTo(expected);
    }


    [Fact]
    public async Task Flight_ShouldReturnNotFoundStatusCode_WhenIdIsNotValid()
    {
        // Given
        var id = Id.Create();


        // When
        var response = await Client.GetAsync($"api/flights/{id}");


        // Then
        response
            .Should()
            .HaveProblemDetails()
            .WithStatusCode(StatusCodes.Status404NotFound)
            .WithTitle(Constants.Rfc9110.Titles.NotFound)
            .WithDetail($"Flight with id ({id}) was not found!")
            .WithInstance($"/api/flights/{id}")
            .WithExtensionsThatContain(Constants.ExceptionCode, nameof(NotFoundException))
            .WithType(Constants.Rfc9110.StatusCodes.NotFound404);
    }


    [Fact]
    public async Task AllFlights_ShouldBeReturnedAsExpected()
    {
        await AirlineWebApplicationFactory.ResetDatabaseAsync();

        // Given
        var flights = FakeFlight.ScheduleNewFakeFlights(5);

        await SaveAsync(flights);

        var expected = Mapper.Map<List<FlightDto>>(flights);


        // When
        var response = await Client.GetAsync("api/flights");


        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var flightDtoList = await response.Content.ReadFromJsonAsync<List<FlightDto>>();

        flightDtoList.Should().BeEquivalentTo(expected);
    }


    [Fact]
    public async Task FlightSchedulingRequest_ShouldReturnScheduleFlightProblem_WhenOriginAirportDoesNotExist()
    {
        // Given
        var flight = FakeFlight.ScheduleNewFakeFlight();

        var aggregateRoots = new List<AggregateRoot>
            {
                flight.DestinationAirport,
                flight.Aircraft
            }
            .Concat(flight.FlightCrewMembers)
            .Concat(flight.CabinCrewMembers);

        await SaveAsync(aggregateRoots);

        var scheduleFlightCommand = Mapper.Map<ScheduleFlightCommand>(flight);


        // When
        var response = await Client.PostAsJsonAsync("api/flights", scheduleFlightCommand);


        // Then
        response
            .Should()
            .HaveProblemDetails()
            .WithStatusCode(StatusCodes.Status400BadRequest)
            .WithTitle(Constants.Rfc9110.Titles.BadRequest)
            .WithDetail($"Origin airport with Id ({flight.OriginAirport.Id}) can not be found!")
            .WithInstance("/api/flights")
            .WithExtensionsThatContain(Constants.ExceptionCode, nameof(ScheduleFlightException))
            .WithType(Constants.Rfc9110.StatusCodes.BadRequest400);
    }


    [Fact]
    public async Task FlightSchedulingRequest_ShouldReturnScheduleFlightProblem_WhenDestinationAirportDoesNotExist()
    {
        // Given
        var flight = FakeFlight.ScheduleNewFakeFlight();

        var aggregateRoots = new List<AggregateRoot>
            {
                flight.OriginAirport,
                flight.Aircraft
            }
            .Concat(flight.FlightCrewMembers)
            .Concat(flight.CabinCrewMembers);

        await SaveAsync(aggregateRoots);

        var scheduleFlightCommand = Mapper.Map<ScheduleFlightCommand>(flight);


        // When
        var response = await Client.PostAsJsonAsync("api/flights", scheduleFlightCommand);


        // Then
        response
            .Should()
            .HaveProblemDetails()
            .WithStatusCode(StatusCodes.Status400BadRequest)
            .WithTitle(Constants.Rfc9110.Titles.BadRequest)
            .WithDetail($"Destination airport with Id ({flight.DestinationAirport.Id}) can not be found!")
            .WithInstance("/api/flights")
            .WithExtensionsThatContain(Constants.ExceptionCode, nameof(ScheduleFlightException))
            .WithType(Constants.Rfc9110.StatusCodes.BadRequest400);
    }


    [Fact]
    public async Task FlightSchedulingRequest_ShouldReturnScheduleFlightProblem_WhenAircraftDoesNotExist()
    {
        // Given
        var flight = FakeFlight.ScheduleNewFakeFlight();

        var aggregateRoots = new List<AggregateRoot>
            {
                flight.OriginAirport,
                flight.DestinationAirport
            }
            .Concat(flight.FlightCrewMembers)
            .Concat(flight.CabinCrewMembers);

        await SaveAsync(aggregateRoots);

        var scheduleFlightCommand = Mapper.Map<ScheduleFlightCommand>(flight);


        // When
        var response = await Client.PostAsJsonAsync("api/flights", scheduleFlightCommand);


        // Then
        response
            .Should()
            .HaveProblemDetails()
            .WithStatusCode(StatusCodes.Status400BadRequest)
            .WithTitle(Constants.Rfc9110.Titles.BadRequest)
            .WithDetail($"Aircraft with Id ({flight.Aircraft.Id}) can not be found!")
            .WithInstance("/api/flights")
            .WithExtensionsThatContain(Constants.ExceptionCode, nameof(ScheduleFlightException))
            .WithType(Constants.Rfc9110.StatusCodes.BadRequest400);
    }



    [Fact]
    public async Task FlightSchedulingRequest_ShouldReturnScheduleFlightProblem_WhenFlightCrewMembersDoNotExist()
    {
        // Given
        var flight = FakeFlight.ScheduleNewFakeFlight();

        var aggregateRoots = new List<AggregateRoot>
            {
                flight.OriginAirport,
                flight.DestinationAirport,
                flight.Aircraft
            }
            .Concat(flight.CabinCrewMembers);

        await SaveAsync(aggregateRoots);

        var scheduleFlightCommand = Mapper.Map<ScheduleFlightCommand>(flight);


        // When
        var response = await Client.PostAsJsonAsync("api/flights", scheduleFlightCommand);


        // Then
        string expectedDetail =
            flight.FlightCrewMembers
            .GenerateFormattedString("Flight Crew with Id ({0}) can not be found!", flightCrew => flightCrew.Id);

        response
            .Should()
            .HaveProblemDetails()
            .WithStatusCode(StatusCodes.Status400BadRequest)
            .WithTitle(Constants.Rfc9110.Titles.BadRequest)
            .WithDetail(expectedDetail)
            .WithInstance("/api/flights")
            .WithExtensionsThatContain(Constants.ExceptionCode, nameof(ScheduleFlightException))
            .WithType(Constants.Rfc9110.StatusCodes.BadRequest400);
    }



    [Fact]
    public async Task FlightSchedulingRequest_ShouldReturnScheduleFlightProblem_WhenCabinCrewMembersDoNotExist()
    {
        // Given
        var flight = FakeFlight.ScheduleNewFakeFlight();

        var aggregateRoots = new List<AggregateRoot>
            {
                flight.OriginAirport,
                flight.DestinationAirport,
                flight.Aircraft
            }
            .Concat(flight.FlightCrewMembers);

        await SaveAsync(aggregateRoots);

        var scheduleFlightCommand = Mapper.Map<ScheduleFlightCommand>(flight);


        // When
        var response = await Client.PostAsJsonAsync("api/flights", scheduleFlightCommand);


        // Then
        string expectedDetail =
            flight.CabinCrewMembers
                .GenerateFormattedString("Cabin Crew with Id ({0}) can not be found!", cabinCrew => cabinCrew.Id);

        response
            .Should()
            .HaveProblemDetails()
            .WithStatusCode(StatusCodes.Status400BadRequest)
            .WithTitle(Constants.Rfc9110.Titles.BadRequest)
            .WithDetail(expectedDetail)
            .WithInstance("/api/flights")
            .WithExtensionsThatContain(Constants.ExceptionCode, nameof(ScheduleFlightException))
            .WithType(Constants.Rfc9110.StatusCodes.BadRequest400);
    }
    

    [Fact]
    public async Task FlightSchedulingRequest_ShouldReturnScheduleFlightProblem_WhenFlightOverlapsWithAnotherFlightOfAircraft()
    {
        await AirlineWebApplicationFactory.ResetDatabaseAsync();

        // Given
        var overlappedFlight = FakeFlight.ScheduleNewFakeFlight(
            scheduledUtcDateTimeOfDeparture: DateTime.UtcNow.AddDays(1)
        );

        await SaveAsync(overlappedFlight);


        var aircraft = overlappedFlight.Aircraft;

        var flight = FakeFlight.ScheduleNewFakeFlight(
            aircraft: aircraft,
            scheduledUtcDateTimeOfDeparture: overlappedFlight.ScheduledUtcDateTimeOfArrival.AddHours(-1)
        );

        var aggregateRoots = new List<AggregateRoot>
            {
                flight.OriginAirport,
                flight.DestinationAirport,
            }
            .Concat(flight.FlightCrewMembers)
            .Concat(flight.CabinCrewMembers);

        await SaveAsync(aggregateRoots);


        var scheduleFlightCommand = Mapper.Map<ScheduleFlightCommand>(flight);


        // When
        var response = await Client.PostAsJsonAsync("api/flights", scheduleFlightCommand);


        // Then
        response
            .Should()
            .HaveProblemDetails()
            .WithStatusCode(StatusCodes.Status400BadRequest)
            .WithTitle(Constants.Rfc9110.Titles.BadRequest)
            .WithDetail($"The aircraft ({aircraft.Id}) is unavailable due to an overlap with the flight ({overlappedFlight.Id})!")
            .WithInstance("/api/flights")
            .WithExtensionsThatContain(Constants.ExceptionCode, nameof(ScheduleFlightException))
            .WithType(Constants.Rfc9110.StatusCodes.BadRequest400);
    }



    [Fact]
    public async Task FlightSchedulingRequest_ShouldReturnScheduleFlightProblem_WhenFlightOverlapsWithAnotherFlightOfFlightCrewMembers()
    {
        await AirlineWebApplicationFactory.ResetDatabaseAsync();

        // Given
        var overlappedFlight = FakeFlight.ScheduleNewFakeFlight(
            scheduledUtcDateTimeOfDeparture: DateTime.UtcNow.AddDays(1)
        );

        await SaveAsync(overlappedFlight);



        var pilot = overlappedFlight.FlightCrewMembers
            .First(fc => fc.FlightCrewType == FlightCrewType.Pilot);

        var flightCrewMembers = FakeFlightCrew
            .NewFakeList(FlightCrewType.CoPilot, FlightCrewType.FlightEngineer);

        flightCrewMembers.Add(pilot);


        var flight = FakeFlight.ScheduleNewFakeFlight(
            flightCrewMembers: flightCrewMembers,
            scheduledUtcDateTimeOfDeparture: overlappedFlight.ScheduledUtcDateTimeOfArrival.AddHours(-1)
        );

        var aggregateRoots = new List<AggregateRoot>
            {
                flight.OriginAirport,
                flight.DestinationAirport,
                flight.Aircraft,
            }
            .Concat(flight.FlightCrewMembers.Except(new[]{pilot}))
            .Concat(flight.CabinCrewMembers);

        await SaveAsync(aggregateRoots);


        var scheduleFlightCommand = Mapper.Map<ScheduleFlightCommand>(flight);


        // When
        var response = await Client.PostAsJsonAsync("api/flights", scheduleFlightCommand);


        // Then
        response
            .Should()
            .HaveProblemDetails()
            .WithStatusCode(StatusCodes.Status400BadRequest)
            .WithTitle(Constants.Rfc9110.Titles.BadRequest)
            .WithDetail($"The Flight Crew ({pilot.Id}) is unavailable due to an overlap with the flight ({overlappedFlight.Id})!")
            .WithInstance("/api/flights")
            .WithExtensionsThatContain(Constants.ExceptionCode, nameof(ScheduleFlightException))
            .WithType(Constants.Rfc9110.StatusCodes.BadRequest400);
    }


    [Fact]
    public async Task FlightSchedulingRequest_ShouldReturnScheduleFlightProblem_WhenFlightOverlapsWithAnotherFlightOfCabinCrewMembers()
    {
        await AirlineWebApplicationFactory.ResetDatabaseAsync();

        // Given
        var overlappedFlight = FakeFlight.ScheduleNewFakeFlight(
            scheduledUtcDateTimeOfDeparture: DateTime.UtcNow.AddDays(1)
        );

        await SaveAsync(overlappedFlight);



        var purser = overlappedFlight.CabinCrewMembers
            .First(cc => cc.CabinCrewType == CabinCrewType.Purser);

        var cabinCrewMembers = FakeCabinCrew
            .NewFakeList(CabinCrewType.Chef, CabinCrewType.FlightAttendant, CabinCrewType.FlightAttendant);

        cabinCrewMembers.Add(purser);


        var flight = FakeFlight.ScheduleNewFakeFlight(
            cabinCrewMembers: cabinCrewMembers,
            scheduledUtcDateTimeOfDeparture: overlappedFlight.ScheduledUtcDateTimeOfArrival.AddHours(-1)
        );

        var aggregateRoots = new List<AggregateRoot>
            {
                flight.OriginAirport,
                flight.DestinationAirport,
                flight.Aircraft,
            }
            .Concat(flight.FlightCrewMembers)
            .Concat(flight.CabinCrewMembers.Except(new[] { purser }));

        await SaveAsync(aggregateRoots);


        var scheduleFlightCommand = Mapper.Map<ScheduleFlightCommand>(flight);


        // When
        var response = await Client.PostAsJsonAsync("api/flights", scheduleFlightCommand);


        // Then
        //response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        //var problemDetails = await response.GetProblemResultAsync();

        //problemDetails.Extensions[ProblemDetailsExtensions.ExceptionCode]?.ToString()
        //    .Should().Be(nameof(ScheduleFlightException));
        response
            .Should()
            .HaveProblemDetails()
            .WithStatusCode(StatusCodes.Status400BadRequest)
            .WithTitle(Constants.Rfc9110.Titles.BadRequest)
            .WithDetail($"The Cabin Crew ({purser.Id}) is unavailable due to an overlap with the flight ({overlappedFlight.Id})!")
            .WithInstance("/api/flights")
            .WithExtensionsThatContain(Constants.ExceptionCode, nameof(ScheduleFlightException))
            .WithType(Constants.Rfc9110.StatusCodes.BadRequest400);
    }

}
