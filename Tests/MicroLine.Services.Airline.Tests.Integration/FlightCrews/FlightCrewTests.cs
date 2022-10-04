﻿using System.Net;
using System.Net.Http.Json;
using MicroLine.Services.Airline.Application.FlightCrews.Commands.CreateFlightCrew;
using MicroLine.Services.Airline.Application.FlightCrews.DataTransferObjects;
using MicroLine.Services.Airline.Domain.FlightCrews;
using MicroLine.Services.Airline.Tests.Common.Fakes;
using MicroLine.Services.Airline.Tests.Integration.Common;

namespace MicroLine.Services.Airline.Tests.Integration.FlightCrews;

public class FlightCrewTests : IntegrationTestBase
{
    public FlightCrewTests(AirlineWebApplicationFactory airlineWebApplicationFactory) : base(airlineWebApplicationFactory)
    {
    }


    [Fact]
    public async Task FlightCrew_ShouldBeCreatedAsExpected_WhenRequestIsValid()
    {
        // Given
        var flightCrew = await FakeFlightCrew.NewFakeAsync(FlightCrewType.Pilot);

        var createFlightCrewCommand = Mapper.Map<CreateFlightCrewCommand>(flightCrew);

        var expected = Mapper.Map<FlightCrewDto>(flightCrew);

        // When
        var response = await Client.PostAsJsonAsync("api/flight-crew", createFlightCrewCommand);


        // Then

        var flightCrewDto = await response.Content.ReadFromJsonAsync<FlightCrewDto>();

        response.Headers.Location.ToString().Should().Be($"api/flight-crew/{flightCrewDto.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        flightCrewDto.Should().BeEquivalentTo(expected, options =>
        {
            options.Excluding(dto => dto.AuditingDetails);
            options.Excluding(dto => dto.Id);

            return options;
        });
    }

    [Fact]
    public async Task FlightCrew_ShouldBeReturnedAsExpected_WhenIdIsValid()
    {
        // Given
        var flightCrew = await FakeFlightCrew.NewFakeAsync(FlightCrewType.FlightEngineer);
        await SaveAsync(flightCrew);

        var expected = Mapper.Map<FlightCrewDto>(flightCrew);

        // When
        var response = await Client.GetAsync($"api/flight-crew/{expected.Id}");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var flightCrewDto = await response.Content.ReadFromJsonAsync<FlightCrewDto>();

        flightCrewDto.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetFlightCrewById_ShouldReturnNotFoundStatusCode_WhenIdIsNotValid()
    {
        // Given
        var id = Guid.NewGuid();

        // When
        var response = await Client.GetAsync($"api/flight-crew/{id}");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


}
