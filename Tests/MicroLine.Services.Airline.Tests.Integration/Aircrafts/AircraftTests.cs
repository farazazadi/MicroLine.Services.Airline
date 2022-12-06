using MicroLine.Services.Airline.Tests.Common.Fakes;
using MicroLine.Services.Airline.Tests.Integration.Common;
using System.Net.Http.Json;
using System.Net;
using MicroLine.Services.Airline.Application.Aircrafts.Commands.CreateAircraft;
using MicroLine.Services.Airline.Application.Aircrafts.DataTransferObjects;
using MicroLine.Services.Airline.Domain.Aircrafts;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Tests.Common.Extensions;
using MicroLine.Services.Airline.Domain.Aircrafts.Exceptions;

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
    public async Task Aircraft_ShouldReturnDuplicateAircraftRegistrationCodeProblem_WhenRegistrationCodeAlreadyExist()
    {
        // Given
        var existingAircraft = FakeAircraft.NewFake(AircraftManufacturer.Boeing);
        await SaveAsync(existingAircraft);

        var registrationCode = existingAircraft.RegistrationCode;

        var aircraft = FakeAircraft.NewFake(AircraftManufacturer.Boeing, registrationCode: registrationCode);

        var createAircraftCommand = Mapper.Map<CreateAircraftCommand>(aircraft);


        // When
        var response = await Client.PostAsJsonAsync("api/aircrafts", createAircraftCommand);


        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problem = await response.GetProblemResultAsync();

        problem.Extensions[ProblemDetailsExtensions.ExceptionCode]?.ToString()
            .Should().Be(nameof(DuplicateAircraftRegistrationCodeException));
    }
    


    [Fact]
    public async Task Aircraft_ShouldBeReturnedAsExpected_WhenIdIsValid()
    {
        // Given
        var aircraft = FakeAircraft.NewFake(AircraftManufacturer.Airbus);
        await SaveAsync(aircraft);

        var expected = Mapper.Map<AircraftDto>(aircraft);

        // When
        var response = await Client.GetAsync($"api/aircrafts/{expected.Id}");


        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var aircraftDto = await response.Content.ReadFromJsonAsync<AircraftDto>();

        aircraftDto.Should().BeEquivalentTo(expected);

    }


    [Fact]
    public async Task Aircraft_ShouldReturnNotFoundStatusCode_WhenIdIsNotValid()
    {
        // Given
        var id = Id.Create();


        // When
        var response = await Client.GetAsync($"api/aircrafts/{id}");


        // Then
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

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
        var response = await Client.GetAsync("api/aircrafts");


        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var aircraftDtoList = await response.Content.ReadFromJsonAsync<IList<AircraftDto>>();

        aircraftDtoList.Should().BeEquivalentTo(expected);
    }
}
