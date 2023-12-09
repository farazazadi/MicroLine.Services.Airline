using MicroLine.Services.Airline.Application.Common.Exceptions;
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
        var flightCrew = FakeFlightCrew.NewFake(FlightCrewType.Pilot);

        var createFlightCrewCommand = Mapper.Map<CreateFlightCrewCommand>(flightCrew);

        var expected = Mapper.Map<FlightCrewDto>(flightCrew);

        // When
        var response = await Client.PostAsJsonAsync("api/flight-crew", createFlightCrewCommand);


        // Then

        var flightCrewDto = await response.Content.ReadFromJsonAsync<FlightCrewDto>();

        response.Headers.Location!.ToString().Should().Be($"api/flight-crew/{flightCrewDto.Id}");

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
        response
            .Should()
            .HaveProblemDetails()
            .WithStatusCode(StatusCodes.Status400BadRequest)
            .WithTitle(Constants.Rfc9110.Titles.BadRequest)
            .WithDetail($"A flight crew member with same PassportNumber ({existingPassportNumber}) already exist!")
            .WithInstance("/api/flight-crew")
            .WithExtensionsThatContain(Constants.ExceptionCode, nameof(CreateFlightCrewException))
            .WithType(Constants.Rfc9110.StatusCodes.BadRequest400);
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
        response
            .Should()
            .HaveProblemDetails()
            .WithStatusCode(StatusCodes.Status400BadRequest)
            .WithTitle(Constants.Rfc9110.Titles.BadRequest)
            .WithDetail($"A flight crew member with same NationalId ({existingNationalId}) already exist!")
            .WithInstance("/api/flight-crew")
            .WithExtensionsThatContain(Constants.ExceptionCode, nameof(CreateFlightCrewException))
            .WithType(Constants.Rfc9110.StatusCodes.BadRequest400);
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
        response
            .Should()
            .HaveProblemDetails()
            .WithStatusCode(StatusCodes.Status404NotFound)
            .WithTitle(Constants.Rfc9110.Titles.NotFound)
            .WithDetail($"FlightCrew with id ({id}) was not found!")
            .WithInstance($"/api/flight-crew/{id}")
            .WithExtensionsThatContain(Constants.ExceptionCode, nameof(NotFoundException))
            .WithType(Constants.Rfc9110.StatusCodes.NotFound404);
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
