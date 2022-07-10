using FluentAssertions;
using MicroLine.Services.Airline.Domain.Aircraft;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using Xunit;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Aircraft;

public class AircraftTests
{
    [Fact]
    public void Aircraft_ShouldNotHaveAnyEvents_WhenCreated()
    {
        // Given
        // When

        var aircraft = Airline.Domain.Aircraft.Aircraft.Create(
            manufacturer: AircraftManufacturer.Airbus,
            model: AircraftModel.Create(aircraftModel: "A320"),
            manufactureDate: Date.Create(year: 2006, month: 05, day: 1),
            maximumSeatingCapacity: AircraftMaximumSeatingCapacity.Create(180),
            cruisingSpeed: Speed.Create(828, unitOfSpeed: Speed.UnitOfSpeed.KilometresPerHour),
            maximumOperatingSpeed: Speed.Create(871, unitOfSpeed: Speed.UnitOfSpeed.KilometresPerHour),
            registrationCode: AircraftRegistrationCode.Create(registerCode: "EP-FSA")
        );

        // Then
        aircraft.DomainEvents.Count.Should().Be(0);
    }
}
