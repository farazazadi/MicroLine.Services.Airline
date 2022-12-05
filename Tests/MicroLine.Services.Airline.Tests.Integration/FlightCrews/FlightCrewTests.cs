using System.Net;
using System.Net.Http.Json;
using MicroLine.Services.Airline.Application.FlightCrews.Commands.CreateFlightCrew;
using MicroLine.Services.Airline.Application.FlightCrews.DataTransferObjects;
using MicroLine.Services.Airline.Domain.FlightCrews;
using MicroLine.Services.Airline.Tests.Common.Extensions;
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
        var flightCrew = FakeFlightCrew.NewFake(FlightCrewType.Pilot);

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
    public async Task FlightCrew_ShouldReturnCreateFlightCrewProblem_WhenPassportNumberAlreadyExist()
    {
        // Given
        var existingFlightCrew = FakeFlightCrew.NewFake(FlightCrewType.Pilot);

        await SaveAsync(existingFlightCrew);

        var existingPassportNumber = existingFlightCrew.PassportNumber;

        var flightCrew = FakeFlightCrew.NewFake(FlightCrewType.Pilot, passportNumber: existingPassportNumber);

        var createFlightCrewCommand = Mapper.Map<CreateFlightCrewCommand>(flightCrew);


        // When
        var response = await Client.PostAsJsonAsync("api/flight-crew", createFlightCrewCommand);


        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.GetProblemResultAsync();

        problemDetails.Extensions.Values.FirstOrDefault()?.ToString().Should().Be(nameof(CreateFlightCrewException));
    }


    [Fact]
    public async Task FlightCrew_ShouldReturnCreateFlightCrewProblem_WhenNationalIdAlreadyExist()
    {
        // Given
        var existingFlightCrew = FakeFlightCrew.NewFake(FlightCrewType.Pilot);

        await SaveAsync(existingFlightCrew);

        var existingNationalId = existingFlightCrew.NationalId;

        var flightCrew = FakeFlightCrew.NewFake(FlightCrewType.Pilot, nationalId: existingNationalId);

        var createFlightCrewCommand = Mapper.Map<CreateFlightCrewCommand>(flightCrew);


        // When
        var response = await Client.PostAsJsonAsync("api/flight-crew", createFlightCrewCommand);


        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.GetProblemResultAsync();

        problemDetails.Extensions.Values.FirstOrDefault()?.ToString().Should().Be(nameof(CreateFlightCrewException));
    }


    [Fact]
    public async Task FlightCrew_ShouldBeReturnedAsExpected_WhenIdIsValid()
    {
        // Given
        var flightCrew = FakeFlightCrew.NewFake(FlightCrewType.FlightEngineer);
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


    [Fact]
    public async Task AllFlightCrew_ShouldBeReturnedAsExpected()
    {
        await AirlineWebApplicationFactory.ResetDatabaseAsync();

        // Given
        var flightCrewList = FakeFlightCrew.NewFakeList(
            FlightCrewType.Pilot,
            FlightCrewType.Pilot,
            FlightCrewType.CoPilot,
            FlightCrewType.FlightEngineer,
            FlightCrewType.Navigator
        );

        await SaveAsync(flightCrewList);

        var expected = Mapper.Map<IList<FlightCrewDto>>(flightCrewList);


        // When
        var request = await Client.GetAsync("api/flight-crew");


        // Then
        request.StatusCode.Should().Be(HttpStatusCode.OK);

        var flightCrewDtoList = await request.Content.ReadFromJsonAsync<IList<FlightCrewDto>>();

        flightCrewDtoList.Should().BeEquivalentTo(expected);
    }


}
