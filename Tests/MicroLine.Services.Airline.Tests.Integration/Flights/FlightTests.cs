using System.Net;
using System.Net.Http.Json;
using MicroLine.Services.Airline.Application.Flights.Commands.ScheduleFlight;
using MicroLine.Services.Airline.Application.Flights.DataTransferObjects;
using MicroLine.Services.Airline.Domain.Common;
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
        var flight = await FakeFlight.ScheduleNewFakeFlightAsync();

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

        response.Headers.Location.ToString().Should().Be($"api/flights/{flightDto.Id}");


        flightDto.Should().BeEquivalentTo(expected, options =>
        {
            options.Excluding(dto=> dto.Id);
            options.Excluding(dto=> dto.AuditingDetails);
            options.Excluding(dto=> dto.Prices);

            return options;
        });

    }
}
