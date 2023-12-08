using MicroLine.Services.Airline.Tests.Common.Fakes;
using MicroLine.Services.Airline.Tests.Integration.Common;
using MicroLine.Services.Airline.Application.Aircrafts.Commands.CreateAircraft;
using MicroLine.Services.Airline.Application.Aircrafts.DataTransferObjects;
using MicroLine.Services.Airline.Domain.Aircrafts;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Domain.Aircrafts.Exceptions;
using MicroLine.Services.Airline.Application.Common.Exceptions;

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
        var aircraft = FakeAircraft.NewFake(AircraftManufacturer.Boeing);

        var createAircraftCommand = Mapper.Map<CreateAircraftCommand>(aircraft);

        var expected = Mapper.Map<AircraftDto>(aircraft);


        // When
        var response = await Client.PostAsJsonAsync("api/aircraft", createAircraftCommand);


        // Then
        var aircraftDto = await response.Content.ReadFromJsonAsync<AircraftDto>();

        response.Headers.Location!.ToString().Should().Be($"api/aircraft/{aircraftDto.Id}");

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
    public async Task Aircraft_ShouldReturnDuplicateAircraftRegistrationCodeProblem_WhenRegistrationCodeAlreadyExist()
    {
        // Given
        var existingAircraft = FakeAircraft.NewFake(AircraftManufacturer.Boeing);
        await SaveAsync(existingAircraft);

        var registrationCode = existingAircraft.RegistrationCode;

        var aircraft = FakeAircraft.NewFake(AircraftManufacturer.Boeing, registrationCode: registrationCode);

        var createAircraftCommand = Mapper.Map<CreateAircraftCommand>(aircraft);


        // When
        var response = await Client.PostAsJsonAsync("api/aircraft", createAircraftCommand);


        // Then
        response
            .Should()
            .HaveProblemDetails()
            .WithStatusCode(StatusCodes.Status400BadRequest)
            .WithTitle(Constants.Rfc9110.Titles.BadRequest)
            .WithDetail($"An aircraft with same RegistrationCode ({aircraft.RegistrationCode}) already exist!")
            .WithInstance("/api/aircraft")
            .WithExtensionsThatContain(Constants.ExceptionCode, nameof(DuplicateAircraftRegistrationCodeException))
            .WithType(Constants.Rfc9110.StatusCodes.BadRequest400);

    }



    [Fact]
    public async Task Aircraft_ShouldBeReturnedAsExpected_WhenIdIsValid()
    {
        // Given
        var aircraft = FakeAircraft.NewFake(AircraftManufacturer.Airbus);
        await SaveAsync(aircraft);

        var expected = Mapper.Map<AircraftDto>(aircraft);

        // When
        var response = await Client.GetAsync($"api/aircraft/{expected.Id}");


        // Then
        response.Should().HaveStatusCode(HttpStatusCode.OK);

        var aircraftDto = await response.Content.ReadFromJsonAsync<AircraftDto>();

        aircraftDto.Should().BeEquivalentTo(expected);

    }


    [Fact]
    public async Task Aircraft_ShouldReturnNotFoundProblem_WhenIdIsNotValid()
    {
        // Given
        var id = Id.Create();


        // When
        var response = await Client.GetAsync($"api/aircraft/{id}");


        // Then
        response
            .Should()
            .HaveProblemDetails()
            .WithStatusCode(StatusCodes.Status404NotFound)
            .WithTitle(Constants.Rfc9110.Titles.NotFound)
            .WithDetail($"Aircraft with id ({id}) was not found!")
            .WithInstance($"/api/aircraft/{id}")
            .WithExtensionsThatContain(Constants.ExceptionCode, nameof(NotFoundException))
            .WithType(Constants.Rfc9110.StatusCodes.NotFound404);
    }



    [Fact]
    public async Task AllAircrafts_ShouldBeReturnedAsExpected()
    {
        await AirlineWebApplicationFactory.ResetDatabaseAsync();

        // Given
        var aircrafts = FakeAircraft.NewFakeList(
            AircraftManufacturer.Airbus,
            AircraftManufacturer.Boeing,
            AircraftManufacturer.Bombardier,
            AircraftManufacturer.LockheedMartin,
            AircraftManufacturer.Embraer,
            AircraftManufacturer.Airbus);

        await SaveAsync(aircrafts);

        var expected = Mapper.Map<IList<AircraftDto>>(aircrafts);


        // When
        var response = await Client.GetAsync("api/aircraft");


        // Then
        response.Should().HaveStatusCode(HttpStatusCode.OK);

        var aircraftDtoList = await response.Content.ReadFromJsonAsync<IList<AircraftDto>>();

        aircraftDtoList.Should().BeEquivalentTo(expected);
    }
}
