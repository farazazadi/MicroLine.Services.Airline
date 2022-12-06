using System.Net;
using System.Net.Http.Json;
using MicroLine.Services.Airline.Application.Airports.Commands.CreateAirport;
using MicroLine.Services.Airline.Application.Airports.DataTransferObjects;
using MicroLine.Services.Airline.Domain.Airports.Exceptions;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Tests.Common.Fakes;
using MicroLine.Services.Airline.Tests.Integration.Common;
using MicroLine.Services.Airline.Tests.Common.Extensions;

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
        var airport = FakeAirport.NewFake();

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
    public async Task Airport_ShouldReturnDuplicateIcaoCodeProblem_WhenIcaoCodeAlreadyExist()
    {
        // Given
        var icaoCode = "CYYJ";
        var existingAirport = FakeAirport.NewFake(icaoCode: icaoCode);
        await SaveAsync(existingAirport);

        var airport = FakeAirport.NewFake(icaoCode: icaoCode);

        var createAirportCommand = Mapper.Map<CreateAirportCommand>(airport);


        // When
        var response = await Client.PostAsJsonAsync("api/airports", createAirportCommand);


        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problem = await response.GetProblemResultAsync();

        problem.Extensions[ProblemDetailsExtensions.ExceptionCode]?.ToString()
            .Should().Be(nameof(DuplicateIcaoCodeException));
    }


    [Fact]
    public async Task Airport_ShouldBeReturnedAsExpected_WhenIdIsValid()
    {
        // Given
        var airport = FakeAirport.NewFake();

        await SaveAsync(airport);

        var expected = Mapper.Map<AirportDto>(airport);


        // When
        var response = await Client.GetAsync($"api/airports/{expected.Id}");


        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var airportDto = await response.Content.ReadFromJsonAsync<AirportDto>();

        airportDto.Should().BeEquivalentTo(expected);
    }


    [Fact]
    public async Task Airport_ShouldReturnNotFoundStatusCode_WhenIdIsNotValid()
    {
        // Given
        var id = Id.Create();


        // When
        var response = await Client.GetAsync($"api/airports/{id}");


        // Then
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

    }


    [Fact]
    public async Task AllAirports_ShouldBeReturnedAsExpected()
    {
        // Given
        await AirlineWebApplicationFactory.ResetDatabaseAsync();

        var airports = FakeAirport.NewFakeList(5);

        await SaveAsync(airports);

        var expected = Mapper.Map<IList<AirportDto>>(airports);


        // When
        var response = await Client.GetAsync("api/airports");


        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var airportDto = await response.Content.ReadFromJsonAsync<List<AirportDto>>();

        airportDto.Should().BeEquivalentTo(expected);
    }
}