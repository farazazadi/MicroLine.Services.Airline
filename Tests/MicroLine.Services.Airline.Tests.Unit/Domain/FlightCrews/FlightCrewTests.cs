﻿using MicroLine.Services.Airline.Domain.Common.Enums;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Domain.FlightCrews;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.FlightCrews;

public class FlightCrewTests
{
    [Fact]
    public void FlightCrew_ShouldNotHaveAnyEvent_WhenCreated()
    {
        // Given

        // When
        var flightCrew = FlightCrew.Create(
                            FlightCrewType.Pilot,
                            Gender.Male,
                            FullName.Create("Gabriel", "Smith"),
                            Date.Create(1991, 2, 10),
                            NationalId.Create("6522255963"),
                            PassportNumber.Create("A43678998"),
                            Email.Create("test@gmail.com"),
                            ContactNumber.Create("+901112223344"),
                            Address.Create("154 Maple Ave", "Toronto", "Ontario", "Canada", "123")
                            );

        // Then
        flightCrew.DomainEvents.Count.Should().Be(0);
    }

}
