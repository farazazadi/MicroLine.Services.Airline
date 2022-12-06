﻿using System.Net;
using System.Net.Http.Json;
using MicroLine.Services.Airline.Application.CabinCrews.Commands.CreateCabinCrew;
using MicroLine.Services.Airline.Application.CabinCrews.DataTransferObjects;
using MicroLine.Services.Airline.Domain.CabinCrews;
using MicroLine.Services.Airline.Tests.Common.Extensions;
using MicroLine.Services.Airline.Tests.Common.Fakes;
using MicroLine.Services.Airline.Tests.Integration.Common;

namespace MicroLine.Services.Airline.Tests.Integration.CabinCrews;

public class CabinCrewTests : IntegrationTestBase
{
    public CabinCrewTests(AirlineWebApplicationFactory airlineWebApplicationFactory) : base(airlineWebApplicationFactory)
    {
    }

    [Fact]
    public async Task CabinCrew_ShouldBeCreatedAsExpected_WhenRequestIsValid()
    {
        // Given
        var cabinCrew = FakeCabinCrew.NewFake(CabinCrewType.Purser);

        var createCabinCrewCommand = Mapper.Map<CreateCabinCrewCommand>(cabinCrew);

        var expected = Mapper.Map<CabinCrewDto>(cabinCrew);


        // When
        var response = await Client.PostAsJsonAsync("api/cabin-crew", createCabinCrewCommand);


        // Then
        var cabinCrewDto = await response.Content.ReadFromJsonAsync<CabinCrewDto>();

        response.Headers.Location.ToString().Should().Be($"api/cabin-crew/{cabinCrewDto.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        cabinCrewDto.Should().BeEquivalentTo(expected, options =>
        {
            options.Excluding(dto => dto.AuditingDetails);
            options.Excluding(dto => dto.Id);

            return options;
        });
    }



    [Fact]
    public async Task CabinCrew_ShouldReturnCreateCabinCrewProblem_WhenPassportNumberAlreadyExist()
    {
        // Given
        var existingCabinCrew = FakeCabinCrew.NewFake(CabinCrewType.Purser);

        await SaveAsync(existingCabinCrew);

        var existingPassportNumber = existingCabinCrew.PassportNumber;

        var cabinCrew = FakeCabinCrew.NewFake(CabinCrewType.Purser, passportNumber: existingPassportNumber);

        var createCabinCrewCommand = Mapper.Map<CreateCabinCrewCommand>(cabinCrew);


        // When
        var response = await Client.PostAsJsonAsync("api/cabin-crew", createCabinCrewCommand);


        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.GetProblemResultAsync();

        problemDetails.Extensions.Values.FirstOrDefault()?.ToString().Should().Be(nameof(CreateCabinCrewException));
    }


    [Fact]
    public async Task CabinCrew_ShouldReturnCreateCabinCrewProblem_WhenNationalIdAlreadyExist()
    {
        // Given
        var existingCabinCrew = FakeCabinCrew.NewFake(CabinCrewType.Purser);

        await SaveAsync(existingCabinCrew);

        var existingNationalId = existingCabinCrew.NationalId;

        var cabinCrew = FakeCabinCrew.NewFake(CabinCrewType.Purser, nationalId: existingNationalId);

        var createCabinCrewCommand = Mapper.Map<CreateCabinCrewCommand>(cabinCrew);


        // When
        var response = await Client.PostAsJsonAsync("api/cabin-crew", createCabinCrewCommand);


        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.GetProblemResultAsync();

        problemDetails.Extensions.Values.FirstOrDefault()?.ToString().Should().Be(nameof(CreateCabinCrewException));
    }


    [Fact]
    public async Task CabinCrew_ShouldBeReturnedAsExpected_WhenIdIsValid()
    {
        // Given
        var cabinCrew = FakeCabinCrew.NewFake(CabinCrewType.FlightAttendant);
        await SaveAsync(cabinCrew);

        var expected = Mapper.Map<CabinCrewDto>(cabinCrew);


        // When
        var response = await Client.GetAsync($"api/cabin-crew/{cabinCrew.Id}");


        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var cabinCrewDto = await response.Content.ReadFromJsonAsync<CabinCrewDto>();

        cabinCrewDto.Should().BeEquivalentTo(expected);
    }


    [Fact]
    public async Task AllCabinCrew_ShouldBeReturnedAsExpected()
    {
        await AirlineWebApplicationFactory.ResetDatabaseAsync();

        // Given
        var cabinCrewList = FakeCabinCrew.NewFakeList(
            CabinCrewType.Purser,
            CabinCrewType.FlightAttendant,
            CabinCrewType.FlightAttendant,
            CabinCrewType.FlightAttendant,
            CabinCrewType.FlightAttendant,
            CabinCrewType.FlightAttendant,
            CabinCrewType.Chef
        );

        await SaveAsync(cabinCrewList);

        var expected = Mapper.Map<IList<CabinCrewDto>>(cabinCrewList);


        // When
        var response = await Client.GetAsync("api/cabin-crew");


        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var cabinCrewDtoList = await response.Content.ReadFromJsonAsync<IList<CabinCrewDto>>();

        cabinCrewDtoList.Should().BeEquivalentTo(expected);
    }
}
