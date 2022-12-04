using MicroLine.Services.Airline.Domain.Aircrafts;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Aircrafts;

public class AircraftTests
{
    [Fact]
    public void Aircraft_ShouldNotHaveAnyEvents_WhenCreated()
    {
        // Given
        var model = AircraftModel.Create(aircraftModel: "A320");
        var manufactureDate = Date.Create(year: 2006, month: 05, day: 1);
        var passengerSeatingCapacity = PassengerSeatingCapacity.Create(180, 30, 15);
        var cruisingSpeed = Speed.Create(828, unitOfSpeed: Speed.UnitOfSpeed.KilometresPerHour);
        var maximumOperatingSpeed = Speed.Create(871, unitOfSpeed: Speed.UnitOfSpeed.KilometresPerHour);
        var aircraftRegistrationCode = AircraftRegistrationCode.Create(registerCode: "EP-FSA");


        // When
        var aircraft = Aircraft.Create(
            AircraftManufacturer.Airbus,
            model,
            manufactureDate,
            passengerSeatingCapacity,
            cruisingSpeed,
            maximumOperatingSpeed,
            aircraftRegistrationCode);

        // Then
        aircraft.DomainEvents.Count.Should().Be(0);
    }

}
