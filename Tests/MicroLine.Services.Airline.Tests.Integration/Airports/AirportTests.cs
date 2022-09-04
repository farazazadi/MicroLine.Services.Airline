﻿using System.Net;
using System.Net.Http.Json;
using MicroLine.Services.Airline.Application.Airports.Commands.CreateAirport;
using MicroLine.Services.Airline.Application.Airports.DataTransferObjects;
using MicroLine.Services.Airline.Tests.Common.Fakes;
using MicroLine.Services.Airline.Tests.Integration.Common;

namespace MicroLine.Services.Airline.Tests.Integration.Airports;

public class AirportTests : IntegrationTestBase
{
    public AirportTests(AirlineWebApplicationFactory airlineWebApplicationFactory) : base(airlineWebApplicationFactory)
    {
    }


    [Fact]
    public async Task Airport_ShouldBeCreatedAsExpected_WhenRequestIsValid()
    {
        // Given
        var airport = await FakeAirport.NewFakeAsync();

        var createAirportCommand = Mapper.Map<CreateAirportCommand>(airport);

        var expected = Mapper.Map<AirportDto>(airport);


        // When
        var response = await Client.PostAsJsonAsync("api/airports", createAirportCommand);


        // Then
        var airportDto = await response.Content.ReadFromJsonAsync<AirportDto>();

        response.Headers.Location.ToString().Should().Be($"api/airports/{airportDto.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        airportDto.Should().BeEquivalentTo(expected, options =>
        {
            options
                .Excluding(dto => dto.AuditingDetails)
                .Excluding(dto => dto.Id);

            return options;
        });
    }


    [Fact]
    public async Task Airport_ShouldBeReturnedAsExpected_WhenIdIsValid()
    {
        // Given
        var airport = await FakeAirport.NewFakeAsync();

        var createAirportCommand = Mapper.Map<CreateAirportCommand>(airport);

        var createAirportResponse = await Client.PostAsJsonAsync("api/airports", createAirportCommand);

        var expected = await createAirportResponse.Content.ReadFromJsonAsync<AirportDto>();


        // When
        var response = await Client.GetAsync($"{createAirportResponse.Headers.Location}");


        // Then
        var airportDto = await response.Content.ReadFromJsonAsync<AirportDto>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        airportDto.Should().BeEquivalentTo(expected);
    }


    [Fact]
    public async Task AllAirports_ShouldBeReturnedAsExpected()
    {
        // Given
        await AirlineWebApplicationFactory.ResetDatabaseAsync();

        var expected = new List<AirportDto>();

        for (var i = 0; i < 10; i++)
        {
            var airport = await FakeAirport.NewFakeAsync();

            var createAirportCommand = Mapper.Map<CreateAirportCommand>(airport);

            var createAirportResponse = await Client.PostAsJsonAsync("api/airports", createAirportCommand);

            var dto = await createAirportResponse.Content.ReadFromJsonAsync<AirportDto>();

            expected.Add(dto);
        }


        // When
        var response = await Client.GetAsync("api/airports");


        // Then
        var airportDto = await response.Content.ReadFromJsonAsync<List<AirportDto>>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        airportDto.Should().BeEquivalentTo(expected);
    }
}