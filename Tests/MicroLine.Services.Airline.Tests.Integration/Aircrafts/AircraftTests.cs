using MicroLine.Services.Airline.Tests.Common.Fakes;
using MicroLine.Services.Airline.Tests.Integration.Common;
using System.Net.Http.Json;
using System.Net;
using MicroLine.Services.Airline.Application.Aircrafts.Commands.CreateAircraft;
using MicroLine.Services.Airline.Application.Aircrafts.DataTransferObjects;
using MicroLine.Services.Airline.Domain.Aircrafts;

namespace MicroLine.Services.Airline.Tests.Integration.Aircrafts;

public class AircraftTests : IntegrationTestBase
{
    public AircraftTests(AirlineWebApplicationFactory airlineWebApplicationFactory) : base(airlineWebApplicationFactory)
    {
    }


    [Fact]
    public async Task Aircraft_ShouldBeCreatedAsExpected_WhenRequestIsValid()
    {
        // Given
        var aircraft = await FakeAircraft.NewFakeAsync(AircraftManufacturer.Boeing);

        var createAircraftCommand = Mapper.Map<CreateAircraftCommand>(aircraft);

        var expected = Mapper.Map<AircraftDto>(aircraft);


        // When
        var response = await Client.PostAsJsonAsync("api/aircrafts", createAircraftCommand);


        // Then
        var aircraftDto = await response.Content.ReadFromJsonAsync<AircraftDto>();

        response.Headers.Location.ToString().Should().Be($"api/aircrafts/{aircraftDto.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        aircraftDto.Should().BeEquivalentTo(expected, options =>
        {
            options
                .Excluding(dto => dto.AuditingDetails)
                .Excluding(dto => dto.Id);

            return options;
        });
    }


    [Fact]
    public async Task Aircraft_ShouldBeReturnedAsExpected_WhenIdIsValid()
    {
        // Given
        var aircraft = await FakeAircraft.NewFakeAsync(AircraftManufacturer.Airbus);
        await SaveAsync(aircraft);

        var expected = Mapper.Map<AircraftDto>(aircraft);

        // When
        var response = await Client.GetAsync($"api/aircrafts/{expected.Id}");


        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var aircraftDto = await response.Content.ReadFromJsonAsync<AircraftDto>();

        aircraftDto.Should().BeEquivalentTo(expected);

    }

}
