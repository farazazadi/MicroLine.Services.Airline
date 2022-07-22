using MicroLine.Services.Airline.Domain.Aircraft;

namespace MicroLine.Services.Airline.Tests.Common.Fakes;

public static class Aircraft
{
    public static Domain.Aircraft.Aircraft NewFake(AircraftManufacturer manufacturer, int? capacity = null)
    {
        var model = ValueObjects.AircraftModel.NewFake(manufacturer);
        var manufactureDate = ValueObjects.BirthDate.NewFake();
        var maximumSeatingCapacity = ValueObjects.AircraftMaximumSeatingCapacity.NewFake(capacity);
        var cruisingSpeed = ValueObjects.AircraftCruisingSpeed.NewFake();
        var maximumOperatingSpeed = ValueObjects.AircraftMaximumOperatingSpeed.NewFake();
        var registrationCode = ValueObjects.AircraftRegistrationCode.NewFake();

        return Domain.Aircraft.Aircraft.Create(manufacturer
            , model
            , manufactureDate
            , maximumSeatingCapacity
            , cruisingSpeed
            , maximumOperatingSpeed
            , registrationCode
            );
    }
}