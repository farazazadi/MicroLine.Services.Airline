using MicroLine.Services.Airline.Application.Airports.Commands.CreateAirport;
using MicroLine.Services.Airline.Application.Airports.DataTransferObjects;
using MicroLine.Services.Airline.Application.Common.Exceptions;
using MicroLine.Services.Airline.Domain.Airports.Exceptions;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Infrastructure.Integration.Airports;
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
        var airport = FakeAirport.NewFake();

        var createAirportCommand = Mapper.Map<CreateAirportCommand>(airport);

        var expected = Mapper.Map<AirportDto>(airport);


        // When
        var response = await Client.PostAsJsonAsync("api/airports", createAirportCommand);


        // Then
        var airportDto = await response.Content.ReadFromJsonAsync<AirportDto>();

        response.Headers.Location!.ToString().Should().Be($"api/airports/{airportDto.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        airportDto.Should().BeEquivalentTo(expected, options =>
        {
            options
                .Excluding(dto => dto.AuditingDetails)
                .Excluding(dto => dto.Id);

            return options;
        });

        var publishedEvent = GetEventFromRabbitMq<AirportCreatedIntegrationEvent>(@event => @event.Id == airportDto.Id);

        publishedEvent.Should().NotBeNull();
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
        response
            .Should()
            .HaveProblemDetails()
            .WithStatusCode(StatusCodes.Status400BadRequest)
            .WithTitle(Constants.Rfc9110.Titles.BadRequest)
            .WithDetail($"An airport with same IcaoCode ({icaoCode}) already exist!")
            .WithInstance("/api/airports")
            .WithExtensionsThatContain(Constants.ExceptionCode, nameof(DuplicateIcaoCodeException))
            .WithType(Constants.Rfc9110.StatusCodes.BadRequest400);
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
    public async Task Airport_ShouldReturnNotFoundProblem_WhenIdIsNotValid()
    {
        // Given
        var id = Id.Create();


        // When
        var response = await Client.GetAsync($"api/airports/{id}");


        // Then
        response
            .Should()
            .HaveProblemDetails()
            .WithStatusCode(StatusCodes.Status404NotFound)
            .WithTitle(Constants.Rfc9110.Titles.NotFound)
            .WithDetail($"Airport with id ({id}) was not found!")
            .WithInstance($"/api/airports/{id}")
            .WithExtensionsThatContain(Constants.ExceptionCode, nameof(NotFoundException))
            .WithType(Constants.Rfc9110.StatusCodes.NotFound404);
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