﻿using System.Net;
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

    [Fact]
    public async Task Flight_ShouldBeReturnedAsExpected_WhenIdIsValid()
    {
        // Given
        var flight = await FakeFlight.ScheduleNewFakeFlightAsync();
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
    public async Task AllFlights_ShouldBeReturnedAsExpected()
    {
        await AirlineWebApplicationFactory.ResetDatabaseAsync();

        // Given
        var flights = await FakeFlight.ScheduleNewFakeFlightsAsync(5);

        await SaveAsync(flights);

        var expected = Mapper.Map<List<FlightDto>>(flights);


        // When
        var response = await Client.GetAsync("api/flights");


        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var flightDtoList = await response.Content.ReadFromJsonAsync<List<FlightDto>>();

        flightDtoList.Should().BeEquivalentTo(expected);
    }
}