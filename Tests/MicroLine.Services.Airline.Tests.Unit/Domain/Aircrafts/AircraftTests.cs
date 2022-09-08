using MicroLine.Services.Airline.Domain.Aircrafts;
using MicroLine.Services.Airline.Domain.Common.ValueObjects;
using MicroLine.Services.Airline.Domain.Aircrafts.Exceptions;

namespace MicroLine.Services.Airline.Tests.Unit.Domain.Aircrafts;

public class AircraftTests
{
    [Fact]
    public async Task Aircraft_ShouldNotHaveAnyEvents_WhenCreated()
    {
        // Given
        var repository = Mock.Of<IAircraftReadonlyRepository>();

        var model = AircraftModel.Create(aircraftModel: "A320");
        var manufactureDate = Date.Create(year: 2006, month: 05, day: 1);
        var passengerSeatingCapacity = PassengerSeatingCapacity.Create(180, 30, 15);
        var cruisingSpeed = Speed.Create(828, unitOfSpeed: Speed.UnitOfSpeed.KilometresPerHour);
        var maximumOperatingSpeed = Speed.Create(871, unitOfSpeed: Speed.UnitOfSpeed.KilometresPerHour);
        var aircraftRegistrationCode = AircraftRegistrationCode.Create(registerCode: "EP-FSA");


        // When
        var aircraft = await Aircraft.CreateAsync(
            AircraftManufacturer.Airbus,
            model,
            manufactureDate,
            passengerSeatingCapacity,
            cruisingSpeed,
            maximumOperatingSpeed,
            aircraftRegistrationCode,
            repository);

        // Then
        aircraft.DomainEvents.Count.Should().Be(0);
    }


    [Fact]
    public async Task Aircraft_ShouldThrowDuplicateAircraftRegistrationCodeException_WhenRegisterCodeCodeAlreadyExist()
    {
        // Given
        var registerCode = "EP-FSA";

        var repository = new Mock<IAircraftReadonlyRepository>();

        repository
            .Setup(r => r.ExistAsync(registerCode, CancellationToken.None))
            .ReturnsAsync(true);

        var model = AircraftModel.Create("A320");
        var manufactureDate = Date.Create(2006, 05,1);
        var passengerSeatingCapacity = PassengerSeatingCapacity.Create(180, 30, 15);
        var cruisingSpeed = Speed.Create(828, Speed.UnitOfSpeed.KilometresPerHour);
        var maximumOperatingSpeed = Speed.Create(871, Speed.UnitOfSpeed.KilometresPerHour);
        var aircraftRegistrationCode = AircraftRegistrationCode.Create(registerCode);


        // When
        var func = async ()=> await Aircraft.CreateAsync(
            AircraftManufacturer.Airbus,
            model,
            manufactureDate,
            passengerSeatingCapacity,
            cruisingSpeed,
            maximumOperatingSpeed,
            aircraftRegistrationCode,
            repository.Object);

        // Then
        (await func.Should().ThrowExactlyAsync<DuplicateAircraftRegistrationCodeException>())
            .And.Code.Should().Be(nameof(DuplicateAircraftRegistrationCodeException));

    }
}
