using System.Net;
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

}

